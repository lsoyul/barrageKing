using Cysharp.Threading.Tasks;
using PD.UnityEngineExtensions;
using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public MapObject[,] mapObjects;

        public List<Box> boxes;

        public async UniTask GenerateMap()
        {
            map2D = await spawner.Spawn(mapObjects, mapWidth, mapLength, fillRatio);
            "1".Log();
            mapObjects.Length.Log();
            mapObjects.ForEach(m => m.Show2DObject());
            "2".Log();

        }


        public void AddBox(Box box)
        {
            boxes.Add(box);
        }

        public void RemoveBox(Box box)
        {
            boxes.Remove(box);
        }

        public Box GetBox(Location location)
        {
            return boxes.Where(b => b.location.Equals(location)).FirstOrDefault();
        }
    }

}
