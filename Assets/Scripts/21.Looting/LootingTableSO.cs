using System;
using UnityEngine;
using Util;

namespace Looting
{
    [CreateAssetMenu(fileName = "Looting Table Data", menuName = "Game/Looting/Table Data", order = 0)]
    public partial class LootingTableSO : ScriptableObject
    {
        public int id;
        [SerializeField] private LootingData[] lootingDataArray;
        
        public void OnLooting(Transform lootingTransform)
        {
            foreach (var data in lootingDataArray)
            {
                if (data.probability.IsProbability())
                {
                    LootingManager.Instance.OnLootingItem(data.id, data.prefab, lootingTransform);
                }
            }
        }
    }

    public partial class LootingTableSO : IComparable, IComparable<LootingTableSO>
    {
        public int CompareTo(object obj)
        {
            if(obj is int otherID)
                return id.CompareTo(otherID);
            return 0;
        }

        public int CompareTo(LootingTableSO other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return id.CompareTo(other.id);
        }
    }
}