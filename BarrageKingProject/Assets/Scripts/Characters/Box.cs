using Cysharp.Threading.Tasks;
using PD.UnityEngineExtensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Adohi
{
    public class Box : MonoBehaviour
    {
        public Location location;

        private void Start()
        {
            Construct2D(new Location(5, 5));
        }
        public void Construct2D(Location location)
        {
            this.location = location;
            this.transform.position = location.ToVector();
        }

        public void To2D()
        {
            this.location = new Location(this.transform.position.x.RoundToInt(), this.transform.position.z.RoundToInt(), 0);
        }

        public void To3D()
        {
            this.location = new Location(this.transform.position.x.RoundToInt(), this.transform.position.z.RoundToInt(), 0);
        }

        [Button]
        public async UniTask Push2DTask(int power, float duration, Direction direction)
        {
            var startPosition = this.transform.position;
            var currentTime = 0f;
            var currentMoveLength = 0;
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                var ratio = currentTime / duration;
                var distance = power * (1f - (1f - ratio).Pow(2));
                distance.Log();
                if (distance.CeilToInt() > currentMoveLength)
                {
                    var nextLocation = this.location + direction;
                    if (MapManager.Instance.map2D[nextLocation.X, nextLocation.Y] == 0)
                    {
                        break;
                    }
                    else
                    {
                        currentMoveLength = distance.CeilToInt();
                        MapManager.Instance.map2D[this.location.X, this.location.Y] = 1;
                        MapManager.Instance.map2D[nextLocation.X, nextLocation.Y] = 0;
                        this.location = nextLocation;
                    }                 
                }
                this.transform.position = startPosition + (direction.ToVector() * distance);

                await UniTask.DelayFrame(1);
            }
            this.transform.position = this.location.ToVector();
        }
    }

}
