using System;
using System.Collections.Generic;
using System.Linq;
using Manager;
using NUnit.Framework;
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
            if(skill.Status.level.IsMax) skillManagerData.levelMaxSkillList.Add(skill);
            skillManagerData.OnLevelUpSkill(skill);
            DebugManager.Log($"{name}의 {skill.skillName}의 레벨 업 [현재 레벨 : {skill.Status.level}]");
        }

        public void AddRandomSkill()
        {
            var hasSkillArray = skillManagerData.hasSkillDictionary.Values.ToArray();
            var levelMaxSkillArray = skillManagerData.levelMaxSkillList.ToArray();
            while (true)
            {
                var skill = skillManagerData.hasSkillCount <= 
                            skillManagerData.hasSkillDictionary.Count ? hasSkillArray.Except(levelMaxSkillArray).ToArray().Random() : SkillPrefabSO.GetRandomSKillExcept(levelMaxSkillArray);
                if (skill.Status.level.IsMax)
                    continue;

                AddSkill(skill);
                break;
            }
        }
    }
}