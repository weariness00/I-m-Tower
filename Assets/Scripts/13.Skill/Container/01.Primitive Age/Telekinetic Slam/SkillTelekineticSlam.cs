using System;
using ProjectTile;
using UnityEngine;

namespace Skill
{
    [RequireComponent(typeof(SkillTelekineticSlamStatus))]
    public class SkillTelekineticSlam : SkillBase
    {
        [NonSerialized] public new SkillTelekineticSlamStatus status;

        public ProjectileBase debrisPrefab;
        public override void Awake()
        {
            base.Awake();
            status = base.status as SkillTelekineticSlamStatus;

            searchColliders = new Collider[100];
            projectilePool = new(
                () =>
                {
                    var debris = Instantiate(debrisPrefab);
                    debris.ownerObject = gameObject;
                    debris.ownerStatus = status;
                    debris.pool = projectilePool;

                    return debris;
                },
                debris => debris.gameObject.SetActive(true),
                debris => debris.gameObject.SetActive(false),
                debris => Destroy(debris.gameObject));
        }

        public void Update()
        {
            status.AttackTimer.Current += Time.deltaTime;
            if (status.AttackTimer.IsMax &&
                TryInstantiateProjectile(out var projectile) &&
                projectile is TelekineticSlamDebris debris)
            {
                debris.Move = new TargetAroundMove(debris)
                {
                    rotateAxis = Vector3.up,
                    rotateSpeed = 0,
                    radius = 5,
                };
                debris.StartCoroutine(debris.A());
                status.AttackTimer.SetMin();
            }
        }

        public override void LevelUp(int upCount)
        {
            status.value.damage += status.upDamageSize * upCount;
        }
        
        public override string Explain()
        {
            if(status == null) status = base.status as SkillTelekineticSlamStatus;
            return base.Explain();
        }
    }
}

