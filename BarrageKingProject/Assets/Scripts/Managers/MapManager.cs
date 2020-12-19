using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adohi
{
    public class MapManager : Singleton<MapManager>
    {
        public MapSpawner spawner;
        [Header("GroundSettings")]
        public int mapWidth;
        public int mapLength;

        [Header("FillSettings")]
        public float fillRatio;

        public int[,] map2D;
        void Start()
        {
            map2D = spawner.Spawn(mapWidth, mapLength, fillRatio);
        }
    }

}
