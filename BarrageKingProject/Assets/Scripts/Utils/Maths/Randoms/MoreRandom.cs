using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PD.Utils
{
    public static class MoreRandom
    {
        public static int seed;

        public static Vector2 OnUnitSquare()
        {
            float x, y;
            var randomAxis = Random.Range(0, 2);

            if (randomAxis == 0)
            {
                x = Random.Range(-1f, 1f);
                y = BoolRandom() ? 1f : -1f;
            }
            else
            {
                y = Random.Range(-1f, 1f);
                x = BoolRandom() ? 1f : -1f;
            }

            return new Vector3(x, y);
        }

        public static Vector2 InsideUnitSquare()
        {
            var (x, y) = (Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            return new Vector2(x, y);
        }

        public static Vector2Int OnGirdSquare(int width, int length)
        {
            return OnGirdSquare(0, width - 1, 0, length - 1);
        }

        public static Vector2Int OnGirdSquare(int minX, int maxX, int minY, int maxY)
        {
            int x, y;
            var randomAxis = Random.Range(0, 2);

            if (randomAxis == 0)
            {
                x = Random.Range(minX, maxX + 1);
                y = BoolRandom() ? minY : maxY;
            }
            else
            {
                y = Random.Range(minY, maxY + 1);
                x = BoolRandom() ? minX : maxX;
            }

            return new Vector2Int(x, y);
        }

        public static Vector2Int InSideGirdSquare(int width, int length)
        {
            return InSideGirdSquare(0, width - 1, 0, length - 1);
        }

        public static Vector2Int InSideGirdSquare(int minX, int maxX, int minY, int maxY)
        {
            return new Vector2Int(Random.Range(minX, maxX + 1), Random.Range(minY, maxY + 1));
        }
        public static Vector3 OnUnitBox()
        {
            float x, y, z;
            var randomAxis = Random.Range(0, 3);

            if (randomAxis == 0)
            {
                x = Random.Range(-1f, 1f);
                y = MoreRandom.BoolRandom() ? 1f : -1f;
                z = MoreRandom.BoolRandom() ? 1f : -1f;
            }
            else if (randomAxis == 0)
            {
                y = Random.Range(-1f, 1f);
                x = MoreRandom.BoolRandom() ? 1f : -1f;
                z = MoreRandom.BoolRandom() ? 1f : -1f;
            }
            else
            {
                z = Random.Range(-1f, 1f);
                x = MoreRandom.BoolRandom() ? 1f : -1f;
                y = MoreRandom.BoolRandom() ? 1f : -1f;
            }

            return new Vector3(x, y, z);
        }

        public static Vector3 InsideUnitBox()
        {
            var (x, y, z) = (Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            return new Vector3(x, y, z);
        }

        public static Vector3 OnUnitSphere()
        {
            return Random.onUnitSphere;
        }

        public static bool BoolRandom() => Random.Range(0, 2) == 1;
    }

}
