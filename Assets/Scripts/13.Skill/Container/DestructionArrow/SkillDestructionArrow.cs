using System;
using ProjectTile;
using UnityEngine;
using UnityEngine.Pool;
using Util;

namespace Skill
{
    public class SkillDestructionArrow : SkillBase
    {
        [NonSerialized] public new SkillDestructionArrowStatus status;

        public ProjectileBase arrowPrefab;
        public ObjectPool<ProjectileBase> arrowPool;

        private Collider[] searchColliders = new Collider[100];

        public override void Awake()
        {
            base.Awake();
            status = base.status as SkillDestructionArrowStatus;

            arrowPool = new(
                () =>
                {
                    var arrow = Instantiate(arrowPrefab);
                    arrow.ownerObject = gameObject;
                    arrow.ownerStatus = status;
                    arrow.collider.includeLayers = LayerMask.GetMask("Monster");
                    arrow.pool = arrowPool;

                    return arrow;
                },
                arrow => arrow.gameObject.SetActive(true),
                arrow => arrow.gameObject.SetActive(false),
                arrow => Destroy(arrow.gameObject));
        }

        public void Update()
        {
            status.AttackTimer.Current += Time.deltaTime;

            if (status.AttackTimer.IsMax)
            {
                Physics.OverlapSphereNonAlloc(transform.position, status.AttackRange, searchColliders, targetLayer);
                var nearTarget = searchColliders.GetNear(transform.position);
                if (nearTarget != null)
                {
                    status.AttackTimer.SetMin();
                    var arrow = arrowPool.Get();
                    arrow.transform.position = transform.position;
                    arrow.targetTransform = nearTarget.transform;
                }
            }
        }
    }
}