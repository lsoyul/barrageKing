using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private static Dictionary<GameStatics.BULLET_TYPE, List<GameObject>> bulletPool;

    private static GameObject container;
    private static BulletManager instance;

    public GameObject bulletRoot;

    public List<BulletInfo> bulletInfoList;

    [Serializable]
    public struct BulletInfo
    {
        public GameStatics.BULLET_TYPE bulletType;
        public GameObject prefab;
        public int bulletPoolCount;
    }

    public static BulletManager Instance()
    {
        if (instance == null)
        {
            container = new GameObject();
            container.name = "BulletManager";
            instance = container.AddComponent(typeof(BulletManager)) as BulletManager;
        }

        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<BulletManager>();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        InitManager();
    }

    void InitManager()
    {
        if (bulletInfoList != null && bulletRoot != null)
        {
            bulletPool = new Dictionary<GameStatics.BULLET_TYPE, List<GameObject>>();

            foreach (BulletInfo item in bulletInfoList)
            {
                GameObject go = item.prefab;
                BulletBase newBullet = go.GetComponent<BulletBase>();

                if (bulletPool.ContainsKey(newBullet.bulletType)) continue;

                List<GameObject> newPrefabList = new List<GameObject>();
                bulletPool.Add(newBullet.bulletType, newPrefabList);

                for (int i = 0; i < item.bulletPoolCount; i++)
                {
                    GameObject createdGo = Instantiate(go);
                    createdGo.GetComponent<BulletBase>().onDestroy = OnDestroyBullet;
                    createdGo.SetActive(false);

                    createdGo.transform.parent = bulletRoot.transform;

                    bulletPool[newBullet.bulletType].Add(createdGo);
                }
            }
        }
    }

    public void FireBullet(GameStatics.BULLET_TYPE bulletType, Vector3 startWorldPos, float radius, float speed, float acceleration, Vector3 direction)
    {
        if (bulletPool == null) return;

        if (bulletPool.ContainsKey(bulletType))
        {
            bool poolLimitOver = true;
            foreach (GameObject go in bulletPool[bulletType])
            {
                if (go.activeSelf == false)
                {
                    go.transform.position = startWorldPos;
                    go.SetActive(true);
                    BulletBase bullet = go.GetComponent<BulletBase>();
                    bullet.Fire(bulletType, radius, speed, acceleration, direction);

                    poolLimitOver = false;
                    break;
                }
            }

            if (poolLimitOver == true)
            {
                GameObject newGo = CreateNewBullet(bulletType);

                newGo.transform.parent = bulletRoot.transform;

                newGo.transform.position = startWorldPos;
                newGo.SetActive(true);

                BulletBase bullet = newGo.GetComponent<BulletBase>();
                bullet.Fire(bulletType, radius, speed, acceleration, direction);

                bulletPool[bulletType].Add(newGo);
            }
        }
    }

    public void OnDestroyBullet(BulletBase bulletBase)
    {
        bulletBase.gameObject.SetActive(false);
    }

    private GameObject CreateNewBullet(GameStatics.BULLET_TYPE bulletType)
    {
        if (bulletInfoList == null) return null;

        foreach (BulletInfo item in bulletInfoList)
        {
            if (item.bulletType == bulletType)
            {
                GameObject go = Instantiate(item.prefab);
                go.GetComponent<BulletBase>().onDestroy = OnDestroyBullet;

                return go;
            }
        }

        return null;
    }
}
