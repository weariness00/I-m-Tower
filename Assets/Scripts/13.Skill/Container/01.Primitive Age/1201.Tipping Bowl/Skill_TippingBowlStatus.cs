using System;
using System.Diagnostics.CodeAnalysis;
using Status;
using UnityEngine;
using Util;

namespace Skill
{
    public class Skill_TippingBowlStatus : SkillStatus
    {
        // 물그릇이 기울어져 물을 쏟는 오브젝트
        [NotNull] public GameObject tippingBowlObject;
        
        // 현재 활성화 상태인지
        public bool isActive; 
        
        // 2번의 타격을 입히는지
        [NonSerialized] public bool isDoubleHit = false;
        
        // 스킬 지속 시간
        public MinMaxValue<float> duration = new(0, 0, 2f);
        // 0.5초당 1번씩 대미지 1틱이 0.5
        public MinMaxValue<float> tickTimer = new(0, 0, 0.5f);
        
        [SerializeField] private StatModifier damagePercent = new(StatModifier.ModifierType.Percent);
        public StatModifier monsterSlowPercent = new(StatModifier.ModifierType.Percent, false);

        public override void Awake()
        {
            base.Awake();
            tippingBowlObject.SetActive(false);
        }

        public override void LevelUp(int upCount)
        {
            base.LevelUp(upCount);

            damage.Value = level.Current;

            // 대미지 10% 추가
            if (CheckGoalLevel(10))
            {
                damagePercent.value = 0.1f;
                damage.AddModifier(damagePercent);
            }
            // 물그릇으로 기울어진 물의 범위 3배 증가
            if (CheckGoalLevel(20))
            {
                tippingBowlObject.transform.localScale *= 3f;
            }
            // 폭포 내부의 적 이동속도 30% 감소
            if (CheckGoalLevel(30))
            {
                monsterSlowPercent.value = -0.3f;
                monsterSlowPercent.isActive = true;
            }
            // 공격이 2번의 타격을 입힌다.
            if (CheckGoalLevel(40))
            {
                isDoubleHit = true;
            }
            // 쿨타임 없이 계속 유지
            if (CheckGoalLevel(50))
            {
                attackTimer.Max = 0;
                tippingBowlObject.SetActive(true);
            }
        }
    }
}