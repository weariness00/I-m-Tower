using System;
using ProjectTile;
using Status;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Util;

namespace Skill
{
    [RequireComponent(typeof(SkillTelekineticSlamStatus))]
    public class SkillTelekineticSlam : SkillBase
    {
        [NonSerialized] public new SkillTelekineticSlamStatus status;

        public ProjectileBase debrisPrefab;

        private ObjectPool<ProjectileBase> projectilePool;
        public ParticleSystem hitEffectPrefab;
        public ObjectPool<ParticleSystem> hitEffectPool;
        public override void Awake()
        {
            base.Awake();

            searchColliders = new Collider[100];
            projectilePool = new(
                () =>
                {
                    var projectile = Instantiate(debrisPrefab, transform);
                    projectile.ownerObject = gameObject;
                    projectile.ownerStatus = status;
                    projectile.pool = projectilePool;

                    return projectile;
                },
                debris => debris.gameObject.SetActive(true),
                debris => debris.gameObject.SetActive(false),
                debris => Destroy(debris.gameObject));
            
            hitEffectPool = new(
                () =>
                {
                    var hitEffect = Instantiate(hitEffectPrefab, transform);
                    var particleUtil = hitEffect.AddComponent<ParticleSystemUtil>();
                    particleUtil.onParticleStopEvent += () => hitEffectPool.Release(hitEffect);
                    
                    return hitEffect;
                },
                hitEffect => hitEffect.gameObject.SetActive(true),
                hitEffect => hitEffect.gameObject.SetActive(false),
                hitEffect => Destroy(hitEffect.gameObject));
        }
        
        public void Update()
        {
            status.AttackTimer.Current += Time.deltaTime;
            if (status.AttackTimer.IsMax &&
                TryInstantiateProjectile(out var projectile) &&
                projectile is TelekineticSlamDebris debris)
            {
                debris.StartCoroutine(debris.ChangedMoveCoroutine());
                status.AttackTimer.SetMin();
            }
        }

        public override void Init()
        {
            base.Init();
            status = base.status as SkillTelekineticSlamStatus;
        }

        public override string Explain()
        {
            var value = base.Explain();
            var nextLevel = status.level + 1;
            if (nextLevel == 10) value += "공격속도 2배 증가";
            else if (nextLevel == 20) value += "대미지 2배 증가";
            return base.Explain();
        }
        
        private bool TryInstantiateProjectile(out ProjectileBase projectile)
        {
            var length = Physics.OverlapSphereNonAlloc(transform.position, status.AttackRange, searchColliders, targetLayer);
            var nearTarget = searchColliders.GetNear(transform.position, length);
            if (nearTarget != null)
            {
                projectile = projectilePool.Get();
                projectile.transform.position = transform.position;
                projectile.targetTransform = nearTarget.transform;
                projectile.targetStatus = nearTarget.GetComponent<StatusBase>();
                return true;
            }
            projectile = null;
            return false;
        }
    }
}

