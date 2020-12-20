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

        [Header("Models")]
        public SpriteRenderer model2D;
        public GameObject model3D;

        [Header("Status")]
        public Location location;
        //public ReactiveProperty<Location> locationProperty;
        public float remainPower;
        public float remainDuration;
        public bool IsPushing { get => remainPower > 0f || remainDuration > 0f; }
        private void Start()
        {
            ViewPointManager.Instance.OnViewChangedStartTo2D += To2DStart;
            ViewPointManager.Instance.OnViewChangedEndTo2D += To2DEnd;
            ViewPointManager.Instance.OnViewChangedStartTo3D += To3DStart;
            ViewPointManager.Instance.OnViewChangedEndTo3D += To3DEnd;
        }
        public void Construct2D(Location location)
        {
            this.location = location;
            this.transform.position = location.ToVector();
            model2D.gameObject.SetActive(true);
        }

        public void Construct3D(Vector3 position)
        {
            this.transform.position = position;
            model3D.gameObject.SetActive(true);
        }

        public void To2DStart()
        {
            cancellationTokenSource?.Cancel();
            model3D.gameObject.SetActive(false);
            this.location = new Location(this.transform.position.x.RoundToInt(), this.transform.position.z.RoundToInt(), 0);
        }

        public void To2DEnd()
        {
            model3D.gameObject.SetActive(true);
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

        public void To3DEnd()
        {
            model2D.gameObject.SetActive(true);
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


    }

}
