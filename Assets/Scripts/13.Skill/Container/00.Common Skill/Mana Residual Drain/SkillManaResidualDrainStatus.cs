using UnityEngine;

namespace Skill
{
    public class SkillManaResidualDrainStatus : SkillStatus
    {
        public float drainManaAmount = 0.5f;

        public override void LevelUp(int upCount)
        {
            base.LevelUp(upCount);
            
            drainManaAmount += 0.15f * upCount;
            if (CheckGoalLevel(10))
            {
                AttackTimer.Max *= 0.5f;
            }
        }
    }
}

