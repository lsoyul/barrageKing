﻿using UnityEditor;
using UnityEngine;

namespace CloudFine
{
    // IngredientDrawer
    [CustomPropertyDrawer(typeof(Shape))]
    public class ShapeDrawer : PropertyDrawer
    {
        private float totalHeight;
        private Rect propRect;

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            
            EditorGUI.BeginProperty(position, label, property);

            var shapeProperty = property.FindPropertyRelative("type");
            var radiusProperty = property.FindPropertyRelative("radius");
            var lengthProperty = property.FindPropertyRelative("length");

            totalHeight = 0;
            propRect = new Rect(position.x, position.y+ totalHeight, position.width, EditorGUIUtility.singleLineHeight);

            totalHeight += DrawProperty(property, propRect, "type");

            switch ((Shape.ShapeType)property.FindPropertyRelative("type").enumValueIndex)
            {
                case Shape.ShapeType.POINT:
                    totalHeight += DrawProperty(property, propRect, "radius");
                    break;
                case Shape.ShapeType.SPHERE:
                    totalHeight += DrawProperty(property, propRect, "radius");
                    break;
                case Shape.ShapeType.LINE:
                    totalHeight += DrawProperty(property, propRect, "radius");
                    totalHeight += DrawProperty(property, propRect, "length");
                    break;
                case Shape.ShapeType.CYLINDER:
                    totalHeight += DrawProperty(property, propRect, "radius");
                    totalHeight += DrawProperty(property, propRect, "length");
                    break;
            }
            //EditorGUI.indentLevel = indent;
            totalHeight -= 2;
            EditorGUI.EndProperty();
        }

        private float DrawProperty(SerializedProperty property, Rect rect, string propertyname)
        {
            SerializedProperty prop = property.FindPropertyRelative(propertyname);
            EditorGUI.PropertyField(rect, prop);
            float height = EditorGUI.GetPropertyHeight(prop) + 2;
            propRect.y += height;
            return height;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return totalHeight;
        }
    }
}