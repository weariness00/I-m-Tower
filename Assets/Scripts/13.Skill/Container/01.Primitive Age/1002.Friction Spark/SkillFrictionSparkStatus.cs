using Status;
using UnityEngine;


namespace Skill
{
    public class SkillFrictionSparkStatus : SkillStatus
    {
        [SerializeField][Tooltip("화상 대미지")] private float burnDamage = 1;
        [SerializeField][Tooltip("화상 대미지 배율")] private float burnDamageMultiple = 1f;
        [Tooltip("화상 대미지 간격")] public float burnTickInterval = 0.5f;
        [Range(0,1)][Tooltip("화상 확률")] public float burnChange = 0.3f;
        [Tooltip("화상 지속 시간")] public float burnDuration = 1f;

        [SerializeField] private StatModifier damageFlat = new(StatModifier.ModifierType.Flat);
        [SerializeField] private StatModifier damagePercent = new(StatModifier.ModifierType.Percent);
        
        public int BurnDamage => Mathf.CeilToInt(burnDamage * burnDamageMultiple);

        public override void Init()
        {
            base.Init();
            damage.AddModifier(damageFlat);
            damage.AddModifier(damagePercent);
        }

        public override void LevelUp(int upCount)
        {
            base.LevelUp(upCount);
            damageFlat.value += 3f * upCount;
            burnDamage += 0.5f * upCount;

            if (CheckGoalLevel(10))
            {
                // 대미지 2배
                damagePercent.value += 1f;
            }
            else if (CheckGoalLevel(20))
            {
                // 화상 확률 30% 증가
                // 화상 대미지 50% 증가
                burnChange += 0.3f;
                burnDamageMultiple += 0.5f;
            }
            else if (CheckGoalLevel(30))
            {
                // 화상 지속 시간 2배
                // 화상 대미지 50% 증가
                burnDuration *= 2f;
                burnDamageMultiple += 0.5f;
            }
            else if (CheckGoalLevel(40))
            {
                // 화상 대미지 간격 속도 2배
                burnTickInterval *= 0.5f;
            }
            else if (CheckGoalLevel(50))
            {
                // 무조건 화상 발생
                burnChange = 1f;
            }
        }   
    }
}

