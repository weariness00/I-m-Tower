using System;
using ProjectTile;
using Status;
using UnityEngine;
using UnityEngine.Pool;
using Util;

namespace Skill
{
    [RequireComponent(typeof(SkillManaBulletStatus))]
    public class SkillManaBullet : SkillBase
    {
        [NonSerialized] public new SkillManaBulletStatus status;
        
        public ProjectileBase bulletPrefab;
        private ObjectPool<ProjectileBase> projectilePool;

        public override void Awake()
        {
            base.Awake();
            projectilePool = new(
                () =>
                {
                    var bullet = Instantiate(bulletPrefab);
                    bullet.ownerObject = gameObject;
                    bullet.ownerStatus = status;
                    bullet.collider.includeLayers = LayerMask.GetMask("Monster");
                    bullet.pool = projectilePool;

                    return bullet;
                },
                arrow => arrow.gameObject.SetActive(true),
                arrow => arrow.gameObject.SetActive(false),
                arrow => Destroy(arrow.gameObject));
        }

        public void Update()
        {
            status.AttackTimer.Current += Time.deltaTime;
            if(status.AttackTimer.IsMax && TryInstantiateProjectile(out var bullet)) status.AttackTimer.SetMin();
        }

        public override void Init()
        {
            base.Init();
            status = base.status as SkillManaBulletStatus;
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