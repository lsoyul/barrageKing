/*
The MIT License (MIT)
Copyright (c) 2015 Scissor Lee
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

//using System;
using System.Collections.Generic;
using UnityEngine;

namespace Adohi
{

    public enum TextColor
    {
        Aqua,
        Black,
        Blue,
        Brown,
        Cyan,
        DarkBlue,
        Fuchsia,
        Green,
        Grey,
        Lime,
        Magenta,
        Maroon,
        Navy,
        Olive,
        Orange,
        Purple,
        Red,
        Silver,
        Teal,
        White,
        Yellow
    }

    public static class Debug
    {
        private static readonly int DEFAULT_SIZE = 12;
        private static readonly TextColor DEFAULT_COLOR = TextColor.Black;

        public static bool IsEnabled = true;

        public static int FontSize = DEFAULT_SIZE;
        public static bool IsBold = false;
        public static bool IsItalic = false;
        public static TextColor TextColor = TextColor.Black;

        #region LogFilter

        private static IList<string> m_objects = new List<string>();

        public static void Add(object obj)
        {
            var name = obj.GetType().FullName;
            if (!m_objects.Contains(name))
            {
                m_objects.Add(name);
            }
        }

        public static void Remove(object obj)
        {
            var name = obj.GetType().FullName;
            if (m_objects.Contains(name))
            {
                m_objects.Remove(name);
            }
        }

        public static void Log(object obj, object message)
        {
            var name = obj.GetType().FullName;
            if (m_objects.Contains(name))
            {
                Log(obj.AddName(message));
            }
        }

        public static void LogWarning(object obj, object message)
        {
            var name = obj.GetType().FullName;
            if (m_objects.Contains(name))
            {
                LogWarning(obj.AddName(message));
            }
        }

        public static void LogError(object obj, object message)
        {
            var name = obj.GetType().FullName;
            if (m_objects.Contains(name))
            {
                LogError(obj.AddName(message));
            }
        }

        public static void LogBold(object obj, object message)
        {
            var name = obj.GetType().FullName;
            if (m_objects.Contains(name))
            {
                Log((obj.AddName(message).ApplyBold()));
            }
        }

        public static void LogItalic(object obj, object message)
        {
            var name = obj.GetType().FullName;
            if (m_objects.Contains(name))
            {
                Log((obj.AddName(message).ApplyItalic()));

            }
        }

        public static void LogColor(object obj, object message, string color)
        {
            var name = obj.GetType().FullName;
            if (m_objects.Contains(name))
            {
                Log((obj.AddName(message).ApplyColor(color)));
            }
        }

        public static void LogColor(object obj, object message, TextColor color)
        {
            Log((obj.AddName(message).ApplyColor(color)));
        }

        #endregion

        #region Log RichText

        public static void LogSize(object message, int size)
        {
            Log(message.ApplySize(size));
        }

        public static void LogBold(object message)
        {
            Log(message.ApplyBold());
        }

        public static void LogItalic(object message)
        {
            Log(message.ApplyItalic());
        }

        public static void LogColor(object message, string color)
        {
            Log(message.ApplyColor(color));
        }

        public static void LogColor(object message, TextColor color)
        {
            Log(message.ApplyColor(color));
        }
        #endregion

        #region Log

        public static void Log(object message)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.Log(ApplyCurrentStyle(message));
            }
        }
        public static void Log(object message, Object context)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.Log(ApplyCurrentStyle(message), context);
            }
        }
        public static void LogFormat(string format, params object[] args)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.LogFormat(format, args);
            }
        }
        public static void LogFormat(Object context, string format, params object[] args)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.LogFormat(context, format, args);
            }
        }

        #endregion

        #region Error

        public static void LogError(object message)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.LogError(ApplyCurrentStyle(message));
            }
        }
        public static void LogError(object message, UnityEngine.Object context)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.LogError(ApplyCurrentStyle(message), context);
            }
        }
        public static void LogErrorFormat(string format, params object[] args)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.LogErrorFormat(format, args);
            }
        }
        public static void LogErrorFormat(Object context, string format, params object[] args)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.LogErrorFormat(context, format, args);
            }
        }

        #endregion

        #region Warning

        public static void LogWarning(object message)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.LogWarning(ApplyCurrentStyle(message));
            }
        }
        public static void LogWarning(object message, Object context)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.LogWarning(ApplyCurrentStyle(message), context);
            }
        }
        public static void LogWarningFormat(string format, params object[] args)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.LogWarningFormat(format, args);
            }
        }
        public static void LogWarningFormat(Object context, string format, params object[] args)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.LogWarningFormat(context, format, args);
            }
        }

        #endregion

        #region exception

        public static void LogException(System.Exception exception)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.LogException(exception);
            }
        }
        public static void LogException(System.Exception exception, UnityEngine.Object context)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.LogException(exception, context);
            }
        }

        #endregion

        #region assert

        public static void Assert(bool condition)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.Assert(condition);
            }
        }

        public static void Assert(bool condition, object message)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.Assert(condition, message);
            }
        }
        public static void Assert(bool condition, Object context)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.Assert(condition, context);
            }
        }
        public static void Assert(bool condition, string message)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.Assert(condition, message);
            }
        }
        public static void Assert(bool condition, object message, Object context)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.Assert(condition, message, context);
            }
        }
        public static void Assert(bool condition, string message, Object context)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.Assert(condition, message, context);
            }
        }

        #endregion

        #region line

        public static void DrawLine(Vector3 start, Vector3 end)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.DrawLine(start, end);
            }
        }

        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.DrawLine(start, end, color);
            }
        }

        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.DrawLine(start, end, color, duration);
            }
        }

        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
            }
        }

        #endregion

        #region lay

        public static void DrawRay(Vector3 start, Vector3 direction)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.DrawRay(start, direction);
            }
        }

        public static void DrawRay(Vector3 start, Vector3 direction, Color color)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.DrawRay(start, direction, color);
            }
        }

        public static void DrawRay(Vector3 start, Vector3 direction, Color color, float duration)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.DrawRay(start, direction, color, duration);
            }
        }

        public static void DrawRay(Vector3 start, Vector3 direction, Color color, float duration, bool depthTest)
        {
            if (IsEnabled)
            {
                UnityEngine.Debug.DrawRay(start, direction, color, duration, depthTest);
            }
        }

        #endregion

        #region draw

        //Use DebugPlus

        #endregion
        public static object ApplyCurrentStyle(object message)
        {
            var returnMessage = message;
            if (IsBold)
            {
                returnMessage.ApplyBold();
            }
            if (IsItalic)
            {
                returnMessage.ApplyItalic();
            }
            if (FontSize != DEFAULT_SIZE)
            {
                returnMessage.ApplySize(FontSize);
            }
            if (TextColor != DEFAULT_COLOR)
            {
                returnMessage.ApplyColor(TextColor);
            }
            return returnMessage;
        }
    }

    public static class DebugExtensions
    {
        public static T LogReturnObject<T>(this T o, string name = "")
        {
            Debug.Log($"{name} : {o.ToString()}");
            return o;
        }

        public static void Log(this object o)
        {
            Debug.Log(o.ToString());
        }

        public static void Log(this object o, string suffix)
        {
            Debug.Log($"{o.ToString()}{suffix}");
        }

        public static void Log(this object o, string prefix, string suffix)
        {
            Debug.Log($"{prefix}{o.ToString()}{suffix}");
        }

        public static void LogWarning(this object o)
        {
            Debug.LogWarning(o.ToString());
        }

        public static void LogWarning(this object o, string suffix)
        {
            Debug.LogWarning($"{o.ToString()}{suffix}");
        }

        public static void LogWarning(this object o, string prefix, string suffix)
        {
            Debug.LogWarning($"{prefix}{o.ToString()}{suffix}");
        }

        public static void LogError(this object o)
        {
            Debug.LogError(o.ToString());
        }

        public static void LogError(this object o, string suffix)
        {
            Debug.LogError($"{o.ToString()}{suffix}");
        }

        public static void LogError(this object o, string prefix, string suffix)
        {
            Debug.LogError($"{prefix}{o.ToString()}{suffix}");
        }

        public static void LogOnGUI(this MonoBehaviour monoBehaviour)
        {

        }

        public static object AddName(this object message, object obj)
        {
            return $"[{ obj.GetType().FullName }]: { message.ToString() }";
        }

        public static object ApplyBold(this object message)
        {
            return $"<b>{ message.ToString() }</b>";
        }

        public static object ApplyItalic(this object message)
        {
            return $"<i>{ message.ToString() }</i>";
        }

        public static object ApplyColor(this object message, string color)
        {
            return $"<color={color}>{ message.ToString() }</color>";
        }

        public static object ApplyColor(this object message, TextColor color)
        {
            return $"<color={ color.ToString().ToLower()}>{message.ToString() }</color>";
        }

        public static object ApplySize(this object message, int fontSize = 12)
        {
            return $"<size={ fontSize.ToString()}>{message.ToString() }</size>";
        }
    }

}