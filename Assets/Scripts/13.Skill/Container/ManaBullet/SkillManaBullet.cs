using System;
using ProjectTile;
using Status;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using Util;

namespace Skill
{
    [RequireComponent(typeof(SkillManaBulletStatus))]
    public class SkillManaBullet : SkillBase
    {
        [SerializeField] private SkillManaBulletStatus status;
        
        public ProjectileBase bulletPrefab;
        private ObjectPool<ProjectileBase> projectilePool;

        public override SkillStatus Status => status;
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
            status.attackTimer.Current += Time.deltaTime;
            if (status.attackTimer.IsMax && TryInstantiateProjectile(out var bullet)) status.attackTimer.SetMin();
        }

        private bool TryInstantiateProjectile(out ProjectileBase projectile)
        {
            var length = Physics.OverlapSphereNonAlloc(transform.position, status.attackRange, searchColliders, targetLayer);
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