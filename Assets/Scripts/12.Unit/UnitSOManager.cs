using UnityEngine;

namespace Unit
{
    [CreateAssetMenu(fileName = "Unit Data", menuName = "Game/Unit/List Data", order = 1)]
    public class UnitSOManager : ScriptableObject
    {
        public static UnitSOManager Instance => Unit.SettingProviderHelper.setting;
        public UnitSO[] unitDataArray;
    }
}

