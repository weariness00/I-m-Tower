using UnityEditor;
using UnityEngine;

public class InspectorReadOnlyAttribute : PropertyAttribute { }

[CustomPropertyDrawer(typeof(InspectorReadOnlyAttribute))]
public class InspectorReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(pos, prop, label, true);
        GUI.enabled = true;
    }
}