using UnityEditor;
using UnityEngine;

namespace Skill.Editor
{
    [CustomEditor(typeof(SkillPrefabSO))]
    public class SkillPrefabSOEditor : UnityEditor.Editor
    {
        private int prevArraySize = 0;
        private bool isElementalEmpty = true;
        
        public void OnEnable()
        {
            var script = target as SkillPrefabSO;
            prevArraySize = script.skillArray.Length;
            
            foreach (var skill in script.skillArray)
            {
                if (skill == null)
                {
                    isElementalEmpty = true;
                    Debug.Log($"{script.name}에 null인 원소가 존재하여 정렬을 할 수 없습니다.");
                    break;
                }
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var script = target as SkillPrefabSO;
            if (script.skillArray.Length != prevArraySize)
            {
                script.Init();
            }
            if (isElementalEmpty)
            {
                isElementalEmpty = false;
                foreach (var skill in script.skillArray)
                {
                    if (skill == null)
                        isElementalEmpty = true;
                }

                if (isElementalEmpty == false)
                {
                    script.Init();
                }
            }
        }
    }
}