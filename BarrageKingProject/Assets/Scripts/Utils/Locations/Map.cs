using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adohi
{

    public enum MapShapeType
    {
        Rectangle,
        Ellipse
    }

    public enum MapBlockType
    {
        Blank,
        Ground
    }

    public class MapBlock
    {
        public int layer;
        public Location location;

        public MapBlockType mapBlockType;

        public MapBlock(int layer, int width, int length, int height)
        {
            this.layer = layer;
            this.location = new Location(width, length, height);
        }
    }

    public class Map
    {
        public int width;
        public int length;
        public int height;
        public int layer;

        public MapBlock[,,] mapBlocks;

        public Map(int layer, int width, int length)
        {
            this.layer = layer;
            this.width = width;
            this.length = length;

            this.mapBlocks = new MapBlock[layer, width, length];
            for (int i = 0; i < layer; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    for (int k = 0; k < length; k++)
                    {
                        this.mapBlocks[i, j, k] = new MapBlock(i, j, k, 0);
                    }
                }
            }
        }
    }

}
