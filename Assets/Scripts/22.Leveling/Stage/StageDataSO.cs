using System;
using Game.Age;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

namespace Leveling.Stage
{
    [CreateAssetMenu(fileName = "Stage Data", menuName = "Game/Leveling/Stage Data", order = 0)]
    public partial class StageDataSO : ScriptableObject
    {
        public AgeType age;
        public StageData[] stageDataArray;

        public StageData GetStage(int level)
        {
            int index = Array.BinarySearch(stageDataArray, level);
            return index < 0 ? stageDataArray[^1] : stageDataArray[index];
        }
    }
    
    public partial class StageDataSO : IComparable, IComparable<StageDataSO>
    {
        public int CompareTo(object obj)
        {
            if (obj is AgeType ageType)
            {
                return age.CompareTo(ageType);
            }

            return 0;
        }

        public int CompareTo(StageDataSO other)
        {
            return other.age.CompareTo(age);
        }
    }
}