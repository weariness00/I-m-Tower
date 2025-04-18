using UnityEngine;

namespace Skill
{
    public class SkillTelekineticSlamStatus : SkillStatus
    {
        public int upDamageSize = 10;

        public override void LevelUp(int upCount)
        {
            var nextLevel = level + upCount;
            var prevLevel = level;
            base.LevelUp(upCount);
            value.damage += upDamageSize * upCount;

            if (CheckGoalLevel(10, prevLevel, nextLevel))
            {
                // 대미지 2배
                DamageMultiple += 2f;
            }
            else if (CheckGoalLevel(20, prevLevel, nextLevel))
            {
                // 공격속도 2배
                AttackTimer.Max *= 0.5f;
            }
        }
    }
}

