using System;
using System.Collections;
using Game.Status;
using ProjectTile;
using UnityEngine;
using Util;

namespace Skill
{
    public class SparkProjectile : ProjectileBase
    {
        [NonSerialized] public new SkillFrictionSparkStatus ownerStatus;
        
        public override void Start()
        {
            base.Start();
            base.ownerStatus = ownerStatus;
        }

        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.TryGetComponent(out StatusBase otherStatus) &&
                !otherStatus.Hp.IsMin)
            {
                if(ownerStatus.burnChange.IsProbability())
                    otherStatus.StartCoroutine(BurnDamage(otherStatus));
            }
        }

        public IEnumerator BurnDamage(StatusBase _targetStatus)
        {
            int burnCount = Mathf.FloorToInt(ownerStatus.burnDuration / ownerStatus.burnTickInterval);
            for (int i = 0; i < burnCount; i++)
            {
                yield return new WaitForSeconds(ownerStatus.burnTickInterval);
                _targetStatus.Damaged(ownerStatus.BurnDamage);
            }
        }
    }
}

