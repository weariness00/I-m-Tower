using System;
using System.Collections;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Skill.UI
{
    public partial class SkillInfoCanvas : MonoBehaviour
    {
        public SkillManager skillManager;

        public void Awake()
        {
            CreateAllSkillSummeryUI();
            
            detailUI.rootObject.SetActive(false);
            
            skillManager.onAddNewSkillEvent += AddNewSkill;
            skillManager.onSkillLevelUpEvent += LevelUpSkill;
        }

        private void AddNewSkill(SkillBase skill)
        {
            int index = Array.BinarySearch(summeryUIArray, skill.id);
            var summeryUI = summeryUIArray[index];
            
            DebugManager.ToDo("소지한 스킬인것처럼 보이는 ui");
        }
        
        private void LevelUpSkill(SkillBase skill)
        {
            int index = Array.BinarySearch(summeryUIArray, skill.id);
            var summeryUI = summeryUIArray[index];
            summeryUI.levelText.text = $"LV.{skill.status.level.Current}/{skill.status.level.Max}";
        }
    }

    // 스킬 리스트 관련
    public partial class SkillInfoCanvas
    {
        [Header("Summery 정보")]
        public ScrollRect summeryScrollRect;
        public SkillInfoSummeryUI summeryUIPrefab;
        private SkillInfoSummeryUI[] summeryUIArray;
        
        // 모든 스킬에 대한 UI를 생성
        private void CreateAllSkillSummeryUI()
        {
            summeryUIArray = new SkillInfoSummeryUI[SkillPrefabSO.Instance.skillArray.Length];
            for (var i = 0; i < SkillPrefabSO.Instance.skillArray.Length; i++)
            {
                var skill = SkillPrefabSO.Instance.skillArray[i];
                var summeryUI = Instantiate(summeryUIPrefab, summeryScrollRect.content);
                summeryUI.button.onClick.AddListener(() => OnDetail(skill.id));
                
                summeryUI.skillID = skill.id;
                summeryUI.icon.sprite = skill.icon;
                summeryUI.nameText.text = skill.skillName;
                summeryUI.levelText.text = $"LV.{skill.status.level.Current}/{skill.status.level.Max}";

                summeryUIArray[i] = summeryUI;
            }
        }
    }

    // Detail 정보 관련
    public partial class SkillInfoCanvas
    {
        [Header("Detail 정보")] 
        public DetailUI detailUI;
        
        [Serializable]
        public class DetailUI
        {
            [Tooltip("최상위 부모")]
            public GameObject rootObject;
            [Tooltip("스킬 아이콘")]
            public Image icon;
            [Tooltip("스킬 이름")]
            public TMP_Text nameText;
            [Tooltip("스킬 레벨 텍스트 \"LV.현재/최대\"")]
            public TMP_Text levelText;
            [Tooltip("스킬 효과 설명")]
            public TMP_Text explainText;
            [Tooltip("스킬 스탯 표시")]
            public TMP_Text statText;
            [Tooltip("스킬 다음 레벨 스탯 표시")]
            public TMP_Text nextLevelStatText;
        }

        private void OnDetail(int skillID)
        {
            var skill = skillManager.GetSkill(skillID);
            if (skill == null)
                skill = SkillPrefabSO.GetSkill(skillID);
            
            detailUI.rootObject.SetActive(true);

            detailUI.icon.sprite = skill.icon;
            detailUI.nameText.text = skill.skillName;
            detailUI.levelText.text = $"LV.{skill.status.level.Current}/{skill.status.level.Max}";
            detailUI.explainText.text = skill.Explain();
            detailUI.statText.text = string.Join("\n", $"공격력 {skill.status.damage}", $"공격 속도 {skill.status.attackSpeed}");
        }
    }
}