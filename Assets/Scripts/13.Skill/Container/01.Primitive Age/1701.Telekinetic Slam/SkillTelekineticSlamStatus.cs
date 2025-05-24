using Status;
using UnityEngine;
using UnityEngine.Serialization;
using Util;

namespace Skill
{
    public class SkillTelekineticSlamStatus : SkillStatus
    {
        public int upDamageSize = 10;
        [Tooltip("대상 방어 N% 관통")] public float targetDefensePenetrationMultiple = 0;
        [Tooltip("받는 피해 증가율")] public StatModifier targetDamagedModifier = new(StatModifier.ModifierType.Percent);
        [Tooltip("받는 피해 증가 지속시간")] public float targetMoreDamageMultipleDuration = 0f;
        
        [Header("대상 체력이 N% 이하일때 아래의 인스펙터 동작")]
        [Range(0f, 1f)][Tooltip("몇퍼 이하인지")] public float targetHpPercent = 0f;
        [SerializeField] private StatModifier damageModifier = new(StatModifier.ModifierType.Percent);
        [SerializeField] private StatModifier criticalDamageModifier = new(StatModifier.ModifierType.Percent);
        [Tooltip("치명타 확률 몇퍼 상승인지")] public StatModifier slamCriticalChangeModifier = new(StatModifier.ModifierType.Flat);
        
        public override void Init()
        {
            base.Init();
            damage.AddModifier(damageModifier);
            criticalDamage.AddModifier(criticalDamageModifier);
        }

        public override void LevelUp(int upCount)
        {
            base.LevelUp(upCount);
            damage.Value += upDamageSize * upCount;
            if (CheckGoalLevel(10))
            {
                // 대미지 100% 증가
                damageModifier.value += 1f;
            }
            else if (CheckGoalLevel(20))
            {
                // 방어력 관통 20% 증가
                targetDefensePenetrationMultiple += 0.2f;
            }
            else if(CheckGoalLevel(30))
            {
                // 치명타 피해 100% 증가
                criticalDamageModifier.value += 1f;
            }
            else if(CheckGoalLevel(40))
            {
                // 체력 50% 이하 적에게 치명타 확률 30% 증가
                targetHpPercent += 0.5f;
                slamCriticalChangeModifier.value += 0.3f;
            }
            else if(CheckGoalLevel(50))
            {
                // 대상이 3초간 받는 피해 30% 증가
                targetDamagedModifier.value += 0.3f;
                targetMoreDamageMultipleDuration += 3f;
            }
        }
    }
}

