using Cysharp.Threading.Tasks;
using NUnit.Framework;
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
        public List<MapObject> mapObjects { get => spawner.mapObjects; }

        public List<Box> boxes;

        public async UniTask GenerateMap()
        {
            map2D = await spawner.Spawn(mapWidth, mapLength, fillRatio);
            mapObjects.ForEach(m => m.Show2DObject());
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

        public bool IsAvailableLocation(Location location)
        {
            return IsAvailableLocation(location.X, location.Y);
        }

        public bool IsAvailableLocation(int x, int y)
        {
            boxes.ForEach(b => b.location.Log());
            var isBoxExist = boxes.Any(b => b.location.Equals(x, y));
            var isObstacle = map2D[x, y] == 0;
            var isCharacter = false;
            if (CharacterManager.Instance.character != null)
            {
                isCharacter = CharacterManager.Instance.character.currentLocation.Equals(x, y);
            }
            return !(isBoxExist || isObstacle || isCharacter);
        }
    }

}
