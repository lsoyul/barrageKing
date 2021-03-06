﻿using Adohi;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace Habun
{
    public class Wall : SerializedMonoBehaviour
    {
        public GameStatics.BULLET_TYPE Type
        {
            get { return type; }
            set { type = value; }
        }

        [SerializeField]
        private GameStatics.BULLET_TYPE type = GameStatics.BULLET_TYPE.NONE;

        [SerializeField, Space]
        private Dictionary<GameStatics.BULLET_TYPE, Material> render = new Dictionary<GameStatics.BULLET_TYPE, Material>();
        [SerializeField, Space]
        private Dictionary<GameStatics.BULLET_TYPE, PoolObject> explosion = new Dictionary<GameStatics.BULLET_TYPE, PoolObject>();
        [SerializeField, Space]
        private Dictionary<GameStatics.BULLET_TYPE, int> counter = new Dictionary<GameStatics.BULLET_TYPE, int>();

        [SerializeField, Space]
        private IntUnityEvent onCounter;

        // PUBLIC METHODS: ----------------------------------------------------

        public void Count(GameStatics.BULLET_TYPE type)
        {
            if (!counter.ContainsKey(type))
            {
                counter[type] = 1;
            }
            else
            {
                counter[type] += 1;
            }

            UpdateCounter();
            UpdateRenderer();
        }

        public void Explosion()
        {
            if (explosion.ContainsKey(type))
            {
                var instance = PoolManager.Instance.Pick(explosion[type]);
                instance.transform.SetPositionAndRotation(transform.position, transform.rotation);
            }

            StartCoroutine(Destroy(0.2f));
        }

        private IEnumerator Destroy(float delay)
        {
            var damage = GetComponentInChildren<Damage>(true);
            damage.gameObject.SetActive(true);

            yield return new WaitForSeconds(delay);
            damage.gameObject.SetActive(false);

            MapManager.Instance.RemoveBox(GetComponent<Box>());
            Destroy(gameObject);
        }

        // PRIVATE METHODS: ----------------------------------------------------

        private void UpdateCounter()
        {
            var count = 0;
            foreach (var pair in counter)
            {
                if (count < pair.Value)
                {
                    count = pair.Value;
                    type = pair.Key;
                }
            }

            onCounter.Invoke((int)type);
        }

        private void UpdateRenderer()
        {
            if (!render.ContainsKey(type))
            {
                return;
            }

            var meshRenderer = GetComponentInChildren<MeshRenderer>(true);
            meshRenderer.material = render[type];
        }

        public Health Health { get; private set; }

        private void Awake()
        {
            Health = GetComponent<Health>();
        }

        private void Update()
        {
            if (Health)
            {
                Health.Damage(Time.deltaTime * 33f);
            }
        }

    }
}
