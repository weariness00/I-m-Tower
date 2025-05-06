using Game;
using Manager;
using Tower;
using UnityEngine;
using Util;

namespace Skill
{
    public class SkillResidualManaDrainStatus : SkillStatus
    {
        [SerializeField] private float drainManaAmount = 0.5f;
        [SerializeField] private float drainManaMultiple = 1f;

        [SerializeField] [Tooltip("스킬 흡수 속도 증가 비율")] private MinMaxValue<float> drainManaSpeed = new(0, 0, 0.3f);
        public bool isDensitySense = false; // 30레벨 효과인 밀도 감지가 켜져있는지
        public float doubleProcChance = 0f;
        [Tooltip("target을 Count만큼 죽이면 마나흡수 발동")]public int killTargetCount = -1;

        private float originAttackTimeMaxValue;
        public float DrainMana => drainManaMultiple * drainManaAmount;

        public void Awake()
        {
            originAttackTimeMaxValue = AttackTimer.Max;
        }

        public void Start()
        {
            if (GameManager.HasInstance)
            {
                TowerControl.Instance.onAddSkillEvent += OnAddSkill;
            }
        }

        public override void LevelUp(int upCount)
        {
            base.LevelUp(upCount);
            
            drainManaAmount += 0.15f * upCount;
            
            if (CheckGoalLevel(10))
            {
                AttackTimer.Max += originAttackTimeMaxValue * 0.75f;
                originAttackTimeMaxValue *= 1.75f;
            }
            else if (CheckGoalLevel(20))
            {
                // "스킬 1개당 0.1% 흡수 시간 감소"
                // 최대 30%까지만 감소
                if (GameManager.HasInstance)
                {
                    drainManaSpeed.Current = TowerControl.Instance.skillList.Count * 0.1f * upCount;
                }
                // 예외 처리용 or 테스트 용
                else
                {
                    drainManaSpeed.SetMax();
                }
            }
            else if (CheckGoalLevel(30))
            {
                // 밀도 감지
                isDensitySense = true;
                DebugManager.ToDo("시대 마다 밀도 다르게 해주기, 이벤트 지역, 보스 지역 나오면 밀도 설정해주기");
            }
            else if (CheckGoalLevel(40))
            {
                // 흡수 할때 한번 더 흡수 발동할 확률 25% 증가
                doubleProcChance += 0.25f;
            }
            else if (CheckGoalLevel(50))
            {
                // target을 300마리 죽이면 마나 흡수 발동
                killTargetCount = 300;
            }
        }

        private void OnAddSkill(SkillBase skill)
        {
            if (level >= 20)
            {
                drainManaSpeed.Current += 0.1f;
            }
        }
    }
}

