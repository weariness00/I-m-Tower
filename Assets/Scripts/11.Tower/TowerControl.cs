using System;
using System.Collections.Generic;
using Skill;
using Status;
using UnityEditor;
using UnityEngine;
using Util;

namespace Tower
{
    [RequireComponent(typeof(TowerPointData))]
    public class TowerControl : Singleton<TowerControl>
    {
        public TowerStatus status;

        [Header("Point 관련")]
        public TowerPointView pointView;
        public TowerPointData pointData;
        
        [Header("Skill 관련")]
        public SkillManager skillManager;

        public void Reset()
        {
            status = GetComponent<TowerStatus>();
            pointData = GetComponent<TowerPointData>();
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public override void Awake()
        {
            base.Awake();
            // status.onLevelUpEvent.AddListener (skillManager.AddRandomSkill);
            status.onLevelUpEvent.AddListener(() => pointData.AddPoint(TowerPointData.PointType.Tech, 1));
        }

        public void Start()
        {
            status.AddEXP(status.experience.Max);

            InitPointSystem();
        }
        
        protected override void Initialize()
        {
            base.Initialize();
            IsDontDestroy = false;
        }

        private void InitPointSystem()
        {
            pointView.SetPointData(pointData);
            
            // 스킬 레벨업
            pointView.techPointLevelUpButton.onClick.AddListener(OnTechPointSkillLevelUpButton);
            pointView.bioPointLevelUpButton.onClick.AddListener(OnBioPointSkillLevelUpButton);
            
            // 스킬 뽑기
            pointView.techUI.skillDrawButton.onClick.AddListener(TechDrawSkillButtonClick);
            pointView.bioUI.skillDrawButton.onClick.AddListener(BioDrawSkillButtonClick);
        }

        private void TechDrawSkillButtonClick() => PointDrawSkillButtonClick(TowerPointData.PointType.Tech);
        private void BioDrawSkillButtonClick() => PointDrawSkillButtonClick(TowerPointData.PointType.Bio);
        private void PointDrawSkillButtonClick(TowerPointData.PointType type)
        {
            if (!pointData.UsePoint(type, 1)) return;
            
            switch (type)
            {
                case TowerPointData.PointType.Tech:
                        skillManager.AddRandomSkill();
                    break;
                case TowerPointData.PointType.Bio:
                    if ((0.1f).IsProbability())
                    {
                        skillManager.AddRandomSkill();
                    }
                    break;
            }
        }


        private void OnTechPointSkillLevelUpButton() => OnPointSkillLevelUpButton(TowerPointData.PointType.Tech, skillManager.skillInfoView.detailUI.skillID);
        private void OnBioPointSkillLevelUpButton() => OnPointSkillLevelUpButton(TowerPointData.PointType.Bio, skillManager.skillInfoView.detailUI.skillID);
        private void OnPointSkillLevelUpButton(TowerPointData.PointType type, int skillID)
        {
            var skill = skillManager.skillManagerData.GetSkill(skillID);
            if (skill == null || skill.Status.level.IsMax) return;
            if (!pointData.UsePoint(type, 1)) return;
            switch (type)
            {
                case TowerPointData.PointType.Tech:
                    skillManager.AddSkill(skill);
                    break;
                case TowerPointData.PointType.Bio:
                    if ((0.1f).IsProbability())
                    {
                        skillManager.AddSkill(skill);
                    }
                    break;
            }
        }
    }
}