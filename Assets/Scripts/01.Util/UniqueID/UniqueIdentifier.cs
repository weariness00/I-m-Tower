using System;
using UnityEditor;
using UnityEngine;

namespace Util.UniqueID
{
    [Serializable]
    public struct UniqueIdentifier : IIdentifiable
    {
        public UniqueIdentifier(int value)
        {
            id = value;
        }
        
        public int id;
        public int Id => id;

        public override string ToString() => id.ToString();
        public override int GetHashCode() => id;
        public override bool Equals(object obj) =>
            obj is UniqueIdentifier other && other.id == this.id;

        public static implicit operator int(UniqueIdentifier pid) => pid.id;
        public static implicit operator UniqueIdentifier(int id) => new UniqueIdentifier(id);
    }
    
    #if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(UniqueIdentifier))]
    public class UniqueIdentifierPropertyDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            var idProp = property.FindPropertyRelative("id");
            if (idProp != null)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.PropertyField(position, idProp, label);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Property not found");
            }
        }
    }
    #endif
}