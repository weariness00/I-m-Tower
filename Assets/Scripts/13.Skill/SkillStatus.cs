using Status;
using Manager;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Util;

namespace Skill
{
    public abstract partial class SkillStatus : StatusBase
    {
        public MinMaxValue<int> level = new(0,0,50);
        public SkillType skillType;
        public SkillAttributeType attributeType;

        public UnityEvent<int> onLevelUpEvent = new();

        protected int prevLevel = 0;
        protected int nextLevel = 0;
        
        public virtual void LevelUp(int upCount)
        {
            nextLevel = level + upCount;
            prevLevel = level;
            level.Current += upCount;
            onLevelUpEvent.Invoke(upCount);
        }

        protected bool CheckGoalLevel(int goalLevel)
        {
            bool isGoal = prevLevel < goalLevel && goalLevel <= nextLevel;
            if(isGoal) DebugManager.Log($"{ownerObject.name}이 {goalLevel}레벨에 도달했습니다.");
            return isGoal;
        }
    }
}