using Cysharp.Threading.Tasks;
using MoreLinq;
using PD.UnityEngineExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Adohi
{
    public class BoxController : MonoBehaviour
    {
        Character character;
        private bool isPushAvailable;
        private bool isConstructAvailable;
        public bool isConstructing = false;

        [Header("Box Prefab")]
        public Box boxPrefab;

        [Header("ContructSetting")]
        public KeyCode constructKey = KeyCode.Mouse0;
        public int maxBoxCount = 5;
        public float jumpHeight = 2f;
        public float consturctDelay = 0.5f;
        public float pushPower;
        public float pushDuration;

        [Header("LockOn")]
        public Box lockOnBox;
        public float minLockOnDistance = 2f;
        
        [Header("PushSetting")]
        public KeyCode pushstructKey = KeyCode.Mouse1;

        public int CurrentBoxCount { get => MapManager.Instance.boxes.Count; }

        private void Awake()
        {
            this.character = GetComponent<Character>();
        }

        private void Start()
        {
            ViewPointManager.Instance.ViewPointChangeConditions += () => ConstuctAvailable();
        }

        private void Update()
        {
            if (PushAvailable())
            {
                switch (ViewPointManager.Instance.currentViewPoint)
                {
                    case ViewPoint.twoDimensional:
                        LockOn2D(character.currentLocation, character.alignDirection);
                        break;
                    case ViewPoint.threeDimensional:
                        LockOn3D(character.transform.position);
                        break;
                }

                if (Input.GetKeyDown(pushstructKey))
                {
                    if (lockOnBox != null)
                    {
                        switch (ViewPointManager.Instance.currentViewPoint)
                        {
                            case ViewPoint.twoDimensional:
                                Push2D(lockOnBox, this.character.currentLocation.OptimalDirectionTo(lockOnBox.location));
                                break;
                            case ViewPoint.threeDimensional:
                                Push3D(lockOnBox, character.XZForward);
                                break;
                        }
                    }
                }
            }

            if (Input.GetKeyDown(constructKey))
            {
                if (ConstuctAvailable())
                {
                    "Construct".Log();
                    switch (ViewPointManager.Instance.currentViewPoint)
                    {
                        case ViewPoint.twoDimensional:
                            Construct2D(character.currentLocation, character.alignDirection, consturctDelay);
                            break;
                        case ViewPoint.threeDimensional:
                            Construct3D(character.transform.position, this.consturctDelay);
                            break;
                    }
                }
            }
        }

        public async UniTask Construct2D(Location characterLocation, Direction direction, float duration)
        {
            isConstructing = true;
            await UniTask.Delay((duration * 1000).ToInt());
            var targetLocation = characterLocation + direction;
            if (MapManager.Instance.IsAvailableLocation(targetLocation))
            {
                var constructedBox = Instantiate(boxPrefab);
                constructedBox.Construct2D(targetLocation);
                MapManager.Instance.AddBox(constructedBox);
            }
            isConstructing = false;
        }

        public async UniTask Construct3D(Vector3 characterPosition, float duration)
        {
            isConstructing = true;
            await Jump(this.jumpHeight, duration);
            int layerMask = LayerMask.GetMask("Ground", "Obstacle", "Box");
            var isHit1 = Physics.Raycast(this.transform.position + jumpHeight * Vector3.up + (Vector3.down + Vector3.left) * 0.5f, Vector3.down, out var hitInfo1, 100f, layerMask);
            var isHit2 = Physics.Raycast(this.transform.position + jumpHeight * Vector3.up + (Vector3.down + Vector3.right) * 0.5f, Vector3.down, out var hitInfo2, 100f, layerMask);
            var isHit3 = Physics.Raycast(this.transform.position + jumpHeight * Vector3.up + (Vector3.down + Vector3.forward) * 0.5f, Vector3.down, out var hitInfo3, 100f, layerMask);
            var isHit4 = Physics.Raycast(this.transform.position + jumpHeight * Vector3.up + (Vector3.down + Vector3.back) * 0.5f, Vector3.down, out var hitInfo4, 100f, layerMask);

            var maxHeight = float.MinValue;
            if (isHit1 || isHit2 || isHit3 || isHit4)
            {
                "ishit".Log();
                if (isHit1)
                {
                    maxHeight = maxHeight > hitInfo1.point.y ? maxHeight : hitInfo1.point.y;

                }
                if (isHit2)
                {
                    maxHeight = maxHeight > hitInfo2.point.y ? maxHeight : hitInfo2.point.y;

                }
                if (isHit3)
                {
                    maxHeight = maxHeight > hitInfo3.point.y ? maxHeight : hitInfo3.point.y;

                }
                if (isHit4)
                {
                    maxHeight = maxHeight > hitInfo4.point.y ? maxHeight : hitInfo4.point.y;
                }
                "Ground Hit".Log();
                var targetPosition = this.transform.position;
                targetPosition.y = maxHeight + 0.51f;
                var constructedBox = Instantiate(boxPrefab);
                constructedBox.Construct3D(targetPosition);
                MapManager.Instance.AddBox(constructedBox);
            }
            isConstructing = false;
        }

        public async UniTask Jump(float height, float duration)
        {
            await UniTask.Delay((duration * 1000).ToInt());
            this.transform.position += Vector3.up * height;
        }

        void LockOn2D(Location characterLocation, Direction direction)
        {
            //"lockon2d".Log();
            var targetLocation = characterLocation + direction;
            //"targetLocation".Log();

            var box = MapManager.Instance.GetBox(targetLocation);
            if (box != null && !box.IsPushing)
            {
                this.lockOnBox = box;
            }
            else if (box == null)
            {
                //"box is null".Log();

                this.lockOnBox = null;
            }
        }


        void Push2D(Box box, Direction direction)
        {
            //box.Push2DTask(this.pushPower.ToInt(), this.pushDuration, this.character.currentLocation.OptimalDirectionTo(box.location));
            box.Push2DTask(this.pushPower.ToInt(), this.pushDuration, direction);
        }


        void LockOn3D(Vector3 position)
        {
            var box = MapManager.Instance.boxes.MinBy(b => Vector3.Distance(position, b.transform.position)).FirstOrDefault();
            if (box != null && !box.IsPushing)
            {
                this.lockOnBox = box;
            }
            else if (box == null)
            {
                this.lockOnBox = null;
            }
        }

        void Push3D(Box box, Vector3 pushDirectionVector)
        {
            box.Push3DTask(pushPower, pushDuration, pushDirectionVector);
        }

        public bool ConstuctAvailable()
        {
            switch (ViewPointManager.Instance.currentViewPoint)
            {
                case ViewPoint.twoDimensional:
                    return !ViewPointManager.Instance.isViewChanging && !character.characterController.isMoving2D && !this.isConstructing;
                case ViewPoint.threeDimensional:
                    return !ViewPointManager.Instance.isViewChanging && character.characterController.isGround3D && !this.isConstructing;
            }
            return false;
        }

        public bool PushAvailable()
        {
            switch (ViewPointManager.Instance.currentViewPoint)
            {
                case ViewPoint.twoDimensional:
                    return !ViewPointManager.Instance.isViewChanging && !character.characterController.isMoving2D && !this.isConstructing;
                case ViewPoint.threeDimensional:
                    return !ViewPointManager.Instance.isViewChanging && character.characterController.isGround3D && !this.isConstructing;
            }
            return false;
        }


    }
}