﻿using Adohi;
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
            [Space] public List<Transform> spawnPositions;
        }

        [Header("Spawn Settings")]
        [SerializeField]
        private GameObjectValueList spawnList;
        [SerializeField]
        private Vector3 spawnOffset;

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
            if (spawnList.Count <= 0)
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
                if (wave.spawnPositions != null && wave.spawnPositions.Count > 0)
                {
                    var randomIndex = Random.Range(0, wave.spawnPositions.Count);
                    var randomPosition = wave.spawnPositions[randomIndex].position;
                    var randomRotation = Quaternion.LookRotation(-1 * randomPosition.normalized);
                    var instance = PoolManager.Instance.Pick(wave.prefab);
                    instance.transform.SetPositionAndRotation(randomPosition + spawnOffset, randomRotation);
                }
                else
                {
                    var centerPoint = new Location(MapManager.Instance.mapWidth / 2, MapManager.Instance.mapLength / 2).ToVector();
                    var randomRadius = Random.Range(wave.minRadius, wave.maxRadius);
                    var randomPoint = Random.insideUnitCircle * randomRadius;
                    var randomPosition = new Vector3(centerPoint.x + randomPoint.x.RoundToInt(), 0.0f, centerPoint.z + randomPoint.y.RoundToInt());
                    var randomRotation = Quaternion.LookRotation(-1 * randomPosition.normalized);
                    var instance = PoolManager.Instance.Pick(wave.prefab);
                    instance.transform.SetPositionAndRotation(randomPosition + spawnOffset, randomRotation);
                }

                yield return new WaitForSeconds(wave.spawnInterval);
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
