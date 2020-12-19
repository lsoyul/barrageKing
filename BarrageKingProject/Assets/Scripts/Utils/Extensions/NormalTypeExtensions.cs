using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PD.UnityEngineExtensions
{
    public static class NormalTypeTempleteExtensions
    {
        public static void ForEach<T>(this T[,] arr, Action<T> action)
        {
            action.ThrowIfNull();

            foreach (var item in arr)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this T[,,] arr, Action<T> action)
        {
            action.ThrowIfNull();

            foreach (var item in arr)
            {
                action(item);
            }
        }

        public static void IndexedForEach<T>(this T[,] arr, Action<T, int, int> action)
        {
            action.ThrowIfNull();

            var x = arr.GetLength(0);
            var y = arr.GetLength(1);
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    action(arr[i, j], i, j);
                }
            }
        }

        public static void IndexedForEach<T>(this T[,,] arr, Action<T, int, int, int> action)
        {
            action.ThrowIfNull();

            var x = arr.GetLength(0);
            var y = arr.GetLength(1);
            var z = arr.GetLength(2);

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    for (int k = 0; k < z; k++)
                    {
                        action(arr[i, j, k], i, j, k);
                    }
                }
            }
        }

        /// <summary>
        /// Get the array slice between the two indexes.
        /// ... Inclusive for start index, exclusive for end index.
        /// </summary>
        public static T[] Slice<T>(this T[] source, int start, int end)
        {
            start %= source.Length;
            end %= source.Length;

            if (start < 0)
            {
                start = source.Length + start;
            }

            if (end < 0)
            {
                end = source.Length + end;
            }


            int len = end - start;

            // Return new array.
            T[] res = new T[len];
            for (int i = 0; i < len; i++)
            {
                res[i] = source[i + start];
            }
            return res;
        }

        public static void Fill<T>(this T[] source, T value)
        {
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = value;
            }
        }

        public static void Fill<T>(this T[,] source, T value)
        {
            for (int i = 0; i < source.GetLength(0); i++)
            {
                for (int j = 0; j < source.GetLength(1); j++)
                {
                    source[i, j] = value;
                }
            }
        }

        public static void Fill<T>(this T[,,] source, T value)
        {
            for (int i = 0; i < source.GetLength(0); i++)
            {
                for (int j = 0; j < source.GetLength(1); j++)
                {
                    for (int k = 0; k < source.GetLength(2); k++)
                    {
                        source[i, j, k] = value;
                    }
                }
            }
        }
    }
    public static class IntegerExtensions
    {
        public static float ToFloat(this int value) => (float)value;

        public static void For(this int endValue, Action<int> action, int interval = 1)
        {
            action.ThrowIfNull();
            var end = endValue;
            for (int i = 0; i < end; i += interval)
            {
                action(i);
            }
        }
        /// <summary>
        /// Ranged For, start[inclusive] to end[inclusive]
        /// </summary>
        /// <param name="pair"></param>
        /// <param name="action"></param>
        /// <param name="interval"></param>
        public static void ForIncludeEnd(this (int start, int end) pair, Action<int> action, int interval = 1)
        {
            action.ThrowIfNull();

            for (int i = pair.start; i <= pair.end; i += interval)
            {
                action(i);
            }
        }

        public static void ForExceptEnd(this (int start, int end) pair, Action<int> action, int interval = 1)
        {
            action.ThrowIfNull();

            for (int i = pair.start; i < pair.end; i += interval)
            {
                action(i);
            }
        }

        public static void ForAsFloatExceptEnd(this (int start, int end) pair, Action<float> action, float interval = 1)
        {
            action.ThrowIfNull();

            for (float f = pair.start; f < pair.end; f += interval)
            {
                action(f);
            }
        }

        public static bool IsInRange(this int value, int min, int max) => value >= min && value < max;

        public static int Abs(this int value) => Mathf.Abs(value);

        public static bool IsAbsoluteBigger(this int value, int comparisionTarget) => Mathf.Abs(value) > Mathf.Abs(comparisionTarget);

        public static bool IsAbsoluteBiggerOrEqual(this int value, int comparisionTarget) => Mathf.Abs(value) >= Mathf.Abs(comparisionTarget);

        public static bool IsAbsoluteSmaller(this int value, int comparisionTarget) => Mathf.Abs(value) < Mathf.Abs(comparisionTarget);

        public static bool IsAbsoluteSmallerOrEqual(this int value, int comparisionTarget) => Mathf.Abs(value) <= Mathf.Abs(comparisionTarget);

        public static int Clamp(this int value, int min, int max) => Mathf.Clamp(value, min, max);

        public static int ClampMin(this int value, int min) => Mathf.Clamp(value, min, value);

        public static int ClampMax(this int value, int max) => Mathf.Clamp(value, value, max);

        public static int Pow(this int value, int power) => Mathf.Pow(value.ToFloat(), power.ToFloat()).ToInt();

        public static int Pow(this int value, float power) => Mathf.Pow(value.ToFloat(), power).ToInt();

        public static float Sqrt(this int value) => Mathf.Sqrt(value);

        public static int Square(this int value) => Mathf.Pow(value, 2).ToInt();


        public static int Add(this (int, int) pair) => pair.Item1 + pair.Item2;

        public static int Sub(this (int, int) pair) => pair.Item1 - pair.Item2;

        public static int Mul(this (int, int) pair) => pair.Item1 * pair.Item2;

        public static int Div(this (int, int) pair) => pair.Item1 / pair.Item1;

        public static int Max(this (int, int) pair) => Mathf.Max(pair.Item1, pair.Item2);

        public static int Min(this (int, int) pair) => Mathf.Min(pair.Item1, pair.Item2);

        public static int RoundUpDiv(this int a, int b)
        {
            return a / b + (a % b == 0 ? 0 : 1);
        }

    }

    public static class FloatExtensions
    {
        public static int ToInt(this float value) => (int)value;

        public static void For(this float end, Action<float> action, float interval = 1)
        {
            action.ThrowIfNull();

            for (float f = 0; f < end; f += interval)
            {
                action(f);
            }
        }

        public static void For(this (float start, float end) pair, Action<float> action, float interval = 1f)
        {
            action.ThrowIfNull();

            for (float f = pair.start; f <= pair.end; f += interval)
            {
                action(f);
            }
        }


        public static bool IsInRange(this float value, float min, float max) => value >= min && value < max;

        public static float Abs(this float value) => Mathf.Abs(value);

        public static bool IsAbsoluteBigger(this float value, float comparisionTarget) => Mathf.Abs(value) > Mathf.Abs(comparisionTarget);

        public static bool IsAbsoluteBiggerOrEqual(this float value, float comparisionTarget) => Mathf.Abs(value) >= Mathf.Abs(comparisionTarget);

        public static bool IsAbsoluteSmaller(this float value, float comparisionTarget) => Mathf.Abs(value) < Mathf.Abs(comparisionTarget);

        public static bool IsAbsoluteSmallerOrEqual(this float value, float comparisionTarget) => Mathf.Abs(value) <= Mathf.Abs(comparisionTarget);

        public static bool Approximately(this float value, float comparisionTarget) => Mathf.Approximately(value, comparisionTarget);

        public static float Clamp(this float value, float min, float max) => Mathf.Clamp(value, min, max);

        public static float ClampMin(this float value, float min) => Mathf.Clamp(value, min, value);

        public static float ClampMax(this float value, float max) => Mathf.Clamp(value, value, max);


        public static float Pow(this float value, int power) => Mathf.Pow(value, power.ToFloat());

        public static float Pow(this float value, float power) => Mathf.Pow(value, power);

        public static float Sqrt(this float value) => Mathf.Sqrt(value);

        public static float Square(this float value) => Mathf.Pow(value, 2);

        public static float Round(this float value) => Mathf.Round(value);

        public static float Round(this float value, float Threshold, bool isCompriseThreshold = true)
        {
            if (isCompriseThreshold)
            {
                return value % 1 >= Threshold ? value.Ceil() : value.Floor();
            }
            else
            {
                return value % 1 > Threshold ? value.Ceil() : value.Floor();
            }
            
        }

        public static int RoundToInt(this float value) => Mathf.RoundToInt(value);

        public static int RoundToInt(this float value, float Threshold, bool isCompriseThreshold = true) => Round(value, Threshold, isCompriseThreshold).ToInt();

        public static float Ceil(this float value) => Mathf.Ceil(value);

        public static int CeilToInt(this float value) => Mathf.CeilToInt(value);

        public static float Floor(this float value) => Mathf.Floor(value);

        public static int FloorToInt(this float value) => Mathf.FloorToInt(value);

        public static float Add(this (float, float) pair) => pair.Item1 + pair.Item2;

        public static float Sub(this (float, float) pair) => pair.Item1 - pair.Item2;

        public static float Mul(this (float, float) pair) => pair.Item1 * pair.Item2;

        public static float Div(this (float, float) pair) => pair.Item1 / pair.Item1;

        public static float Max(this (float, float) pair) => Mathf.Max(pair.Item1, pair.Item2);

        public static float Min(this (float, float) pair) => Mathf.Min(pair.Item1, pair.Item2);
    }

    public static class BoolFuncExtensions
    {

        public static bool ActionIfTrue(this bool b, Action action)
        {
            if (b)
            {
                action();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ActionIfFalse(this bool b, Action action)
        {
            if (!b)
            {
                action();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool SafeWhile(this Func<bool> b, Action action, int loopCount = 1000)
        {
            action.ThrowIfNull();
            if (action == null) throw new ArgumentNullException(nameof(action));
            while (b() && loopCount-- > 0)
            {
                action();
            }
            return loopCount > 0;
        }

    }
    public static class StringExtensions
    {
        public static string Concatenate(this string[] strings, string token = default)
        {
            if (strings == null) throw new ArgumentNullException(nameof(strings));

            var capacity = strings.Sum(s => s.Length + 1);
            StringBuilder sb = new StringBuilder(capacity);
            strings.ForEach(s =>
            {
                sb.Append(s);
                sb.Append(token);
            });
            return sb.ToString();
        }

        public static string Concatenate(this IEnumerable<string> strings, string token = default)
        {
            if (strings == null) throw new ArgumentNullException(nameof(strings));


            var capacity = strings.Sum(s => s.Length + 1);
            StringBuilder sb = new StringBuilder(capacity);
            strings.ForEach(s =>
            {
                sb.Append(s);
                sb.Append(token);
            });
            return sb.ToString();
        }
    }


    public static class IComparableExtension
    {
        //IEquatable
        public static bool IsBigger<T>(this T equatable, T obj) where T : IComparable
        {
            return equatable.CompareTo(obj) > 0;
        }

        public static bool IsBiggerOrEqual<T>(this T equatable, T obj) where T : IComparable
        {
            return equatable.CompareTo(obj) >= 0;
        }

        public static bool IsSmaller<T>(this T equatable, T obj) where T : IComparable
        {
            return equatable.CompareTo(obj) < 0;
        }

        public static bool IsSmallerOrEqual<T>(this T equatable, T obj) where T : IComparable
        {
            return equatable.CompareTo(obj) <= 0;
        }
    }
    public static class MathExtensions
    {
        

    
    }

    public static class ObjectExtenstions
    {
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }
    }
}
