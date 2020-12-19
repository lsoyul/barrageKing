using NGenerics.DataStructures.Queues;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adohi
{
    public class TSP
    {
        // Start is called before the first frame update
        public static List<int> GetTSP(int[,] graph, int[] parent, int vertexCount)
        {
            var tree = new int[vertexCount, vertexCount];

            for (int i = 0; i < vertexCount; i++)
            {
                for (int j = 1; j < vertexCount; j++)
                {
                    if (parent[j] == i)
                    {
                        tree[i, j] = graph[i, j];
                    }
                }
            }

            var route = new List<int>();
            var searchStack = new Stack<int>();
            searchStack.Push(0);
            while(searchStack.Count > 0)
            {
                var currentIndex = searchStack.Pop();
                route.Add(currentIndex);
                var currentConnectedNodes = new PriorityQueue<int, int>(PriorityQueueType.Maximum);
                for (int i = 0; i < vertexCount; i++)
                {
                    if (tree[currentIndex, i] > 0)
                    {
                        currentConnectedNodes.Add(i, tree[currentIndex, i]);
                    }
                }

                foreach (var node in currentConnectedNodes)
                {
                    searchStack.Push(node);
                }
                
            }
            route.Add(0);
            return route;
        }
        /*
        public void DFS(int currentIndex, int[,] tree, bool[] visited, int vertexCount)
        {
            var pq = new PriorityQueue<int, int>(PriorityQueueType.Minimum);
            for (int i = 0; i < vertexCount; i++)
            {
                if (tree[currentIndex, tree] > 0 && )
            }
        }
        */
    }

}
