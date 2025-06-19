using System;
using ProjectTile;
using Status;
using Tower;
using UnityEngine;
using UnityEngine.Pool;
using Util;

namespace Skill
{
    public partial class Skill_PrimitiveGust_Status : SkillStatus
    {
        public ProjectileBase projectilePrefab;
        public ObjectPool<ProjectileBase> projectilePool;
        public int projectileCount = 3;
        // 3발이면 -30, 0, 30 도의 방향으로 1발씩 발사
        [Tooltip("발사체들의 퍼져나갈 각도")]public MinMax<float> projectileAngle = new(-30f, 30f);
        
        [SerializeField] private StatModifier criticalChanceModifier = new(StatModifier.ModifierType.Percent, 0.1f);
        [SerializeField] private StatModifier attackSpeedModifier = new(StatModifier.ModifierType.Flat, 1f);
        [SerializeField] public float doubleAttackChance = 0f;

        public void OnValidate()
        {
            BleedingChance = bleedingChance;
        }

        public override void Awake()
        {
            base.Awake();
            projectilePool = new(
                () =>
                {
                    var projectile = Instantiate(projectilePrefab);
                    projectile.ownerObject = ownerObject;
                    projectile.ownerStatus = this;
                    projectile.pool = projectilePool;
                    projectile.Move = new NonTargetMove(projectile);

                    return projectile;
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
                projectile => Destroy(projectile.gameObject));
        }

        public override void LevelUp(int upCount)
        {
            base.LevelUp(upCount);

            damage.Value += 1;
            
            if (CheckGoalLevel(10))
            {
                BleedingChance = 0.2f;
            }
            if (CheckGoalLevel(20))
            {
                attackSpeed.AddModifier(attackSpeedModifier);
            }
            if (CheckGoalLevel(30))
            {
                criticalChance.AddModifier(criticalChanceModifier);
            }
            if (CheckGoalLevel(40))
            {
                projectileCount += 2;
            }

            if (CheckGoalLevel(50))
            {
                doubleAttackChance = 0.1f;
            }
        }
    }

    public partial class Skill_PrimitiveGust_Status : IBleedingData
    {
        public MinMaxValue<int> BleedingStack { get; set; } = new(0, 0, 1);
        public float BleedingChance { get; set; } = 0f;
        public float BleedingDamage => TowerControl.Instance.status.bleedingDamage.Value;
        public float BleedingDuration { get; set; } = 2f;

#if UNITY_EDITOR
        [SerializeField] private float bleedingChance = 0f;
#endif
    }
}