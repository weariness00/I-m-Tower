using System;
using UnityEngine;

namespace Unit
{
    [CreateAssetMenu(fileName = "Unit Data", menuName = "Game/Unit/List Data", order = 1)]
    public class UnitSOManager : ScriptableObject
    {
        public static UnitSOManager Instance => Unit.SettingProviderHelper.setting;
        public UnitSO[] monsterDataArray;

        public void Init()
        {
            Array.Sort(monsterDataArray);
        }

        public static UnitSO GetMonsterSO(int id)
        {
            var index = Array.BinarySearch(Instance.monsterDataArray, id);
            return index >= 0 ? Instance.monsterDataArray[index] : null;
        }
    }
}

