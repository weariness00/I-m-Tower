using System;
using System.Collections.Generic;
using Skill;
using UnityEngine;

namespace Tower
{
    public class TowerControl : MonoBehaviour
    {
        [HideInInspector] public List<SkillBase> skillList = new();
        
        public void Awake()
        {
            var skill = Instantiate(SkillPrefabSO.GetSkill(0), transform);
            skillList.Add(skill);
        }
    }
}