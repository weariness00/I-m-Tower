using UnityEditor;
using UnityEngine;

namespace Skill.Editor
{
    [CustomEditor(typeof(SkillBase), true)]
    public class SkillEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (EditorApplication.isPlaying)
            {
                var script = target as SkillBase;
                if (GUILayout.Button("Level Up"))
                {
                    script.status.LevelUp(1);
                }
            }
            
            base.OnInspectorGUI();
        }
    }
}