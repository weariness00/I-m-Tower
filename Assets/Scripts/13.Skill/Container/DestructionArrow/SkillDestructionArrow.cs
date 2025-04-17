using System;
using Game.Status;
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
        
        public override void Awake()
        {
            base.Awake();
            status = base.status as SkillDestructionArrowStatus;
            searchColliders = new Collider[100];
            
            projectilePool = new(
                () =>
                {
                    var arrow = Instantiate(arrowPrefab);
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
            
            if(InstantiateProjectile()) status.AttackTimer.SetMin();
        }

        public override void LevelUp(int upCount)
        {
            status.DamageMultiple += upCount * 0.5f;
        }
        
        public override string Explain()
        {
            if(status == null) status = base.status as SkillDestructionArrowStatus;
            return base.Explain();
        }
    }
}