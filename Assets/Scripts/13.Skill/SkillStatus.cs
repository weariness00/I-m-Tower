using Game.Status;
using UnityEngine;
using UnityEngine.Events;

namespace Skill
{
    public class SkillStatus : StatusBase
    {
        public int level = 0;
        public SkillType skillType;

        public UnityEvent<int> onLevelUpEvent = new();

        public void LevelUp(int upCount)
        {
            level += upCount;
            onLevelUpEvent.Invoke(upCount);
        }
    }
}