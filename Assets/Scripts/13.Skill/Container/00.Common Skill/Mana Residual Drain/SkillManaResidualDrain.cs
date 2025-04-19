using System;
using Game;
using Tower;
using UnityEngine;

namespace Skill
{
    // 잔류 마나 흡수
    [RequireComponent(typeof(SkillManaResidualDrainStatus))]
    public class SkillManaResidualDrain : SkillBase
    {
        [NonSerialized] public new SkillManaResidualDrainStatus status;

        public override void Awake()
        {
            base.Awake();
            status = base.status as SkillManaResidualDrainStatus;
        }

        public void Update()
        {
            status.AttackTimer.Current += Time.deltaTime;
            if (status.AttackTimer.IsMax)
            {
                status.AttackTimer.SetMin();
                GameManager.Instance.tower.status.AddEXP(status.DrainMana);
            }
        }

        public override string Explain()
        {
            if(status == null) status = base.status as SkillManaResidualDrainStatus;
            var value = base.Explain();
            var nextLevel = status.level + 1;
            if (nextLevel == 10) value += "잔류 마나 흡수 속도 2배";
            
            return base.Explain();
        }
    }
}

