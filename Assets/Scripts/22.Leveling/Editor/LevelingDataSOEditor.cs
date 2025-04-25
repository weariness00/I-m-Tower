using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Leveling.Editor
{
    [CustomEditor(typeof(LevelingDataSO))]
    public class LevelingDataSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var script = (LevelingDataSO)target;
            
            if (GUILayout.Button("Json 불러오기"))
            {
                script.JsonLoad();
            }
            
            base.OnInspectorGUI();
        }
    }
}