using UnityEditor;
using UnityEngine;

namespace Tower.Editor
{
    [CustomEditor(typeof(TowerStatus))]
    public class TowerStatusEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (EditorApplication.isPlaying)
            {
                var script = target as TowerStatus;
                if (GUILayout.Button("Level Up"))
                {
                    script.AddEXP(script.experience.Max - script.experience.Current);
                }
            }
            
            base.OnInspectorGUI();
        }
    }
}