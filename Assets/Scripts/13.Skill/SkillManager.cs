using System;
using System.Collections.Generic;
using System.Linq;
using Manager;
using UnityEngine;
using Util;

namespace Skill
{
    public class SkillManager : MonoBehaviour
    {
        public Dictionary<int, SkillBase> hasSkillDictionary = new(); // 소지한 스킬
        public int HasSkillCount => hasSkillDictionary.Count;
        public Dictionary<int, SkillBase> useSkillDictionary = new(); // 실제 사용하고 있는 스킬들 (key : 스킬 소켓, value : 스킬)

        [SerializeField] private int useSkillCount = 5; // 사용 가능한 스킬 갯수
        [SerializeField] private int hasSkillCount = 8; // 소지 가능한 스킬 갯수

        public event Action<SkillBase> onAddNewSkillEvent;
        public event Action<SkillBase> onSkillLevelUpEvent;

        public void AddSkill(SkillBase skill)
        {
            // 리스트에 없는경우
            if (!hasSkillDictionary.ContainsKey(skill.id))
            {
                skill = Instantiate(skill, transform);
                hasSkillDictionary.Add(skill.id, skill);
                onAddNewSkillEvent?.Invoke(skill);

                DebugManager.Log($"{name}이 {skill.skillName}을 습득");

                // 소켓에 자리가 비어 있을 경우
                if (useSkillCount > useSkillDictionary.Count && !useSkillDictionary.ContainsKey(skill.id))
                {
                    for (int i = 0; i < useSkillCount; i++)
                    {
                        if (useSkillDictionary.TryAdd(i, skill)) break;
                    }
                }
                else
                {
                    skill.gameObject.SetActive(false);
                }
            }
            else
                skill = hasSkillDictionary[skill.id];

            skill.status.LevelUp(1);
            onSkillLevelUpEvent?.Invoke(skill);
            DebugManager.Log($"{name}의 {skill.skillName}의 레벨 업 [현재 레벨 : {skill.status.level}]");
        }

        public void AddRandomSkill()
        {
            AddSkill(hasSkillCount <= hasSkillDictionary.Count ? hasSkillDictionary.Values.ToArray().Random() : SkillPrefabSO.GetRandomSkill());
        }

        public SkillBase GetSkill(int id)
        {
            if (!hasSkillDictionary.TryGetValue(id, out var skill))
            {
                DebugManager.LogWarning($"소지한 스킬이 아닙니다. [id : {id}]");
            }

            return skill;
        }
    }
}