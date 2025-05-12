using System;
using UnityEngine;

namespace Unit
{
    [CreateAssetMenu(fileName = "Unit Data", menuName = "Game/Unit/Data", order = 0)]
    public partial class UnitSO : ScriptableObject
    {
        public int id;
        
        public UnitType type;
        public UnitAttackType attackType;
    }

    public partial class UnitSO : IComparable, IComparable<UnitSO>
    {
        public int CompareTo(object obj)
        {
            if(obj is int otherID)
                return id.CompareTo(otherID);
            return 0;
        }

        public int CompareTo(UnitSO other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return id.CompareTo(other.id);
        }
    }

}