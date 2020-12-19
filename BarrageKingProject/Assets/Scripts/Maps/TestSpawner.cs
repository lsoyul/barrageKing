using PD.UnityEngineExtensions;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour
{
    public int width;
    public int length;
    public GameObject spawnPrefab2D;
    public GameObject spawnWallPrefab2D;
    public GameObject spawnPrefab3D;
    public GameObject spawnWallPrefab3D;

    [Button]
    public void Spawn2D()
    {
        this.transform.DestroyChildrenImmediate();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                if (i == 0 || j == 0 || i == width - 1 || j == length - 1)
                {
                    var obj = Instantiate(spawnWallPrefab2D);
                    obj.transform.position = new Vector3(i, 0f, j);
                    obj.transform.parent = this.transform;
                }
                else
                {
                    var obj = Instantiate(spawnPrefab2D);
                    obj.transform.position = new Vector3(i, 0f, j);
                    obj.transform.parent = this.transform;
                }

            }
        }


    }

    [Button]
    public void Spawn3D()
    {
        this.transform.DestroyChildrenImmediate();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                if (i == 0 || j == 0 || i == width - 1 || j == length - 1)
                {
                    var obj = Instantiate(spawnWallPrefab3D);
                    obj.transform.position = new Vector3(i, 0f, j);
                    obj.transform.parent = this.transform;
                }
                else
                {
                    var obj = Instantiate(spawnPrefab3D);
                    obj.transform.position = new Vector3(i, 0f, j);
                    obj.transform.parent = this.transform;
                }

            }
        }
    }
}
