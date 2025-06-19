using System;
using System.Security.Cryptography.X509Certificates;
using Status;
using UnityEngine;
using Util;

namespace Skill
{
    [RequireComponent(typeof(Skill_PrimitiveGust_Status))]
    public partial class Skill_PrimitiveGust : SkillBase
    {
        [SerializeField] private Skill_PrimitiveGust_Status status;
        
        public override SkillStatus Status => status;

        public void Update()
        {
            status.attackTimer.Current += Time.deltaTime;
            if (status.attackTimer.IsMax)
            {
                status.attackTimer.SetMin();
                Execute();
            }
        }

        // 발사체 각도 min, max 값에 따라 퍼지게 발사
        private void Execute()
        {
            var angle = status.projectileAngle.Min;
            for (int i = 0; i < status.projectileCount; i++)
            {
                var projectile = status.projectilePool.Get();
                projectile.transform.rotation = Quaternion.Euler(0, angle, 0);
                angle += status.projectileAngle.Magnitude() / (status.projectileCount - 1);
            }
        }
    }
    
    public partial class Skill_PrimitiveGust : IBleedingController
    {
        public IDebuffData Data => status;
        public IBleedingData BleedingData => status;

        public void Enter(StatusBase targetStatus)
        {
            BleedingData.BleedingStack.Current += 1;
        }

        public void Execute(StatusBase targetStatus)
        {
            targetStatus.Damaged(status.BleedingDamage);
        }

        public void End(StatusBase targetStatus)
        {
            BleedingData.BleedingStack.Current -= 1;
        }

    }
}