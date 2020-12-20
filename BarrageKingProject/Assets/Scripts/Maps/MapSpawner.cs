using Cysharp.Threading.Tasks;
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
        public List<MapObject> mapObjects;

        public async UniTask<int[,]> Spawn(int mapWidth = 22, int mapLength = 22, float fillRatio = 10f)
        {
            this.transform.DestroyChildrenImmediate();
            var map = new int[mapWidth, mapLength];
            mapObjects = new List<MapObject>();
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
                        wall.transform.position = new Vector3(i, 1f, j);
                        wall.transform.parent = this.transform;
                        var mapObejct = wall.GetComponent<MapObject>();
                        mapObjects.Add(mapObejct);
                        mapObejct.InitObject();
                    }

                    else
                    {
                        var ground = Instantiate(groundBlockPrefab);
                        ground.transform.position = new Vector3(i, 0f, j);
                        ground.transform.parent = this.transform;
                        var mapObejct = ground.GetComponent<MapObject>();
                        mapObjects.Add(mapObejct);


                        mapObejct.InitObject();
                        //Obstacle
                        if (Random.Range(0f, 100f) < fillRatio && (CharacterManager.Instance.initialLocation.X != i && CharacterManager.Instance.initialLocation.Y != j))
                        {
                            map[i, j] = 0;
                            var obstacle = Instantiate(obstaclePrefab, new Vector3(i, 1f, j), Quaternion.identity);
                            obstacle.transform.parent = this.transform;
                            var mapObejcts = obstacle.GetComponent<MapObject>();
                            mapObejcts.height = Random.Range(1, 4);
                            mapObjects.Add(mapObejcts);
                            mapObejcts.InitObject();

                        }
                    }

                }
            }
            "Map Spawn Complete".Log();
            return map;
        }

    }

}
