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
        public SkillManager skillManager;

        protected override void Initialize()
        {
            base.Initialize();
            IsDontDestroy = false;
        }

        public override void Awake()
        {
            base.Awake();
            status.onLevelUpEvent.AddListener(() => skillManager.AddRandomSkill());
        }

        public void Start()
        {
            status.AddEXP(status.experience.Max);
        }
    }
}