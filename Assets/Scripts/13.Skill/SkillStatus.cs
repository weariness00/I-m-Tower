using System;
using System.Collections.Generic;
using Game.Status;
using UnityEngine;
using UnityEngine.Events;

namespace Skill
{
    public abstract partial class SkillStatus : StatusBase
    {
        public int level = 0;
        public SkillType skillType;

        public UnityEvent<int> onLevelUpEvent = new();
        
        public virtual void LevelUp(int upCount)
        {
            level += upCount;
            onLevelUpEvent.Invoke(upCount);
        }

        protected bool CheckGoalLevel(int goalLevel, int prevLevel, int nextLevel)
        {
            return prevLevel < goalLevel && goalLevel <= nextLevel;
        }
    }
}