using System;
using ProjectTile;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;
using Util;

namespace Skill
{
    // 마찰에 의한 불꽃 공격
    // 전범위 공격
    [RequireComponent(typeof(SkillFrictionSparkStatus))]
    public class SkillFrictionSpark : SkillBase
    {
        [NonSerialized] public new SkillFrictionSparkStatus status;
        public SparkProjectile sparkProjectilePrefab;

        private ObjectPool<ProjectileBase> projectilePool;
        public ObjectPool<ParticleSystem> burnEffectPool;
        public ParticleSystem burnEffectPrefab;

        public override void Awake()
        {
            base.Awake();

            projectilePool = new(
                () =>
                {
                    var spark = Instantiate(sparkProjectilePrefab);
                    spark.ownerObject = this;
                    spark.ownerStatus = status;
                    spark.pool = projectilePool;
                    spark.Move = new NonTargetMove(spark);

                    return spark;
                },
                projectile =>
                {
                    projectile.gameObject.SetActive(true);
                    projectile.transform.position = transform.position;
                    projectile.transform.rotation = transform.rotation;
                    
                    if(projectile is IPoolOnOff poolOnOff)
                        poolOnOff.PoolOn();
                },
                projectile =>
                {
                    projectile.gameObject.SetActive(false);
                    
                    if(projectile is IPoolOnOff poolOnOff)
                        poolOnOff.PoolOff();
                },
                spark => Destroy(spark.gameObject));

            burnEffectPool = new(
                () =>
                {
                    var burnEffect = Instantiate(burnEffectPrefab);
                    return burnEffect;
                },
                burnEffect =>
                {
                    burnEffect.gameObject.SetActive(true);
                    burnEffect.Play();
                },
                burnEffect =>
                {
                    burnEffect.gameObject.SetActive(false);
                    burnEffect.transform.SetParent(transform);
                    burnEffect.Stop();
                },
                burnEffect => Destroy(burnEffect.gameObject));
        }

        public void Update()
        {
            status.AttackTimer.Current += Time.deltaTime;
            if (status.AttackTimer.IsMax)
            {
                status.AttackTimer.SetMin();
                projectilePool.Get();
            }
        }

        public override void Init()
        {
            base.Init();
            status = base.status as SkillFrictionSparkStatus;
        }
    }
}

