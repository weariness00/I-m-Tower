using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Util.AStar.Editor
{
    [CustomEditor(typeof(AStarGrid))]
    public class AStarGridEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if(EditorApplication.isPlaying) return;
            
            var script = target as AStarGrid;
            if (GUILayout.Button("Grid 생성"))
            {
                    script.CalculateGridSize();
                    script.CreateGrid();
                    
                    EditorUtility.SetDirty(script);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
            }
        }
    }
}