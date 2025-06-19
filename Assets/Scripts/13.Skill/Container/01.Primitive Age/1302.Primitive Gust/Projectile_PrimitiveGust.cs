using ProjectTile;
using Status;
using UnityEngine;
using Util;

namespace Skill
{
    public class Projectile_PrimitiveGust : ProjectileBase
    {
        public override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out StatusBase otherStatus) &&
                !otherStatus.hp.IsMin)
            {
                var skill = ownerObject.GetComponent<Skill_PrimitiveGust>();
                var status = skill.Status as Skill_PrimitiveGust_Status;
                
                // 출혈
                if (status.BleedingChance.IsProbability())
                {
                    otherStatus.GetDebuff(skill, IBleedingData.Tick, status.BleedingDuration);
                }
                
                // 1회 추가 타격
                if (status.doubleAttackChance.IsProbability())
                {
                    otherStatus.Damaged(status.Damage);
                }
            }
            base.OnTriggerEnter(other);
        }
    }
}