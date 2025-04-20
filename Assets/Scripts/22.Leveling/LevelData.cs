using System;
using Unit.Monster;
using UnityEngine;

namespace Leveling
{
    [Serializable]
    public partial class LevelData
    {
        public float awakeWaitTime;
        public MonsterControl monsterPrefab;
        public int spawnCount;
        public float spawnInterval;
    }
}