using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PD.UnityEngineExtensions
{
    public static class SystemRandomExtensions
    {
        public static float NextFloat(this System.Random rand)
        {
            return (float)rand.NextDouble();
        }
        public static float NextFloat(this System.Random rand, float min, float max)
        {
            return (float)rand.NextDouble() * (max - min) + min;
        }

        public static Vector2 InsideUnitCircle(this System.Random rand)
        {
            var angle = rand.NextFloat() * Mathf.PI * 2f;
            var dist = rand.NextFloat().Sqrt();

            return new Vector2(Mathf.Cos(angle) * dist, Mathf.Sin(angle) * dist);
        }
    }
    public static class GetComponentExtensions
    {
        public static T GetSafeComponent<T>(this MonoBehaviour obj) where T : Component
        {
            if (!obj.TryGetComponent<T>(out var component))
            {
                Debug.LogError($"Expected to find component of type { typeof(T).ToString()} but found none", obj);
            }

            return component;
        }
        public static T GetSafeComponent<T>(this GameObject obj) where T : Component
        {
            if (!obj.TryGetComponent<T>(out var component))
            {
                Debug.LogError($"Expected to find component of type { typeof(T).ToString()} but found none", obj);
            }

            return component;
        }

        public static T GetSafeComponent<T>(this Component obj) where T : Component
        {
            if (!obj.TryGetComponent<T>(out var component))
            {
                Debug.LogError($"Expected to find component of type { typeof(T).ToString()} but found none", obj);
            }

            return component;
        }

        public static T GetDynamicComponent<T>(this MonoBehaviour obj) where T : Component
        {
            T component = obj.GetComponent<T>();

            if (component.IsNull())
            {
                Debug.LogWarning
                    ($"Expected to find component of type { typeof(T).ToString() } but found none, it will be added dynamic", obj);

                component = obj.gameObject.AddComponent<T>();
            }

            return component;
        }

        public static T GetDynamicComponent<T>(this GameObject obj) where T : Component
        {
            T component = obj.GetComponent<T>();

            if (component.IsNull())
            {
                Debug.LogWarning
                    ($"Expected to find component of type { typeof(T).ToString() } but found none, it will be added dynamic", obj);

                component = obj.AddComponent<T>();
            }

            return component;
        }

        public static T GetDynamicComponent<T>(this Component obj) where T : Component
        {
            T component = obj.GetComponent<T>();

            if (component.IsNull())
            {
                Debug.LogWarning
                    ($"Expected to find component of type { typeof(T).ToString() } but found none, it will be added dynamic", obj);

                component = obj.gameObject.AddComponent<T>();
            }

            return component;
        }

        public static T GetSafeComponentInFirstChildrenOnly<T>(this MonoBehaviour obj) where T : Component
        {
            T component = obj.gameObject.transform.GetChild(0).GetComponentInChildren<T>();

            if (component.IsNull())
            {
                Debug.LogError($"Expected to find component of type { typeof(T).ToString()} but found none", obj);
            }
            return component;
        }

        public static T GetSafeComponentInFirstChildrenOnly<T>(this GameObject obj) where T : Component
        {
            T component = obj.gameObject.transform.GetChild(0).GetComponentInChildren<T>();

            if (component.IsNull())
            {
                Debug.LogError($"Expected to find component of type { typeof(T).ToString()} but found none", obj);
            }
            return component;
        }

        public static T GetSafeComponentInFirstChildrenOnly<T>(this Component obj) where T : Component
        {
            T component = obj.gameObject.transform.GetChild(0).GetComponentInChildren<T>();

            if (component.IsNull())
            {
                Debug.LogError($"Expected to find component of type { typeof(T).ToString()} but found none", obj);
            }
            return component;
        }

        public static HashSet<T> GetComponentsInChildrenOnly<T>(this MonoBehaviour obj, int minAmount = 0) where T : Component
        {
            HashSet<T> components = new HashSet<T>(obj.GetComponentsInChildren<T>().Where(c => !c.gameObject.Equals(obj.gameObject)));

            if (components.Count < minAmount)
            {
                Debug.LogError($"Expected to find component of amount { minAmount.ToString()} but found { components.Count.ToString() }", obj);
            }
            return components;
        }

        public static HashSet<T> GetComponentsInChildrenOnly<T>(this GameObject obj, int minAmount = 0) where T : Component
        {
            HashSet<T> components = new HashSet<T>(obj.GetComponentsInChildren<T>().Where(c => !c.gameObject.Equals(obj)));

            if (components.Count < minAmount)
            {
                Debug.LogError($"Expected to find component of amount { minAmount.ToString()} but found { components.Count.ToString() }", obj);
            }
            return components;
        }

        public static HashSet<T> GetComponentsInChildrenOnly<T>(this Component obj, int minAmount = 0) where T : Component
        {
            HashSet<T> components = new HashSet<T>(obj.GetComponentsInChildren<T>().Where(c => !c.gameObject.Equals(obj.gameObject)));

            if (components.Count < minAmount)
            {
                Debug.LogError($"Expected to find component of amount { minAmount.ToString()} but found { components.Count.ToString() }", obj);
            }
            return components;
        }

        public static HashSet<T> GetSafeComponentsInChildrenOnly<T>(this MonoBehaviour obj, int minAmount = 0) where T : Component
        {
            HashSet<T> components = new HashSet<T>(obj.GetComponentsInChildren<T>().Where(c => !c.gameObject.Equals(obj.gameObject)));

            if (components.Count == 0)
            {
                Debug.LogError($"Expected to find component of type { typeof(T).ToString()} but found none", obj);
            }
            else if (components.Count < minAmount)
            {
                Debug.LogError($"Expected to find component of amount { minAmount.ToString()} but found { components.Count.ToString() }", obj);
            }
            return components;
        }

        public static HashSet<T> GetSafeComponentsInChildrenOnly<T>(this GameObject obj, int minAmount = 0) where T : Component
        {
            HashSet<T> components = new HashSet<T>(obj.GetComponentsInChildren<T>().Where(c => !c.gameObject.Equals(obj)));

            if (components.Count == 0)
            {
                Debug.LogError($"Expected to find component of type { typeof(T).ToString()} but found none", obj);
            }
            else if (components.Count < minAmount)
            {
                Debug.LogError($"Expected to find component of amount { minAmount.ToString()} but found { components.Count.ToString() }", obj);
            }
            return components;
        }

        public static HashSet<T> GetSafeComponentsInChildrenOnly<T>(this Component obj, int minAmount = 0) where T : Component
        {
            HashSet<T> components = new HashSet<T>(obj.GetComponentsInChildren<T>().Where(c => !c.gameObject.Equals(obj.gameObject)));

            if (components.Count == 0)
            {
                Debug.LogError($"Expected to find component of type { typeof(T).ToString()} but found none", obj);
            }
            else if (components.Count < minAmount)
            {
                Debug.LogError($"Expected to find component of amount { minAmount.ToString()} but found { components.Count.ToString() }", obj);
            }
            return components;
        }

        public static T GetSafeComponentInFirstParentOnly<T>(this MonoBehaviour obj) where T : Component
        {
            T component = obj.gameObject.transform.parent.GetComponentInParent<T>();

            if (component.IsNull())
            {
                Debug.LogError($"Expected to find component of type { typeof(T).ToString()} but found none", obj);
            }
            return component;
        }

        public static T GetSafeComponentInFirstParentOnly<T>(this GameObject obj) where T : Component
        {
            T component = obj.gameObject.transform.parent.GetComponentInParent<T>();

            if (component.IsNull())
            {
                Debug.LogError($"Expected to find component of type { typeof(T).ToString()} but found none", obj);
            }
            return component;
        }

        public static T GetSafeComponentInFirstParentOnly<T>(this Component obj) where T : Component
        {
            T component = obj.gameObject.transform.parent.GetComponentInParent<T>();

            if (component.IsNull())
            {
                Debug.LogError($"Expected to find component of type { typeof(T).ToString()} but found none", obj);
            }
            return component;
        }

        public static HashSet<T> GetSafeComponentsInParentOnly<T>(this MonoBehaviour obj, int minAmount = int.MaxValue) where T : Component
        {
            HashSet<T> components = new HashSet<T>(obj.GetComponentsInParent<T>().Where(c => !c.gameObject.Equals(obj.gameObject)));

            if (components.Count == 0)
            {
                Debug.LogError($"Expected to find component of type { typeof(T).ToString()} but found none", obj);
            }
            else if (components.Count < minAmount)
            {
                Debug.LogError($"Expected to find component of amount { minAmount.ToString()} but found { components.Count.ToString() }", obj);
            }
            return components;
        }

        public static HashSet<T> GetSafeComponentsInParentOnly<T>(this GameObject obj, int minAmount = int.MaxValue) where T : Component
        {
            HashSet<T> components = new HashSet<T>(obj.GetComponentsInParent<T>().Where(c => !c.gameObject.Equals(obj)));

            if (components.Count == 0)
            {
                Debug.LogError($"Expected to find component of type { typeof(T).ToString()} but found none", obj);
            }
            else if (components.Count < minAmount)
            {
                Debug.LogError($"Expected to find component of amount { minAmount.ToString()} but found { components.Count.ToString() }", obj);
            }
            return components;
        }

        public static HashSet<T> GetSafeComponentsInParentOnly<T>(this Component obj, int minAmount = int.MaxValue) where T : Component
        {
            HashSet<T> components = new HashSet<T>(obj.GetComponentsInParent<T>().Where(c => !c.gameObject.Equals(obj.gameObject)));

            if (components.Count == 0)
            {
                Debug.LogError($"Expected to find component of type { typeof(T).ToString()} but found none", obj);
            }
            else if (components.Count < minAmount)
            {
                Debug.LogError($"Expected to find component of amount { minAmount.ToString()} but found { components.Count.ToString() }", obj);
            }
            return components;
        }
    }

    public static class UnityObjectExtensions
    {
        public static void Destroy<T>(this T obj) where T : Object
        {
            Object.Destroy(obj);
        }

        public static void Destroy<T>(this T obj, float t) where T : Object
        {
            Object.Destroy(obj, t);
        }

        public static void DestroyImmediate<T>(this T obj) where T : Object
        {
            Object.DestroyImmediate(obj);
        }

        public static void DestroyImmediate<T>(this T obj, bool allowDestroyingAssets) where T : Object
        {
            Object.DestroyImmediate(obj, allowDestroyingAssets);
        }
    }
    public static class TransformExtensions
    {
        public static void SetXPosition(this Transform transform, float x)
        {
            var newPosition = transform.position;
            newPosition.x = x;
            transform.position = newPosition;
        }

        public static void SetYPosition(this Transform transform, float y)
        {
            var newPosition = transform.position;
            newPosition.y = y;
            transform.position = newPosition;
        }

        public static void SetZPosition(this Transform transform, float z)
        {
            var newPosition = transform.position;
            newPosition.z = z;
            transform.position = newPosition;
        }

        public static void AddXPosition(this Transform transform, float x)
        {
            var newPosition = transform.position;
            newPosition.x += x;
            transform.position = newPosition;
        }

        public static void AddYPosition(this Transform transform, float y)
        {
            var newPosition = transform.position;
            newPosition.y += y;
            transform.position = newPosition;
        }

        public static void AddZPosition(this Transform transform, float z)
        {
            var newPosition = transform.position;
            newPosition.z += z;
            transform.position = newPosition;
        }

        public static void Move(this Transform transform, Vector3 velocity)
        {
            transform.position += velocity;
        }

        public static void Move(this Transform transform, Vector3 directionVector, float speed)
        {
            var normalizedDirectionVector = directionVector.normalized;
            transform.position += normalizedDirectionVector * speed;
        }

        public static void MoveTo(this Transform transform, Vector3 destination, float speed)
        {
            var directionVector = (destination - transform.position).normalized;
            var distance = Vector3.Distance(transform.position, destination);
            if (distance <= speed)
            {
                transform.position = destination;
            }
            else
            {
                transform.position += directionVector * speed;
            }
        }

        public static Transform DestroyChildren(this Transform transform)
        {
            while (transform.childCount > 0)
            {
                GameObject.Destroy(transform.GetChild(0).gameObject);
            }
            return transform;
        }

        public static Transform DestroyChildrenImmediate(this Transform transform)
        {
            while (transform.childCount > 0)
            {
                GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
            }
            return transform;
        }

    }

    public static class GameObjectExtensions
    {
        public static void SetColor(this GameObject obj, string colorName, Color color)
        {
            var mat = obj.GetComponent<Renderer>().material;
            mat.SetColor(colorName, color);
        }

    }

    public static class VectorExtensions
    {
        public static Vector3 ToXZVector3(this Vector2 vector, float y = 0) => new Vector3(vector.x, y, vector.y);

        public static Vector2 GetVector2(this Vector3 vector) => new Vector2(vector.x, vector.y);

        public static Vector3 GetVector3(this Vector2 vector, float z = 0) => new Vector3(vector.x, vector.y, z);

        public static Vector2 GetXVector(this Vector2 vector) => new Vector2(vector.x, 0);

        public static Vector3 GetXVector(this Vector3 vector) => new Vector3(vector.x, 0, 0);

        public static Vector2 GetYVector(this Vector2 vector) => new Vector2(0, vector.y);

        public static Vector3 GetYVector(this Vector3 vector) => new Vector3(0, vector.y, 0);

        public static Vector3 GetZVector(this Vector3 vector) => new Vector3(0, 0, vector.z);

        public static Vector3 GetXYVector(this Vector3 vector) => new Vector3(vector.x, vector.y, 0f);
        public static Vector3 GetXZVector(this Vector3 vector) => new Vector3(vector.x, 0, vector.z);
        public static Vector3 GetYZector(this Vector3 vector) => new Vector3(0, vector.y, vector.z);

        public static Vector2 GetRotatedVector(this Vector2 vector, float xAngle, float yAngle, float zAngle) => Quaternion.Euler(xAngle, yAngle, zAngle) * vector;

        public static Vector3 GetRotatedVector(this Vector3 vector, float xAngle, float yAngle, float zAngle) => Quaternion.Euler(xAngle, yAngle, zAngle) * vector;

        public static Vector2 GetRotatedVector(this Vector2 vector, float angle) => GetRotatedVector(vector, 0f, 0f, angle);

        public static Vector2 GetRotatedXVector(this Vector2 vector, float angle)
        {
            var xAxis = Vector2.right.GetRotatedVector(angle);
            return vector.GetRotatedVector(angle).Project(xAxis);
        }

        public static Vector2 GetRotatedYVector(this Vector2 vector, float angle)
        {
            var yAxis = Vector2.up.GetRotatedVector(angle);
            return vector.GetRotatedVector(angle).Project(yAxis);
        }

        public static Vector2 Project(this Vector2 vector, Vector2 projectionTarget) => Vector3.Project(vector, projectionTarget);


        public static Vector2 GetDirectionVector(this Vector2 vector)
        {
            if (vector.x.IsAbsoluteBiggerOrEqual(vector.y))
            {
                return vector.GetXVector();
            }
            else
            {
                return vector.GetYVector();
            }
        }

        public static Vector3 GetDirectionVector(this Vector3 vector)
        {
            if (vector.x.IsAbsoluteBiggerOrEqual(vector.y) && vector.x.IsAbsoluteBiggerOrEqual(vector.z))
            {
                return vector.GetXVector();
            }
            else if (vector.y.IsAbsoluteBiggerOrEqual(vector.x) && vector.y.IsAbsoluteBiggerOrEqual(vector.z))
            {
                return vector.GetYVector();
            }
            else
            {
                return vector.GetZVector();
            }
        }

        public static Vector2 Clamp(this Vector2 vector, float minX, float maxX, float minY, float maxY)
        {
            var x = Mathf.Clamp(vector.x, minX, maxX);
            var y = Mathf.Clamp(vector.y, minY, maxY);

            return new Vector2(x, y);
        }

        public static Vector3 Clamp(this Vector3 vector, float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {
            var x = Mathf.Clamp(vector.x, minX, maxX);
            var y = Mathf.Clamp(vector.y, minY, maxY);
            var z = Mathf.Clamp(vector.z, minZ, maxZ);

            return new Vector3(x, y, z);
        }


        public static Vector2 GetRandomVector2() => new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        public static Vector3 GetRandomVector3() => new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        public static Vector3 Round(this Vector3 vector3, int decimalPlaces = 2)
        {
            float multiplier = 1;
            for (int i = 0; i < decimalPlaces; i++)
            {
                multiplier *= 10f;
            }
            return new Vector3(
                Mathf.Round(vector3.x * multiplier) / multiplier,
                Mathf.Round(vector3.y * multiplier) / multiplier,
                Mathf.Round(vector3.z * multiplier) / multiplier);
        }
        public static Vector2 Round(this Vector2 vector, int decimalPlaces = 2)
        {
            float multiplier = 1;
            for (int i = 0; i < decimalPlaces; i++)
            {
                multiplier *= 10f;
            }
            return new Vector2(
                Mathf.Round(vector.x * multiplier) / multiplier,
                Mathf.Round(vector.y * multiplier) / multiplier);
        }

    }



    public static class ColorExtensions
    {
        public static Color GetRChangedColor(this Color color, float r) => color = new Color(r, color.g, color.b, color.a);
        public static Color GetGChangedColor(this Color color, float g) => color = new Color(color.r, g, color.b, color.a);
        public static Color GetBChangedColor(this Color color, float b) => color = new Color(color.r, color.g, b, color.a);
        public static Color GetAChangedColor(this Color color, float a) => color = new Color(color.r, color.g, color.b, a);
        public static bool IsSimilar(this Color a, Color b, float range)
        {
            return (a.r - b.r).Abs() < range && (a.g - b.g).Abs() < range && (a.b - b.b).Abs() < range;
        }

    }

    public static class RectTransformExtensions
    {
        public static void SetLeft(this RectTransform rt, float left)
        {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }

        public static void SetRight(this RectTransform rt, float right)
        {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }

        public static void SetTop(this RectTransform rt, float top)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }

        public static void SetBottom(this RectTransform rt, float bottom)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }

        public static void SetMargin(this RectTransform rt, float margin)
        {
            rt.SetLeft(margin);
            rt.SetRight(margin);
            rt.SetTop(margin);
            rt.SetBottom(margin);
        }
    }

    public static class LayerMaskHelper
    {
        public static bool IndexInMask(this int index, int mask)
        {
            mask &= 1 << index;

            return mask != 0;
        }
    }
}
