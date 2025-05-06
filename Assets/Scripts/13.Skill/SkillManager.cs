using System;
using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;

namespace Skill
{
    public class SkillManager : MonoBehaviour
    {
        public Dictionary<int, SkillBase> skillDictionary = new();
        public int SkillCount => skillDictionary.Count;
        
        public event Action<SkillBase> onAddNewSkillEvent;
        public event Action<SkillBase> onSkillLevelUpEvent;

        public void AddSkill(SkillBase skill)
        {
            // 리스트에 없는경우
            if (!skillDictionary.ContainsKey(skill.id))
            {
                skill = Instantiate(skill, transform);
                skillDictionary.Add(skill.id, skill);
                onAddNewSkillEvent?.Invoke(skill);
                
                DebugManager.Log($"{name}이 {skill.skillName}을 습득");
            }
            else
                skill = skillDictionary[skill.id];
            skill.status.LevelUp(1);
            onSkillLevelUpEvent?.Invoke(skill);
            DebugManager.Log($"{name}의 {skill.skillName}의 레벨 업 [현재 레벨 : {skill.status.level}]");
        }

        public void AddRandomSkill() => AddSkill(SkillPrefabSO.GetRandomSkill());
    }
}