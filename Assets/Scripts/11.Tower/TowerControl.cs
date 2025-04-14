using System;
using Skill;
using UnityEngine;

namespace Tower
{
    public class TowerControl : MonoBehaviour
    {
        public void Awake()
        {
            Instantiate(SkillPrefabSO.GetSkill(0), transform);
        }
    }
}