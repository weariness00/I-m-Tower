using System;
using ProjectTile;
using Unit.Monster;
using UnityEngine;
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

        public override void Awake()
        {
            base.Awake();
            status = base.status as SkillFrictionSparkStatus;

            projectilePool = new(
                () =>
                {
                    var spark = Instantiate(sparkProjectilePrefab);
                    spark.pool = projectilePool;
                    spark.ownerObject = gameObject;
                    spark.ownerStatus = status;
                    spark.Move = new NonTargetMove(spark)
                    {
                        direction = Vector3.forward
                    };

                    return spark;
                }, 
                spark => spark.gameObject.SetActive(true),
                spark => spark.gameObject.SetActive(false),
                spark => Destroy(spark.gameObject));
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
    }
}

