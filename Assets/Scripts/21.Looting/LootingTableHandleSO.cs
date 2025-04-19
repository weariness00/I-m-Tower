using System;
using UnityEngine;

namespace Looting
{
    [CreateAssetMenu(fileName = "Looting Table Handle", menuName = "Game/Looting/List Handle", order = 0)]
    public class LootingTableHandleSO : ScriptableObject
    {
        public static LootingTableHandleSO Instance => SettingProviderHelper.setting;
        [SerializeField] private LootingTableSO[] lootingTableSOArray;

        public static LootingTableSO GetLootingTableSO(int id)
        {
            var index = Array.BinarySearch(Instance.lootingTableSOArray, id);
            return index >= 0 ? Instance.lootingTableSOArray[index] : null;
        }
    }
}