using PD.UnityEngineExtensions;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adohi
{
    public class MapSpawner : MonoBehaviour
    {
        public GameObject groundBlockPrefab;
        public GameObject outerWallPrefab;
        public GameObject obstaclePrefab;

        [Header("Map Setting")]
        public int mapWidth = 22;
        public int mapLength = 22;
        public float fillRatio = 10f;

        [Header("Obstacle setting")]
        public int minObstacleHeight;
        public int maxObstacleHeight;


        [Header("Objects")]
        public MapObject[,] mapObjects;


        [Button]
        public int[,] Spawn(int mapWidth = 22, int mapLength = 22, float fillRatio = 10f)
        {
            var map = new int[mapWidth, mapLength];
            mapObjects = new MapObject[mapWidth, mapLength];
            map.Fill(1);


            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapLength; j++)
                {
                    //outerWall
                    if (i == 0 || j == 0 || i == mapWidth - 1 || j == mapLength - 1)
                    {
                        map[i, j] = 0;
                        var wall = Instantiate(outerWallPrefab);
                        wall.transform.position = new Vector3(i, 0f, j);
                        wall.transform.parent = this.transform;
                        var mapObejct = wall.GetComponent<MapObject>();
                        mapObjects[i, j] = mapObejct;
                    }

                    else
                    {
                        //Obstacle
                        if (Random.Range(0f, 100f) < fillRatio)
                        {
                            map[i, j] = 0;
                            var obstacle = Instantiate(obstaclePrefab, new Vector3(i, 0f, j), Quaternion.identity);
                            var mapObejct = obstacle.GetComponent<MapObject>();
                            mapObejct.height = Random.Range(1, 4);
                            mapObjects[i, j] = mapObejct;
                        }
                        //Ground
                        else
                        {
                            var ground = Instantiate(groundBlockPrefab);
                            ground.transform.position = new Vector3(i, 0f, j);
                            var mapObejct = ground.GetComponent<MapObject>();
                            mapObjects[i, j] = mapObejct;
                        }
                    }

                }
            }
            return map;
        }


        
    }

}
