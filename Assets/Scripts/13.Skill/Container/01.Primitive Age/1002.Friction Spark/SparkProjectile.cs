using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Status;
using ProjectTile;
using UnityEngine;
using Util;

namespace Skill
{
    
    public class SparkProjectile : ProjectileBase
    {
        [NonSerialized] public new SkillFrictionSpark ownerObject;
        [NonSerialized] public new SkillFrictionSparkStatus ownerStatus;

        public ParticleSystem fireParticle;
        
        public override void Start()
        {
            base.Start();
            base.ownerObject = ownerObject.gameObject;
            base.ownerStatus = ownerStatus;
        }

        public override void Update()
        {
            base.Update();
            if (Vector3.Distance(ownerObject.transform.position, transform.position) > 100)
            {
                pool.Release(this);
            }
        }

        public override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.TryGetComponent(out StatusBase otherStatus) &&
                !otherStatus.Hp.IsMin)
            {
                if (ownerStatus.burnChange.IsProbability())
                {
                    BurnDamageAsync(otherStatus, otherStatus.dieCancelToken.Token).Forget();
                }
            }
        }

        public override void PoolOn()
        {
            base.PoolOn();
            fireParticle.Play();
        }

        public override void PoolOff()
        {
            base.PoolOff();
            fireParticle.Clear(true);
        }

        public async UniTask BurnDamageAsync(StatusBase _targetStatus, CancellationToken token)
        {
            var burnEffect = ownerObject.burnEffectPool.Get();
            burnEffect.transform.SetParent(_targetStatus.transform);
            burnEffect.transform.localPosition = Vector3.zero;

            try
            {
                int burnCount = Mathf.FloorToInt(ownerStatus.burnDuration / ownerStatus.burnTickInterval);
                for (int i = 0; i < burnCount; i++)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(ownerStatus.burnTickInterval), cancellationToken: token);
                    _targetStatus.Damaged(ownerStatus.BurnDamage);
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                ownerObject.burnEffectPool.Release(burnEffect);
            }
        }
    }
}

