using System;
using Game;
using Game.Status;
using Manager;
using ProjectTile;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Pool;
using Util;

namespace Skill
{
    public partial class SkillBase : MonoBehaviour
    {
        public int id;
        public string skillName;
        public Sprite icon;

        [Space]
        [InspectorName("등급")] public RatingType rating;
        [HideInInspector] public LayerMask targetLayer;

        [NonSerialized] public SkillStatus status;
        
        public ObjectPool<ProjectileBase> projectilePool;
        protected Collider[] searchColliders;

        public virtual void Awake()
        {
            targetLayer = LayerMask.GetMask("Monster");
            status = GetComponent<SkillStatus>();
            
            status.onLevelUpEvent.AddListener(LevelUp);
        }

        public bool TryInstantiateProjectile(out ProjectileBase projectile)
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

    public abstract partial class SkillBase
    {
        public abstract void LevelUp(int upCount);

        public virtual string Explain()
        {
            if (status == null) status = GetComponent<SkillStatus>();
            return $"{skillName}\n" + $"공격력 {status.Damage}, 공격 속도 {status.AttackSpeed}";
        }
    }

    public partial class SkillBase : IComparable, IComparable<SkillBase>
    {
        public int CompareTo(SkillBase other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return id.CompareTo(other.id);
        }

        public int CompareTo(object obj)
        {
            if (obj is int otherID)
            {
                return id.CompareTo(otherID);
            }

            return 0;
        }
    }
}