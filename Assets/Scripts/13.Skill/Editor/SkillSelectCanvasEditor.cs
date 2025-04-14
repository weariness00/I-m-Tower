using Skill.UI;
using UnityEditor;
using UnityEngine;

namespace Skill.Editor
{
    [CustomEditor(typeof(SkillSelectCanvas))]
    public class SkillSelectCanvasEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (EditorApplication.isPlaying)
            {
                var script = target as SkillSelectCanvas;
                if (GUILayout.Button("On"))
                {
                    script.On();
                }
            }
            
            base.OnInspectorGUI();
        }
    }
}