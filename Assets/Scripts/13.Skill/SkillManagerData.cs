using System;
using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace Skill
{
    public class SkillManagerData : MonoBehaviour
    {
        public Dictionary<int, SkillBase> hasSkillDictionary = new(); // 소지한 스킬
        public int HasSkillCount => hasSkillDictionary.Count;
        public Dictionary<int, SkillBase> useSkillDictionary = new(); // 실제 사용하고 있는 스킬들 (key : 스킬 소켓, value : 스킬)

        [NonSerialized] public List<SkillBase> levelMaxSkillList = new();

        public int useSkillCount = 5; // 사용 가능한 스킬 갯수
        public int hasSkillCount = 8; // 소지 가능한 스킬 갯수

        public event Action<SkillBase> onAddNewSkillEvent; // 스킬 추가 이벤트 
        public event Action<SkillBase> onLevelUpSkillEvent; // 스킬 추가 이벤트 
        
        public SkillBase GetSkill(int id)
        {
            if (!hasSkillDictionary.TryGetValue(id, out var skill))
            {
                DebugManager.LogWarning($"소지한 스킬이 아닙니다. [id : {id}]");
            }

            return skill;
        }
        
        public void OnAddNewSkill(SkillBase skill) => onAddNewSkillEvent?.Invoke(skill);
        public void OnLevelUpSkill(SkillBase skill) => onLevelUpSkillEvent?.Invoke(skill);
    }
}