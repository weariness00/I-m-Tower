using System;
using Status;
using ProjectTile;
using UnityEngine;
using UnityEngine.Pool;
using Util;

namespace Skill
{
    [RequireComponent(typeof(SkillDestructionArrowStatus))]
    public class SkillDestructionArrow : SkillBase
    {
        [NonSerialized] public new SkillDestructionArrowStatus status;

        public ProjectileBase arrowPrefab;

        private Collider[] searchColliders = new Collider[100];
        private ObjectPool<ProjectileBase> projectilePool;
        
        public override void Awake()
        {
            base.Awake();
            searchColliders = new Collider[100];
            
            projectilePool = new(
                () =>
                {
                    var arrow = Instantiate(arrowPrefab);
                    arrow.Move = new TargetMove(arrow);
                    arrow.ownerObject = gameObject;
                    arrow.ownerStatus = status;
                    arrow.collider.includeLayers = LayerMask.GetMask("Monster");
                    arrow.pool = projectilePool;

                    return arrow;
                },
                arrow => arrow.gameObject.SetActive(true),
                arrow => arrow.gameObject.SetActive(false),
                arrow => Destroy(arrow.gameObject));
        }

        public void Update()
        {
            status.AttackTimer.Current += Time.deltaTime;
            
            if(status.AttackTimer.IsMax && TryInstantiateProjectile(out var arrow)) status.AttackTimer.SetMin();
        }

        public override void Init()
        {
            base.Init();
            status = base.status as SkillDestructionArrowStatus;
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