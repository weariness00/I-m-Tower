using UnityEditor;
using UnityEngine;

namespace Util.AStar.Editor
{
    [CustomPropertyDrawer(typeof(PathFindDirection))]
    public class PathFindDirectionDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 4f + 3f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty directionsProp = property.FindPropertyRelative("directions");

            if (directionsProp.arraySize != 8)
            {
                directionsProp.arraySize = 8;
            }

            EditorGUI.LabelField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), label);

            float cellSize = 18f; // 셀 하나 크기 고정 (적당히 작게)
            float padding = 5f;   // 셀 사이 간격
            float startX = position.x;
            float startY = position.y + EditorGUIUtility.singleLineHeight;

            int dataIdx = 0;

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Rect cellRect = new Rect(startX + x * (cellSize + padding), startY + y * (cellSize + padding), cellSize, cellSize);

                    if (x == 1 && y == 1)
                    {
                        GUI.enabled = false;
                        EditorGUI.Toggle(cellRect, false);
                        GUI.enabled = true;
                    }
                    else
                    {
                        SerializedProperty boolProp = directionsProp.GetArrayElementAtIndex(dataIdx);
                        boolProp.boolValue = EditorGUI.Toggle(cellRect, boolProp.boolValue);
                        dataIdx++;
                    }
                }
            }
        }
    }
}