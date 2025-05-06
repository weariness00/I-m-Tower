using System;
using System.Linq;
using Unit.Monster;
using UnityEngine;

namespace Leveling.Age
{
    public class AgeMap : MonoBehaviour
    {
        [InspectorReadOnly] public AgeType currentAge;
        [SerializeField] private MapData[] mapDataArray;
        public MapData CurrentMapData { get; private set; }

        public void Awake()
        {
            CurrentMapData = null;
            Array.Sort(mapDataArray, (a,b) => a.age.CompareTo(b.age));
        }

        public void ChangeMap(AgeType age)
        {
            if(CurrentMapData != null && CurrentMapData.mapObject != null) CurrentMapData.mapObject.SetActive(false);
            CurrentMapData = mapDataArray.FirstOrDefault(map => map.age == age);
            CurrentMapData.mapObject.SetActive(true);

            age = currentAge;
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