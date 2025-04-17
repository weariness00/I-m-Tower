using System;
using System.Linq;
using Game;
using UnityEngine;

namespace Skill.UI
{
    public class SkillSelectCanvas : MonoBehaviour
    {
        public Canvas canvas;
        
        public int selectCount = 3;
        public SkillSelectUI[] selectUIArray;

        private bool isSelect = false;

        public void Awake()
        {
            canvas.gameObject.SetActive(false);

            foreach (var ui in selectUIArray)
            {
                ui.button.onClick.AddListener(() =>
                {
                    Select(ui);
                });
            }
            GameManager.Instance.tower.status.onLevelUpEvent.AddListener(On);
        }

        public void On()
        {
            Time.timeScale = 0f;
            isSelect = false;
            canvas.gameObject.SetActive(true);
            
            for (int i = 0; i < selectCount; i++)
            {
                var skill = SkillPrefabSO.GetRandomSkill();
                var ui = selectUIArray[i];
                ui.skillID = skill.id;
                ui.icon.sprite = skill.icon;
                ui.explainText.text = $"{skill.skillName}\n" + "간단 설명, 초당 공격횟수, 범위, 공격력";
            }
        }

        // 선택 안하고 건너뛸 경우
        public void NotSelect()
        {
            isSelect = true;
            canvas.gameObject.SetActive(false);
            // 플레이어 레벨 하락해야됨
            // 혹은 다른거 되게끔
        }

        private void Select(SkillSelectUI ui)
        {
            if(isSelect) return;
            isSelect = true;
                    
            canvas.gameObject.SetActive(false);

            var tower = GameManager.Instance.tower;
            var skill = tower.skillList.FirstOrDefault(s => ui.skillID == s.id);
            if (skill == null)
            {
                skill = Instantiate(SkillPrefabSO.GetSkill(ui.skillID), tower.transform);
                tower.skillList.Add(skill);
            }
            skill.LevelUp(1);
            
            Time.timeScale = 1f;
        }
    }
}