using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Util.AStar.Editor
{
    [CustomEditor(typeof(AStarGridRenderer))]
    public class AStarGridRendererEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var script = target as AStarGridRenderer;
            if (GUILayout.Button("Grid Mesh 생성"))
            {
                script.CreateMesh();
            }
        }
    }
}