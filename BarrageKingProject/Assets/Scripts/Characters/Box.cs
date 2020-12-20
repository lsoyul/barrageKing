using Cysharp.Threading.Tasks;
using PD.UnityEngineExtensions;
using Sirenix.OdinInspector;
using System.Threading;
using UniRx;
using UnityEngine;

namespace Adohi
{
    public class Box : MonoBehaviour
    {
        private CancellationTokenSource cancellationTokenSource;
        private Direction pushingDirection2D;
        private Vector3 pushingDirection3D;
        private Rigidbody rb;
        [Header("Models")]
        public SpriteRenderer model2D;
        public GameObject model3D;

        [Header("Status")]
        public Location location;
        //public ReactiveProperty<Location> locationProperty;
        public float remainPower;
        public float remainDuration;
        public bool isGround3D;
        public float velocityY;
        public bool IsPushing { get => remainPower > 0f || remainDuration > 0f; }
        private void Awake()
        {
            this.rb = GetComponent<Rigidbody>();
        }
        private void Start()
        {
            ViewPointManager.Instance.OnViewChangedStartTo2D += To2DStart;
            ViewPointManager.Instance.OnViewChangedMiddleTo2D += To2DMiddle;
            ViewPointManager.Instance.OnViewChangedEndTo2D += To2DEnd;

            ViewPointManager.Instance.OnViewChangedStartTo3D += To3DStart;
            ViewPointManager.Instance.OnViewChangedMiddleTo3D += To3DMiddle;
            ViewPointManager.Instance.OnViewChangedEndTo3D += To3DEnd;
        }

        private void Update()
        {
            if (!ViewPointManager.Instance.isViewChanging && ViewPointManager.Instance.currentViewPoint == ViewPoint.threeDimensional)
            {
                CheckGround(0.6f);
                Gravity();
                this.transform.position += Vector3.up * velocityY * Time.deltaTime;
            }
        }

        public void Construct2D(Location location)
        {
            this.location = location;
            this.transform.position = location.ToVector();
            model2D.gameObject.SetActive(true);
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        }

        public void Construct3D(Vector3 position)
        {
            this.transform.position = position;
            model3D.gameObject.SetActive(true);
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        }

        public void To2DStart()
        {
            cancellationTokenSource?.Cancel();
            model3D.gameObject.SetActive(false);
        }

        public void To2DMiddle()
        {
            model2D.gameObject.SetActive(true);
            rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;


            this.location = new Location(this.transform.position.x.RoundToInt(), this.transform.position.z.RoundToInt(), 0);
            this.transform.position = location.ToVector();
        }

        public void To2DEnd()
        {
            if (this.remainPower > 0f && this.remainDuration > 0f)
            {
                var direction = pushingDirection3D.ToDirection();
                Push2DTask(this.remainPower.RoundToInt(), this.remainDuration, direction);
            }
        }

        public void To3DStart()
        {
            cancellationTokenSource?.Cancel();
            model2D.gameObject.SetActive(false);
            this.transform.position = this.location.ToVector();
        }

        public void To3DMiddle()
        {
            model3D.gameObject.SetActive(true);
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

            int layerMask = LayerMask.GetMask("Ground", "Obstacle", "Box");
            var isHit1 = Physics.Raycast(this.transform.position + Vector3.left * 0.5f + Vector3.up * 20f, Vector3.down, out var hitInfo1, 100f, layerMask);
            var isHit2 = Physics.Raycast(this.transform.position + Vector3.right * 0.5f + Vector3.up * 20f, Vector3.down, out var hitInfo2, 100f, layerMask);
            var isHit3 = Physics.Raycast(this.transform.position + Vector3.forward * 0.5f + Vector3.up * 20f, Vector3.down, out var hitInfo3, 100f, layerMask);
            var isHit4 = Physics.Raycast(this.transform.position + Vector3.back * 0.5f + Vector3.up * 20f, Vector3.down, out var hitInfo4, 100f, layerMask);

            var maxHeight = float.MinValue;
            if (isHit1 || isHit2 || isHit3 || isHit4)
            {
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
                this.isGround3D = true;
                this.transform.SetYPosition(maxHeight + 0.51f);
            }
        }

        public void To3DEnd()
        {
            if (this.remainPower > 0f && this.remainDuration > 0f)
            {
                var direction = pushingDirection2D.ToVector();
                Push3DTask(this.remainPower.RoundToInt(), this.remainDuration, direction);
            }
        }

        [Button]
        public async UniTask Push2DTask(int power, float duration, Direction direction)
        {
            cancellationTokenSource = new CancellationTokenSource();
            this.pushingDirection2D = direction;
            var startPosition = this.transform.position;
            var currentTime = 0f;
            var currentMoveLength = 0;
            this.remainPower = power;
            while (!cancellationTokenSource.Token.IsCancellationRequested && currentTime < duration)
            {
                currentTime += Time.deltaTime;
                var ratio = currentTime / duration;
                var distance = power * (1f - (1f - ratio).Pow(2));
                if (distance.CeilToInt() > currentMoveLength)
                {
                    var nextLocation = this.location + direction;
                    if (!MapManager.Instance.IsAvailableLocation(nextLocation.X, nextLocation.Y))
                    {
                        this.remainPower = 0f;
                        this.remainDuration = 0f;
                        break;
                    }
                    else
                    {
                        currentMoveLength = distance.CeilToInt();
                        this.remainPower = (float)power * (1f - ratio);
                        this.remainDuration = duration - currentTime;
                        //MapManager.Instance.map2D[this.location.X, this.location.Y] = 1;
                        //MapManager.Instance.map2D[nextLocation.X, nextLocation.Y] = 0;
                        this.location = nextLocation;
                    }                 
                }
                this.transform.position = startPosition + (direction.ToVector() * distance);

                await UniTask.DelayFrame(1);
            }
            if (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                this.remainPower = 0f;
                this.remainDuration = 0f;
                this.pushingDirection2D = Direction.None;
                this.pushingDirection3D = Vector3.zero;
            }
            this.transform.position = this.location.ToVector();
        }


        [Button]
        public async UniTask Push3DTask(float power, float duration, Vector3 directionVector)
        {
            cancellationTokenSource = new CancellationTokenSource();
            this.pushingDirection3D = directionVector;

            var currentTime = 0f;
            while (!cancellationTokenSource.Token.IsCancellationRequested && currentTime < duration)
            {
                currentTime += Time.deltaTime;
                var ratio = currentTime / duration;
                var velocitiy = 2 * power / duration * (1f - ratio);
                this.transform.position += directionVector * velocitiy * Time.deltaTime;
                this.remainPower = (float)power * (1f - ratio);
                this.remainDuration = duration - currentTime;
                await UniTask.DelayFrame(1);
            }

            if (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                this.remainPower = 0f;
                this.remainDuration = 0f;
                this.pushingDirection2D = Direction.None;
                this.pushingDirection3D = Vector3.zero;
            }
        }

        public void CheckGround(float rayDistance)
        {
            int layerMask = LayerMask.GetMask("Ground", "Obstacle", "Box");
            var isHit1 = Physics.Raycast(this.transform.position + (Vector3.down + Vector3.left) * 0.5f, Vector3.down, out var hitInfo1, 0.2f, layerMask);
            var isHit2 = Physics.Raycast(this.transform.position + (Vector3.down + Vector3.right) * 0.5f, Vector3.down, out var hitInfo2, 0.2f, layerMask);
            var isHit3 = Physics.Raycast(this.transform.position + (Vector3.down + Vector3.forward) * 0.5f, Vector3.down, out var hitInfo3, 0.2f, layerMask);
            var isHit4 = Physics.Raycast(this.transform.position + (Vector3.down + Vector3.back) * 0.5f, Vector3.down, out var hitInfo4, 0.2f, layerMask);

            var maxHeight = float.MinValue;
            if (isHit1 || isHit2 || isHit3 || isHit4)
            {
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
                this.isGround3D = true;
                this.transform.SetYPosition(maxHeight + 0.51f);
            }
            else
            {
                this.isGround3D = false;
            }
        }
        public void Gravity()
        {
            if (!isGround3D)
            {
                velocityY -= 9.8f * Time.deltaTime;
            }
            else
            {
                velocityY = 0f;
            }
        }

    }

}
