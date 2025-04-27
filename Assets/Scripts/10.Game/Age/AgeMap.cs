using System;
using System.Collections.Generic;
using System.Linq;
using Game.Age;
using Unit.Monster;
using UnityEngine;

namespace Game
{
    public class AgeMap : MonoBehaviour
    {
        [SerializeField] private MapData[] mapDataArray;
        public MapData CurrentMapData { get; private set; }

        public void Awake()
        {
            Array.Sort(mapDataArray, (a,b) => a.age.CompareTo(b.age));
        }

        public void ChangeMap(AgeType age)
        {
            if(CurrentMapData != null) CurrentMapData.mapObject.SetActive(false);
            CurrentMapData = mapDataArray.FirstOrDefault(map => map.age == age);
            CurrentMapData.mapObject.SetActive(true);
        }

        [Serializable]
        public class MapData
        {
            public AgeType age;
            public GameObject mapObject;
            public MonsterSpawner[] monsterSpawnerArray;
        }
    }
}