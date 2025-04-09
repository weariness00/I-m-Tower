using Game.Status;
using UnityEngine;
using Util;

namespace Unit
{
    [CreateAssetMenu(fileName = "Unit Data", menuName = "Game/Unit/Data", order = 0)]
    public class UnitSO : ScriptableObject
    {
        public int id;
        
        public UnitType type;
        public UnitAttackType attackType;
        
        public StatusData data;
    }
}