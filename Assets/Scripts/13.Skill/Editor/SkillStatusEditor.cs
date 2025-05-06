using UnityEditor;
using UnityEngine;

namespace Skill.Editor
{
    [CustomEditor(typeof(SkillStatus), true)]
    public class SkillStatusEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (EditorApplication.isPlaying)
            {
                var script = target as SkillStatus;
                if (GUILayout.Button("Level Up"))
                {
                    script.LevelUp(1);
                }
            }
            
            base.OnInspectorGUI();
        }
    }
}