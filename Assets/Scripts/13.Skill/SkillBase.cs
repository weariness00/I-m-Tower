using System;
using Game;
using UnityEngine;

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

        public virtual void Awake()
        {
            targetLayer = LayerMask.GetMask("Monster");
            status = GetComponent<SkillStatus>();
            
            status.onLevelUpEvent.AddListener(LevelUp);
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