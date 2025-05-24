using System;
using Game;
using Tower;
using UnityEngine;
using Util;

namespace Skill
{
    // 잔류 마나 흡수
    [RequireComponent(typeof(SkillResidualManaDrainStatus))]
    public class SkillResidualManaDrain : SkillBase
    {
        [NonSerialized] public new SkillResidualManaDrainStatus status;
        
        public void Update()
        {
            status.attackTimer.Current += Time.deltaTime;
            if (status.attackTimer.IsMax)
            {
                status.attackTimer.SetMin();
                TowerControl.Instance.status.AddEXP(status.DrainMana);

                if (status.doubleProcChance.IsProbability())
                {
                    TowerControl.Instance.status.AddEXP(status.DrainMana);
                }
            }
        }

        public override void Init()
        {
            base.Init();
            status = base.status as SkillResidualManaDrainStatus;
        }

        public override string Explain()
        {
            var value = base.Explain();
            var nextLevel = status.level + 1;
            if (nextLevel == 10) value += "잔류 마나 흡수 속도 2배";
            
            return base.Explain();
        }
    }
}

