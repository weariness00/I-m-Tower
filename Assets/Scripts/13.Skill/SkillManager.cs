using System;
using System.Collections.Generic;
using System.Linq;
using Manager;
using Skill.UI;
using UnityEngine;
using Util;

namespace Skill
{
    public class SkillManager : MonoBehaviour
    {
        public SkillManagerData skillManagerData;
        public SkillInfoView skillInfoView;
        public SkillLogView skillLogView;

        public void Awake()
        {
            skillInfoView.SetSkillManager(this);
            skillLogView.SetSkillManager(this);
        }

        public void AddSkill(SkillBase skill)
        {
            // 리스트에 없는경우
            if (!skillManagerData.hasSkillDictionary.ContainsKey(skill.id))
            {
                skill = Instantiate(skill, transform);
                skillManagerData.hasSkillDictionary.Add(skill.id, skill);
                skillManagerData.OnAddNewSkill(skill);

                DebugManager.Log($"{name}이 {skill.skillName}을 습득");

                // 소켓에 자리가 비어 있을 경우
                if (skillManagerData.useSkillCount > skillManagerData.useSkillDictionary.Count && !skillManagerData.useSkillDictionary.ContainsKey(skill.id))
                {
                    for (int i = 0; i < skillManagerData.useSkillCount; i++)
                    {
                        if (skillManagerData.useSkillDictionary.TryAdd(i, skill)) break;
                    }
                }
                else
                {
                    skill.gameObject.SetActive(false);
                }
            }
            else
                skill = skillManagerData.hasSkillDictionary[skill.id];

            skill.Status.LevelUp(1);
            skillManagerData.OnLevelUpSkill(skill);
            DebugManager.Log($"{name}의 {skill.skillName}의 레벨 업 [현재 레벨 : {skill.Status.level}]");
        }

        public void AddRandomSkill()
        {
            AddSkill(skillManagerData.hasSkillCount <= skillManagerData.hasSkillDictionary.Count ? skillManagerData.hasSkillDictionary.Values.ToArray().Random() : SkillPrefabSO.GetRandomSkill());
        }
    }
}