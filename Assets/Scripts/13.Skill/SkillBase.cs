using System;
using Leveling.Age;
using Status;
using UnityEngine;

namespace Skill
{
    public abstract partial class SkillBase : MonoBehaviour
    {
        public int id;
        public string skillName;
        public Sprite icon;

        [Space] 
        public AgeType ageType;
        [HideInInspector] public LayerMask targetLayer;

        public SkillStatus status;
        
        protected Collider[] searchColliders;

        public virtual void Awake()
        {
            targetLayer = LayerMask.GetMask("Monster");
            Init();
        }

        public virtual void Init()
        {
            status = GetComponent<SkillStatus>();
            status.ownerObject = gameObject;
        }
    }

    public partial class SkillBase
    {
        public virtual string Explain()
        {
            return $"{skillName}\n";
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