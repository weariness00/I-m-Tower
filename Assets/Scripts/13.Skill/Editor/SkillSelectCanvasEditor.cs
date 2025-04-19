using Skill.UI;
using UnityEditor;
using UnityEngine;

namespace Skill.Editor
{
    [CustomEditor(typeof(SkillSelectCanvas))]
    public class SkillSelectCanvasEditor : UnityEditor.Editor
    {
        public static int skillID;
        public override void OnInspectorGUI()
        {
            if (EditorApplication.isPlaying)
            {
                var script = target as SkillSelectCanvas;
                if (GUILayout.Button("On"))
                {
                    script.On();
                }

                EditorGUILayout.Space();
                skillID = EditorGUILayout.IntField(skillID);
                if (GUILayout.Button("Set Skill ID On"))
                {
                    script.On();
                    var skill = SkillPrefabSO.GetSkill(skillID);
                    script.selectUIArray[0].SetSKill(skill);
                }
            }
            
            base.OnInspectorGUI();
        }
    }
}