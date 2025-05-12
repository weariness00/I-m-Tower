using Status;
using UnityEngine;
using UnityEngine.Serialization;

namespace Skill
{
    public class SkillDustGaleStatus : SkillStatus
    {
        [Header("먼지 바람 Status")]
        [Tooltip("먼지 범위")] public float dustRadius = 5;
        [Tooltip("먼지 지속시간")] public float dustDuration = 3f;
        [Tooltip("이동속도 감소량")] public StatModifier targetSpeedModifier = new (StatModifier.ModifierType.Percent, -0.2f);
        [Tooltip("받는 피해 증가량")] public StatModifier targetDamagedModifier = new(StatModifier.ModifierType.Percent);
        [Tooltip("스턴 지속 시간")] public float stunDuration = 1f;
        [Tooltip("스턴 발동 확률")] public float stunProbability = 0;

        public override void LevelUp(int upCount)
        {
            base.LevelUp(upCount);

            if (CheckGoalLevel(10))
            {
                // 범위 2배 증가
                dustRadius *= 2f;
            }
            else if (CheckGoalLevel(20))
            {
                // 20% 확률로 스턴
                stunProbability = 0.2f;
            }
            else if (CheckGoalLevel(30))
            {
                // 받는 대미지 15%증가
                targetDamagedModifier.value = 0.15f;
            }
            else if (CheckGoalLevel(40))
            {
                // 이동 속도 감소량 15% 증가
                targetSpeedModifier.value -= 0.15f;
            }
            else if (CheckGoalLevel(50))
            {
                // 스턴 확률 80%로 변경
                stunProbability = 0.8f;
            }
        }
    }
}