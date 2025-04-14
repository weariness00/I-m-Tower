using System;
using UnityEngine;

namespace Skill.UI
{
    public class SkillSelectCanvas : MonoBehaviour
    {
        public int selectCount = 3;
        public SkillSelectUI[] selectUIArray;

        private bool isSelect = false;

        public void Awake()
        {
            gameObject.SetActive(false);

            foreach (var ui in selectUIArray)
            {
                ui.button.onClick.AddListener(() =>
                {
                    if(isSelect) return;
                    isSelect = true;
                    
                    gameObject.SetActive(false);
                });
            }
        }

        public void On()
        {
            isSelect = false;
            gameObject.SetActive(true);
            
            for (int i = 0; i < selectCount; i++)
            {
                var skill = SkillPrefabSO.GetRandomSkill();
                var ui = selectUIArray[i];
                ui.skill = skill;
                ui.icon.sprite = skill.icon;
                ui.explainText.text = $"{skill.skillName}\n" + "간단 설명, 초당 공격횟수, 범위, 공격력";
            }
        }
    }
}