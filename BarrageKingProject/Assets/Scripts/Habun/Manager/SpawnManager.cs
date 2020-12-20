using Adohi;
using PD.UnityEngineExtensions;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Habun
{
    public class SpawnManager : MonoSingleton<SpawnManager>
    {
        [System.Serializable]
        public struct SpawnInfo
        {
            [Space] public PoolObject prefab;
            [Indent] public int spawnCount;
            [Indent] public float spawnInterval;
            [Indent] public float minRadius;
            [Indent] public float maxRadius;
        }

        [Header("Spawn Settings")]
        [SerializeField]
        private GameObjectValueList spawnObjects;

        [Header("Wave Settings")]
        [SerializeField]
        private IntReference waveNumber;
        [SerializeField, Space]
        private List<List<SpawnInfo>> wave = new List<List<SpawnInfo>>();

        // PUBLIC METHODS: ----------------------------------------------------

        public void StartWave()
        {
            NextWave(GameStateDispatcher.GAMESTART);
        }

        public void NextWave(string state)
        {
            if (state == GameStateDispatcher.GAMESTART)
            {
                waveNumber.Value = 0;
                NextWave(default(GameObject));
            }
        }

        public void NextWave(GameObject _)
        {
            if (spawnObjects.Count <= 0)
            {
                var nextWave = wave[waveNumber.Value++];
                StartCoroutine(SpawnWave(nextWave));
            }
        }

        // PRIVATE METHODS: ----------------------------------------------------

        private IEnumerator SpawnWave(List<SpawnInfo> nextWave)
        {
            foreach (var wave in nextWave)
            {
                for (var i = 0; i < wave.spawnCount; i++)
                {
                    var centerPoint = new Location(MapManager.Instance.mapWidth / 2, MapManager.Instance.mapLength / 2).ToVector();
                    var randomRadius = Random.Range(wave.minRadius, wave.maxRadius);
                    var randomPoint = Random.insideUnitCircle * randomRadius;
                    var randomPosition = new Vector3(centerPoint.x + randomPoint.x.RoundToInt(), 0.0f, centerPoint.z + randomPoint.y.RoundToInt());
                    var randomRotation = Quaternion.LookRotation(-1 * randomPosition.normalized);

                    var tempA = randomPosition.x.RoundToInt() > 21 ? 0 : randomPosition.x.RoundToInt();
                    var tempB = randomPosition.z.RoundToInt() > 21 ? 0 : randomPosition.z.RoundToInt();
                    if (!MapManager.Instance.IsAvailableLocation(new Location(tempA, tempB)))
                    {
                        continue;
                    }

                    var instance = PoolManager.Instance.Pick(wave.prefab);
                    instance.transform.SetPositionAndRotation(randomPosition + Vector3.up, randomRotation);

                    yield return new WaitForSeconds(wave.spawnInterval);
                }
            }
        }

        // MONOBEHAVIOUR METHODS: ----------------------------------------------------

        private void OnEnable()
        {
            IngameTaskManager.Instance.OnStartGame += StartWave;
        }

        private void OnDisable()
        {
            IngameTaskManager.Instance.OnStartGame -= StartWave;
        }

    }
}
