using Adohi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PD.UnityEngineExtensions
{
    public static class CollectionExtensions
    {
        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return collection.Count == 0;
        }

        public static T ImprovedElementAt<T>(this ICollection<T> collection, int index)
        {
            if (collection.Count == 0)
            {
                "collection size is 0".LogError();
                return default(T);
            }
            else if (index >= 0)
            {
                return collection.ElementAt(index % collection.Count);
            }
            else
            {
                ((index % collection.Count + collection.Count) % collection.Count).Log();
                return collection.ElementAt((index % collection.Count + collection.Count) % collection.Count);
            }
        }
        public static void IndexedForEach<T>(this ICollection<T> collection, Action<T, int> action, int interval = 1)
        {
            action.ThrowIfNull();

            var collectionSize = collection.Count;
            for (int i = 0; i < collectionSize; i += interval)
            {
                action(collection.ElementAt(i), i);
            }
        }

        public static bool RemoveIfContain<T>(this ICollection<T> collection, T item)
        {
            item.ThrowIfNull();

            if (collection.Contains(item))
            {
                collection.Remove(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool RemoveFirst<T>(this ICollection<T> collection)
        {
            return collection.Remove(collection.First());
        }

        public static bool RemoveLast<T>(this ICollection<T> collection)
        {
            return collection.Remove(collection.Last());
        }


    }
    public static class DictionaryExtensions
    {
        public static bool SafeAdd<TKey, TValue>(this Dictionary<TKey, TValue> items,
                                             TKey key, TValue value)
        {
            if (!items.ContainsKey(key))
            {
                items.Add(key, value);
                return true;
            }
            return false;
        }

        public static void AddOverwrite<TKey, TValue>(this Dictionary<TKey, TValue> items,
                                             TKey key, TValue value)
        {
            items[key] = value;
        }

        public static TKey GetRandomIndex<TKey>(this Dictionary<TKey, int> items)
        {
            var percentageSum = items.Values.Sum();
            var randomPercentage = UnityEngine.Random.Range(0, percentageSum);
            foreach (var item in items)
            {
                if (item.Value < percentageSum)
                {
                    return item.Key;
                }
                else
                {
                    randomPercentage -= item.Value;
                }
            }
            throw new Exception($"Error");
        }

        public static TKey GetRandomIndex<TKey>(this Dictionary<TKey, float> items)
        {
            var percentageSum = items.Values.Sum();
            var randomPercentage = UnityEngine.Random.Range(0, percentageSum);
            foreach (var item in items)
            {
                if (item.Value < percentageSum)
                {
                    return item.Key;
                }
                else
                {
                    randomPercentage -= item.Value;
                }
            }
            throw new Exception($"Error");
        }
    }

    public static class ListExtensions
    {
        public static List<T> GetRandomSamples<T>(this List<T> items, int m)
        {
            List<T> res = new List<T>();
            int n = items.Count();
            for (int i = n - m; i < n; i++)
            {
                int pos = UnityEngine.Random.Range(0, i + 1);
                T item = items[pos];
                if (res.Contains(item))
                    res.Add(items[i]);
                else
                    res.Add(item);
            }
            return res;
        }

        public static void SetRangeValues<T>(this List<T> items, int start, int end, T value)
        {
            if (items.Count == 0) return;
            if (start < 0) return;
            if (end > items.Count - 1) return;

            for (int i = start; i <= end; i++)
            {
                items[i] = value;
            }
        }

        public static int GetRandomIndex(this List<int> items)
        {
            var percentageSum = items.Sum();
            var randomPercentage = UnityEngine.Random.Range(0, percentageSum);
            for (int i = 0; i < items.Count; i++)
            {
                var percentage = items[i];
                if (randomPercentage < percentage)
                {
                    return i;
                }
                else
                {
                    randomPercentage -= percentage;
                }
            }
            return -1;
        }

        public static int GetRandomIndex(this List<float> items)
        {
            var percentageSum = items.Sum();
            var randomPercentage = UnityEngine.Random.Range(0, percentageSum);
            for (int i = 0; i < items.Count; i++)
            {
                var percentage = items[i];
                if (randomPercentage < percentage)
                {
                    return i;
                }
                else
                {
                    randomPercentage -= percentage;
                }
            }
            return -1;
        }

        public static (int area, int start, int end, int length) GetMaxArea(this IEnumerable<int> hist, int minWidth = -1, int minLength = -1)
        {
            var histList = hist.ToList();

            var heightStack = new Stack<int>();
            var indexStack = new Stack<int>();

            var maxArea = -1;

            var (start, end) = (-1, -1);
            var maxLength = -1;

            for (int i = 0; i < histList.Count; i++)
            {
                if (heightStack.Count == 0 || histList[i] > heightStack.Peek())
                {
                    heightStack.Push(histList[i]);
                    indexStack.Push(i);
                }
                else if (histList[i] < heightStack.Peek())
                {
                    var lastIndex = 0;
                    while (heightStack.Count != 0 && histList[i] < heightStack.Peek())
                    {
                        lastIndex = indexStack.Pop();
                        var width = i - lastIndex;
                        var length = heightStack.Pop();
                        if (width >= minWidth && length >= minLength)
                        {
                            var area = width * length;
                            if (area > maxArea)
                            {
                                start = lastIndex;
                                end = i - 1;
                                maxLength = length;
                                maxArea = area;
                            }
                            //maxArea = Mathf.Max(maxArea, area);
                        }
                    }
                    heightStack.Push(histList[i]);
                    indexStack.Push(i);
                }
            }

            while (heightStack.Count != 0)
            {
                var currentIndex = indexStack.Pop();
                var width = histList.Count - currentIndex;
                var length = heightStack.Pop();
                if (width >= minWidth && length >= minLength)
                {
                    var area = width * length;
                    if (area > maxArea)
                    {
                        start = currentIndex;
                        end = histList.Count - 1;
                        maxLength = length;
                        maxArea = area;
                    }
                    //maxArea = Mathf.Max(maxArea, area);
                }
            }

            return (maxArea, start, end, maxLength);


            /*
            var n = histList.Count();
            // Create an empty stack. The stack  
            // holds indexes of hist[] array  
            // The bars stored in stack are always  
            // in increasing order of their heights.  
            Stack<int> s = new Stack<int>();

            int max_area = 0; // Initialize max area 
            int tp; // To store top of stack 
            int area_with_top; // To store area with top  
                               // bar as the smallest bar 

            // Run through all bars of 
            // given histogram  
            int i = 0;
            while (i < n)
            {
                // If this bar is higher than the  
                // bar on top stack, push it to stack  
                if ((histList[s.Peek()] <= histList[i]) && (s.Count == 0 || (histList[s.Peek()] <= histList[i])))
                {
                    s.Push(i++);
                }

                // If this bar is lower than top of stack, 
                // then calculate area of rectangle with  
                // stack top as the smallest (or minimum   
                // height) bar. 'i' is 'right index' for  
                // the top and element before top in stack 
                // is 'left index'  
                else
                {
                    tp = s.Peek(); // store the top index 
                    s.Pop(); // pop the top 

                    // Calculate the area with hist[tp] 
                    // stack as smallest bar  
                    area_with_top = histList[tp] *
                                   (s.Count == 0 ? i : i - s.Peek() - 1);

                    // update max area, if needed  
                    if (max_area < area_with_top)
                    {
                        max_area = area_with_top;
                    }
                }
            }

            // Now pop the remaining bars from  
            // stack and calculate area with every  
            // popped bar as the smallest bar  
            while (s.Count > 0)
            {
                tp = s.Peek();
                s.Pop();
                area_with_top = histList[tp] *
                               (s.Count == 0 ? i : i - s.Peek() - 1);

                if (max_area < area_with_top)
                {
                    max_area = area_with_top;
                }
            }

            return max_area;
            */
        }
    }

    public static class MultiDimensionalArrayExtensions
    {
        public static IEnumerable<T> Flatten<T>(this T[,] source)
        {
            for (int width = 0; width < source.GetLength(0); width++)
            {
                for (int lenth = 0; lenth < source.GetLength(1); lenth++)
                {
                    yield return source[width, lenth];
                }
            }
        }

        public static IEnumerable<T> Flatten<T>(this T[,,] source)
        {
            for (int width = 0; width < source.GetLength(0); width++)
            {
                for (int length = 0; length < source.GetLength(1); length++)
                {
                    for (int height = 0; height < source.GetLength(2); height++)
                    {
                        yield return source[width, length, height];
                    }
                }
            }
        }

        public static IEnumerable<T> Cast<T>(this T[,] source)
        {
            foreach (T item in source)
            {
                yield return item;
            }
        }

        public static IEnumerable<T> Cast<T>(this T[,,] source)
        {
            foreach (T item in source)
            {
                yield return item;
            }
        }

    }

}

