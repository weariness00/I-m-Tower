using System;
using UnityEngine;
using Util.UniqueID;

namespace Looting
{
    [Serializable]
    public struct LootingData
    {
        public UniqueIdentifier id;
        [Range(0,1)] public float probability;
        public GameObject prefab;
    }
    
}