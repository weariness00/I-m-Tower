using System;
using ProjectTile;
using UnityEngine;
using UnityEngine.Pool;

namespace Skill
{
    [RequireComponent(typeof(SkillManaBulletStatus))]
    public class SkillManaBullet : SkillBase
    {
        [NonSerialized] public new SkillManaBulletStatus status;
        
        public ProjectileBase bulletPrefab;
        
        public override void Awake()
        {
            base.Awake();
            status = base.status as SkillManaBulletStatus;
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
            if(InstantiateProjectile()) status.AttackTimer.SetMin();
        }

        public override void LevelUp(int upCount)
        {
            status.DamageMultiple += upCount * 0.5f;
        }

        public override string Explain()
        {
            if(status == null) status = base.status as SkillManaBulletStatus;
            return base.Explain();
        }
    }
}