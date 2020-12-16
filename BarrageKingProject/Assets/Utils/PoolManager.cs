using System.Collections.Generic;
using UnityEngine;

namespace Habun
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        [System.Serializable]
        public class PoolData
        {
            public PoolObject prefab;
            public GameObject container;
            public List<GameObject> instances;

            public PoolData(PoolObject prefab)
            {
                container = new GameObject(prefab.name);
                container.transform.SetParent(Instance.transform);
                container.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

                instances = new List<GameObject>();

                this.prefab = prefab;
                this.prefab.gameObject.SetActive(false);
            }

            public GameObject Get()
            {
                if (instances.Count == 0) Rebuild();

                foreach (var instance in instances)
                {
                    if (instance == null)
                    {
                        instances.Remove(instance);
                        continue;
                    }

                    if (!instance.activeSelf)
                    {
                        instance.transform.SetParent(container.transform);
                        instance.SetActive(true);
                        return instance;
                    }
                }

                var newInstance = Instantiate(prefab.gameObject);
                newInstance.transform.SetParent(container.transform);
                newInstance.SetActive(true);
                instances.Add(newInstance);

                return newInstance;
            }

            private void Rebuild()
            {
                for (var i = 0; i < prefab.InitCount; ++i)
                {
                    var instance = Instantiate(prefab.gameObject);
                    instance.transform.SetParent(container.transform);
                    instance.SetActive(false);
                    instances.Add(instance);
                }
            }
        }

        // VARIABLES: ---------------------------------------------------------

        private Dictionary<int, PoolData> pool = new Dictionary<int, PoolData>();

        // PUBLIC METHODS: -----------------------------------------------------

        public GameObject Pick(GameObject prefab)
        {
            if (prefab == null) return null;

            var component = prefab.GetComponent<PoolObject>();
            if (component == null)
            {
                component = prefab.AddComponent<PoolObject>();
            }

            return Pick(component);
        }

        public GameObject Pick(PoolObject prefab)
        {
            if (prefab == null) return null;

            var instanceID = prefab.GetInstanceID();
            if (!pool.ContainsKey(instanceID))
            {
                BuildPool(prefab);
            }

            return pool[instanceID].Get();
        }

        // PRIVATE METHODS: ----------------------------------------------------

        private void BuildPool(PoolObject prefab)
        {
            var instanceID = prefab.GetInstanceID();
            pool.Add(instanceID, new PoolData(prefab));
        }

    }
}
