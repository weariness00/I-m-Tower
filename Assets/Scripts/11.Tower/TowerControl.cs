using System;
using System.Collections.Generic;
using Skill;
using UnityEngine;

namespace Tower
{
    public class TowerControl : MonoBehaviour
    {
        public TowerStatus status;
        [HideInInspector] public List<SkillBase> skillList = new();
    }
}