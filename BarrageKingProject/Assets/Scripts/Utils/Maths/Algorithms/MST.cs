using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Adohi
{
    class MST
    {
        public static void RandomMST(int vertexCount)
        {
            var graph = new int[vertexCount, vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                for (int j = 0; j < vertexCount; j++)
                {
                    if (i == j)
                    {
                        graph[i, j] = 0;
                    }
                    else
                    {
                        graph[i, j] = UnityEngine.Random.Range(1, 100);
                    }
                }
            }

            PrimMST(graph, vertexCount);
        }

        static int MinKey(int[] key, bool[] mstSet, int vertexCount)
        {

            // Initialize min value 
            int min = int.MaxValue, min_index = -1;

            for (int v = 0; v < vertexCount; v++)
                if (mstSet[v] == false && key[v] < min)
                {
                    min = key[v];
                    min_index = v;
                }

            return min_index;
        }

        // A utility function to print 
        // the constructed MST stored in 
        // parent[] 
        static void PrintMST(int[] parent, int[,] graph, int vertexCount)
        {
            ("Edge \tWeight").Log();
            for (int i = 1; i < vertexCount; i++)
                (parent[i] + " - " + i + "\t" + graph[i, parent[i]]).Log();
        }

        // Function to construct and 
        // print MST for a graph represented 
        // using adjacency matrix representation 
        public static int[] PrimMST(int[,] graph, int vertexCount)
        {

            // Array to store constructed MST 
            int[] parent = new int[vertexCount];

            // Key values used to pick 
            // minimum weight edge in cut 
            int[] key = new int[vertexCount];

            // To represent set of vertices 
            // included in MST 
            bool[] mstSet = new bool[vertexCount];

            // Initialize all keys 
            // as INFINITE 
            for (int i = 0; i < vertexCount; i++)
            {
                key[i] = int.MaxValue;
                mstSet[i] = false;
            }

            // Always include first 1st vertex in MST. 
            // Make key 0 so that this vertex is 
            // picked as first vertex 
            // First node is always root of MST 
            key[0] = 0;
            parent[0] = -1;

            // The MST will have V vertices 
            for (int count = 0; count < vertexCount - 1; count++)
            {

                // Pick thd minimum key vertex 
                // from the set of vertices 
                // not yet included in MST 
                int u = MinKey(key, mstSet, vertexCount);

                // Add the picked vertex 
                // to the MST Set 
                mstSet[u] = true;

                // Update key value and parent 
                // index of the adjacent vertices 
                // of the picked vertex. Consider 
                // only those vertices which are 
                // not yet included in MST 
                for (int v = 0; v < vertexCount; v++)

                    // graph[u][v] is non zero only 
                    // for adjacent vertices of m 
                    // mstSet[v] is false for vertices 
                    // not yet included in MST Update 
                    // the key only if graph[u][v] is 
                    // smaller than key[v] 
                    if (graph[u, v] != 0 && mstSet[v] == false
                        && graph[u, v] < key[v])
                    {
                        parent[v] = u;
                        key[v] = graph[u, v];
                    }
            }

            // print the constructed MST 
            PrintMST(parent, graph, vertexCount);

            return parent;
        }
    }
}

