using System;
using System.Linq;
using Game.Age;
using Leveling.Stage;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Util;

namespace Leveling
{
    [CreateAssetMenu(fileName = "Leveling Data", menuName = "Game/Leveling/Level", order = 0)]
    public partial class LevelingDataSO : ScriptableObject
    {
        public static LevelingDataSO Instance => SettingProviderHelper.setting;
        [SerializeField] private StageDataSO[] stageDataSoArray;
        
        public StageDataSO GetStageDataSO(AgeType ageType)
        {
            return stageDataSoArray.FirstOrDefault(stageDataSo => stageDataSo.age == ageType);
        }
    }
    
    #if UNITY_EDITOR

    public partial class LevelingDataSO
    {
        public TextAsset levelingDataJson;

        public void JsonLoad()
        {
            var stageDataArray = JsonConvert.DeserializeObject<StageData[]>(levelingDataJson.text);
            if(stageDataArray == null)
            {
                Debug.LogError($"Leveling Data Load Error : {levelingDataJson.name}");
                return;
            }
            
            Array.Sort(stageDataArray, (a, b) => a.age.CompareTo(b.age));
            var grouped = stageDataArray
                .GroupBy(stage => stage.age)
                .ToDictionary(age => age.Key, age => age.ToArray());
            
            if (stageDataSoArray != Array.Empty<StageDataSO>())
                ScriptableObjectExtension.DeleteSOAsset(stageDataSoArray);
            ScriptableObjectExtension.GenerateSOAssets<StageDataSO>(grouped.Count, "Assets/Resources/Game/Leveling", out stageDataSoArray);

            int i = 0;
            foreach (var (key, value) in grouped)
            {
                var stageDataSo = stageDataSoArray[i];
                stageDataSo.age = key;
                stageDataSo.stageDataArray = value;
                ++i;
                
                EditorUtility.SetDirty(stageDataSo);
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    
    #endif
}