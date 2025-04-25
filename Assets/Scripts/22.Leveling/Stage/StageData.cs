using System;
using Game.Age;
using Newtonsoft.Json;
using UnityEngine.Serialization;
using Util;

namespace Leveling.Stage
{
    [Serializable]
    public partial class StageData
    {
        public AgeType age;
        [JsonProperty("Level Range")]                         public MinMax<int> levelRange;
        [JsonProperty("Clear Gold")]                          public int clearGold;
        [JsonProperty("Clear Gold Multiple Per Stage Level")] public int clearGoldMultiplePerStageLevel;
        
        [JsonProperty("HP")]                          public int monsterHP;
        [JsonProperty("HP Multiple Per Stage Level")] public int monsterHPMultiplePerStageLevel;
        [JsonProperty("Attack")]                      public int monsterAttack;
        [JsonProperty("Attack Multiple Per Stage Level")] public int monsterAttackMultiplePerStageLevel;
        [JsonProperty("Defense")]                     public int monsterDefense;
        [JsonProperty("Defense Multiple Per Stage Level")] public int monsterDefenseMultiplePerStageLevel;
        [JsonProperty("EXP")]                         public int monsterEXP;
        [JsonProperty("EXP Multiple Per Stage Level")] public int monsterEXPMultiplePerStageLevel;
    }
    
    public partial class StageData : IComparable, IComparable<StageData>
    {
        public int CompareTo(object obj)
        {
            if(obj is int level)
            {
                if (levelRange.IsInRange(level))
                    return 0;
                return level < levelRange.Min ? -1 : 1;
            }

            return 0;
        }

        public int CompareTo(StageData other)
        {
            return levelRange.Min.CompareTo(other.levelRange.Min);
        }
    }
}