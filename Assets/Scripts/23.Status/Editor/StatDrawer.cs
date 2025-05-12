using UnityEditor;
using UnityEngine;
using Util;

namespace Status.Editor
{
    [CustomPropertyDrawer(typeof(Stat))]
    public class StatDrawer : PropertyDrawer
    {
        private static readonly string ValueKey = "baseValue";
        private static readonly string ModifierListKey = "modifierList";

        private const float FoldoutWidth = 15f;
        private bool showModifiers = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var baseValueProp = property.FindPropertyRelative("baseValue");
            var modifiersProp = property.FindPropertyRelative("modifierList");

            float y = position.y;
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = 6f;

            float totalWidth = position.width;

            Rect foldoutRect = new Rect(position.x, y, FoldoutWidth, lineHeight);
            Rect baseLabelRect = new Rect(foldoutRect.xMax + spacing, y, 70f, lineHeight);
            Rect baseValueRect = new Rect(baseLabelRect.xMax + spacing, y, 60f, lineHeight);
            Rect realValueRect = new Rect(baseValueRect.xMax + spacing, y, totalWidth - baseValueRect.xMax - spacing, lineHeight);

            showModifiers = EditorGUI.Foldout(foldoutRect, showModifiers, GUIContent.none);
            EditorGUI.LabelField(baseLabelRect, "Value");
            EditorGUI.PropertyField(baseValueRect, baseValueProp, GUIContent.none);

            // ✅ 안전하게 Stat 인스턴스 추출
            if (property.GetTargetObjectOfProperty() is Stat statObj)
            {
                float realValue = statObj.GetValue();
                EditorGUI.LabelField(realValueRect, $"Real Value: {realValue}");
            }

            y += lineHeight + 2;

            if (showModifiers)
            {
                EditorGUI.PropertyField(new Rect(position.x + 10f, y, totalWidth, EditorGUI.GetPropertyHeight(modifiersProp, true)), modifiersProp, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight + 2;
            if (showModifiers)
            {
                SerializedProperty modifiersProp = property.FindPropertyRelative("modifierList");
                height += EditorGUI.GetPropertyHeight(modifiersProp, true);
            }
            return height;
        }
    }
}