using System.Collections.Generic;
using UnityEditor;
using Util.UniqueID;

namespace Looting.Editor
{
    [CustomEditor(typeof(LootingTableSO))]
    public class LootingTableSOEditor : UnityEditor.Editor
    {
        private SerializedProperty lootingDataArrayProp;

        private void OnEnable()
        {
            lootingDataArrayProp = serializedObject.FindProperty("lootingDataArray");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            for (int i = 0; i < lootingDataArrayProp.arraySize; i++)
            {
                var element = lootingDataArrayProp.GetArrayElementAtIndex(i);
                var prefabProp = element.FindPropertyRelative("prefab");
                var idProp = element.FindPropertyRelative("id");
                if (prefabProp != null && idProp != null)
                {
                    int id = 0;
                    if (prefabProp.objectReferenceValue != null)
                    {
                        id = prefabProp.objectReferenceValue.GetInstanceID();
                    }
                    idProp.FindPropertyRelative("id").intValue = id;
                    EditorUtility.SetDirty(target);
                }
            }

            serializedObject.ApplyModifiedProperties();
            DrawDefaultInspector();
        }
    }
}