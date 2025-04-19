using System;
using UnityEngine;

namespace Skill
{
    public class SkillManaResidualDrainStatus : SkillStatus
    {
        [SerializeField] private float drainManaAmount = 0.5f;
        [SerializeField] private float drainManaMultiple = 1f;

        private float originAttackTimeMaxValue;
        public float DrainMana => drainManaMultiple * drainManaAmount;

        public void Awake()
        {
            originAttackTimeMaxValue = AttackTimer.Max;
        }

        public override void LevelUp(int upCount)
        {
            base.LevelUp(upCount);
            
            drainManaAmount += 0.15f * upCount;
            if (CheckGoalLevel(10))
            {
                // 흡수 속도 2배
                AttackTimer.Max -= originAttackTimeMaxValue * 0.5f;
            }
            else if (CheckGoalLevel(20))
            {
                // 흡수량 100% 증가
                drainManaMultiple += 1;
            }
            else if (CheckGoalLevel(30))
            {
                // 흡수량 5증가
                drainManaAmount += 5f;
            }
            else if (CheckGoalLevel(40))
            {
                // 흡수량 5증가
                drainManaAmount += 5f;
            }
            else if (CheckGoalLevel(50))
            {
                // 흡수 속도 1.5배 증가
                // 흡수량 50% 증가
                AttackTimer.Max -= originAttackTimeMaxValue * 0.25f;
                drainManaMultiple += 0.5f;
            }
        }
    }
}

