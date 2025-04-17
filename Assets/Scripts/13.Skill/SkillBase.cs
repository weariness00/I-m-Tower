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

            LocalizationManager.Instance.onChangeLanguage.AddListener(LocaleChanged);
        }

        public bool InstantiateProjectile()
        {
            if (status.AttackTimer.IsMax)
            {
                var length = Physics.OverlapSphereNonAlloc(transform.position, status.AttackRange, searchColliders, targetLayer);
                var nearTarget = searchColliders.GetNear(transform.position, length);
                if (nearTarget != null)
                {
                    var arrow = projectilePool.Get();
                    arrow.transform.position = transform.position;
                    arrow.targetTransform = nearTarget.transform;
                    arrow.targetStatus = nearTarget.GetComponent<StatusBase>();
                    return true;
                }
            }
            return false;
        }

        public virtual void LocaleChanged(Locale locale)
        {
            skillName = LocalizationSettings.StringDatabase.GetLocalizedString("Skill Table", skillName);
        }
    }

    public abstract partial class SkillBase
    {
        public abstract void LevelUp(int upCount);
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