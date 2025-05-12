using UnityEditor;
using UnityEngine;

namespace Status.Editor
{
    [CustomPropertyDrawer(typeof(StatModifier))]
    public class StatModifierDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // 한 줄로 두 개 필드를 나누기
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = 6f;

            SerializedProperty typeProp = property.FindPropertyRelative("type");
            SerializedProperty valueProp = property.FindPropertyRelative("value");

            float halfWidth = (position.width - spacing) / 2f;

            Rect typeRect = new Rect(position.x + halfWidth + spacing, position.y, halfWidth, lineHeight);
            Rect valueRect = new Rect(position.x, position.y, halfWidth, lineHeight);

            EditorGUI.PropertyField(typeRect, typeProp, GUIContent.none);
            EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}