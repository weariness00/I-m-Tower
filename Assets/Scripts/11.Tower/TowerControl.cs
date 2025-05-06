using System;
using System.Collections.Generic;
using Skill;
using UnityEngine;
using Util;

namespace Tower
{
    public class TowerControl : Singleton<TowerControl>
    {
        public TowerStatus status;
        [HideInInspector] public List<SkillBase> skillList = new();

        public Action<SkillBase> onAddSkillEvent;

        protected override void Initialize()
        {
            base.Initialize();
            IsDontDestroy = false;
        }

        public void Start()
        {
            status.AddEXP(status.experience.Max);
        }
    }
}