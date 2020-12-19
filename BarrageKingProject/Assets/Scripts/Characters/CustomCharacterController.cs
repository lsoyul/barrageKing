using Cysharp.Threading.Tasks;
using NUnit.Framework;
using PD.UnityEngineExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

namespace Adohi
{
    public class CustomCharacterController : MonoBehaviour
    {
        private Character character;
        private CapsuleCollider collider3D;
        private bool is2DMove = true;
        public KeyCode upKey = KeyCode.W;
        public KeyCode downKey = KeyCode.S;
        public KeyCode leftKey = KeyCode.A;
        public KeyCode rightKey = KeyCode.D;

        public Direction moveDirection2D;

        [Header("Move2D")]      
        public bool is2DMoveAvailable;
        public bool isMoving;
        public int moveFrame;
        public float keyInputAvailableThreshold = 0.5f;
        public bool isKeyInputAvailable = true;

        [Header("Move3D")]
        public bool isGround;
        public Vector3 currentVelocity;
        public float rotateSpeed = 10f;
        public float jumpPower = 5f;
        public Location CurrentLocation
        {
            get
            {
                if (character != null)
                {
                    return character.currentLocation;
                }
                else
                {
                    return Location.Zero;
                }
            }
            set
            {
                if (character != null)
                {
                    character.currentLocation = value;
                }
            }
        }

        [Header("Move3D")]
        public float speed = 1f;

        private void Awake()
        {
            this.character = GetComponent<Character>();
            this.collider3D = GetComponent<CapsuleCollider>();
        }

        private void Start()
        {
            ViewPointManager.Instance.OnViewChangedMiddleTo2D += () => To2DViewStart();
            ViewPointManager.Instance.OnViewChangedMiddleTo3D += () => To3DViewMiddle();
            ViewPointManager.Instance.OnViewChangedEndTo2D += () => To2DViewEnd();
            ViewPointManager.Instance.OnViewChangedEndTo3D += () => To3DViewEnd();

            this.transform.position = CurrentLocation.ToVector();

        }
        private void FixedUpdate()
        {
            if (!ViewPointManager.Instance.isViewChanging)
            {
                if (ViewPointManager.Instance.currentViewPoint == ViewPoint.twoDimensional)
                {
                    if (isKeyInputAvailable)
                    {
                        SetDirection();

                    }
                    if (is2DMoveAvailable && !isMoving)
                    {
                        if (moveDirection2D != Direction.None)
                        {
                            var nextLocation = CurrentLocation + moveDirection2D;
                            if (MapManager.Instance.map2D[nextLocation.X, nextLocation.Y] == 1)
                            {
                                Move2DTask(nextLocation);
                            }
                        }
                    }
                }
                else
                {
                    CheckGround(1.01f);
                    Gravity();
                    Jump3D();
                    Move3D();
                    this.transform.position += this.currentVelocity * Time.deltaTime;
                }
            }
        }
        public void SetDirection()
        {
            if (Input.GetKey(upKey))
            {
                moveDirection2D = Direction.Up;
            }
            if (Input.GetKey(downKey))
            {
                moveDirection2D = Direction.Down;
            }
            if (Input.GetKey(leftKey))
            {
                moveDirection2D = Direction.Left;
            }
            if (Input.GetKey(rightKey))
            {
                moveDirection2D = Direction.Right;
            }
        }

        public async UniTask Move2DTask(Location nextLocation)
        {
            this.isKeyInputAvailable = false;
            this.moveDirection2D = Direction.None;
            is2DMoveAvailable = false;
            await MovePosition2DTask(nextLocation);
            is2DMoveAvailable = true;
            
        }

        public async UniTask MovePosition2DTask(Location location)
        {
            var startPosition = this.transform.position;
            isMoving = true;
            for (int i = 0; i < moveFrame; i++)
            {
                if ((float)(i + 1) / moveFrame > keyInputAvailableThreshold)
                {
                    isKeyInputAvailable = true;
                }
                this.transform.position = Vector3.Lerp(startPosition, location.ToVector(), (float)(i + 1) / moveFrame);
                await UniTask.DelayFrame(1, PlayerLoopTiming.FixedUpdate);
            }
            this.CurrentLocation = location;
            this.transform.position = location.ToVector();
            isMoving = false;
        }

        public void Move3D()
        {
            var directionVector = CalculateDirectionVector();
            Set3DMoveAnimation(directionVector);
            var yAngle = Vector3.SignedAngle(Vector3.forward, character.XZForward, Vector3.up);
            var moveVector = Quaternion.Euler(0f, yAngle, 0f) * directionVector;
            this.currentVelocity.x = (moveVector * speed).x;
            this.currentVelocity.z = (moveVector * speed).z;
            if (moveVector != Vector3.zero)
            {
                this.transform.forward = Vector3.Lerp(this.transform.forward, moveVector, Time.deltaTime * rotateSpeed);
            }
        }

        public void Jump3D()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                "jump".Log();
                if (this.isGround)
                {
                    this.currentVelocity.y = jumpPower;
                }
            }
        }

        public void Gravity()
        {
            if (!isGround)
            {
                this.currentVelocity.y -= 9.8f * Time.deltaTime;
            }
            else
            {
                this.currentVelocity.y = 0f;
            }
        }

        public void CheckGround(float rayDistance)
        {
            if (Physics.Raycast(this.transform.position+Vector3.up, Vector3.down, out var hitinfo, rayDistance, 1 << LayerMask.NameToLayer("Ground")))
            {
                this.isGround = true;
                this.transform.SetYPosition(hitinfo.point.y);
            }
            else
            {
                this.isGround = false;
            }
        }

        public Vector3 CalculateDirectionVector()
        {
            var directionVector = Vector3.zero;
            if (Input.GetKey(upKey))
            {
                directionVector += Vector3.forward;
            }
            if (Input.GetKey(downKey))
            {
                directionVector -= Vector3.forward;
            }
            if (Input.GetKey(leftKey))
            {
                directionVector += Vector3.left;
            }
            if (Input.GetKey(rightKey))
            {
                directionVector += Vector3.right;
            }
            return directionVector;
        }

        public void Set3DMoveAnimation(Vector3 directionalMoveVector)
        {
            if (directionalMoveVector != Vector3.zero)
            {
                this.character.animator3D.SetBool("isMove", true);
            }
            else
            {
                this.character.animator3D.SetBool("isMove", false);
            }
        }

        public void To2DViewStart()
        {
            collider3D.enabled = false;
            CurrentLocation = new Location(this.transform.position.x.Round(), this.transform.position.z.Round(), 0);
            this.transform.position = CurrentLocation.ToVector();
            this.character.alignDirection = this.character.XZForward.ToDirection();
            this.transform.rotation = this.character.alignDirection.ToRotation();
        }

        public void To2DViewEnd()
        {
        }

        public void To3DViewMiddle()
        {
            collider3D.enabled = true;
            var isHit = Physics.Raycast(this.CurrentLocation.ToVector() + Vector3.up * 100f, Vector3.down, out var hitinfo, 200f, 1 << LayerMask.NameToLayer("Ground"));
            if (isHit)
            {
                var height = hitinfo.point.y;
                this.transform.position = this.CurrentLocation.ToVector() + Vector3.up * height;
            }
        }

        public void To3DViewEnd()
        {

        }


    }

}
