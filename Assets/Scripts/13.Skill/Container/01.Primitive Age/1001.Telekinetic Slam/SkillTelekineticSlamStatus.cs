using UnityEngine;
using UnityEngine.Serialization;

namespace Skill
{
    public class SkillTelekineticSlamStatus : SkillStatus
    {
        public int upDamageSize = 10;
        [Tooltip("대상 방어 N% 관통")] public float targetDefensePenetrationMultiple = 0;
        [Tooltip("받는 피해 증가율")] public float targetMoreDamageMultiple = 0f;
        [Tooltip("받는 피해 증가 지속시간")] public float targetMoreDamageMultipleDuration = 0f;
        
        [Header("대상 체력이 N% 이하일때 아래의 인스펙터 동작")]
        [Range(0f, 1f)][Tooltip("몇퍼 이하인지")] public float targetHpPercent = 0f;
        [Tooltip("치명타 확률 몇퍼 상승인지")] public float slamCriticalChange = 0f;
        
        public override void LevelUp(int upCount)
        {
            base.LevelUp(upCount);
            value.damage += upDamageSize * upCount;
            if (CheckGoalLevel(10))
            {
                // 대미지 100% 증가
                DamageMultiple += 2f;
            }
            else if (CheckGoalLevel(20))
            {
                // 방어력 관통 20% 증가
                targetDefensePenetrationMultiple += 0.2f;
            }
            else if(CheckGoalLevel(30))
            {
                // 치명타 피해 100% 증가
                value.criticalMultiple += 1f;
            }
            else if(CheckGoalLevel(40))
            {
                // 체력 50% 이하 적에게 치명타 확률 30% 증가
                targetHpPercent += 0.5f;
                slamCriticalChange += 0.3f;
            }
            else if(CheckGoalLevel(50))
            {
                // 대상이 3초간 받는 피해 30% 증가
                targetMoreDamageMultiple += 0.3f;
                targetMoreDamageMultipleDuration += 3f;
            }
        }
    }
}

