using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Util;

namespace Skill
{
#if UNITY_EDITOR
    
    public static class SkillProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateSettingProvider()
        {
            var provider = new SettingsProvider(
                "Project/Game Play/Skill",
                SettingsScope.Project,
                new []{"Game", "Skill"})
            {
                guiHandler = searchContext =>
                {
                    EditorGUILayout.LabelField("Skill", EditorStyles.boldLabel);
                    var setting = SettingProviderHelper.setting = (SkillPrefabSO)EditorGUILayout.ObjectField(
                        $"User Data",
                        SettingProviderHelper.setting,
                        typeof(SkillPrefabSO),
                        false
                    );

                    if (setting != null)
                    {
                        Editor.CreateEditor(setting).OnInspectorGUI();
                    }
                    
                    // setting이 변경되었을 경우 Save() 호출
                    if (GUI.changed)
                    {
                        SettingProviderHelper.Save();
                    }
                }
            };

            return provider;
        }
    }
    
#endif   
    
    [Serializable]
    public struct SettingJson
    {
        public string SettingPath;
    }

    public static class SettingProviderHelper
    {
        public static SkillPrefabSO setting;

        private static readonly string JsonDirectory = "Assets/Resources/Data/Json";
        private static readonly string SettingKey = nameof(SkillPrefabSO);

#if UNITY_EDITOR
        static SettingProviderHelper()
        {
            if (!Directory.Exists(JsonDirectory))
                Directory.CreateDirectory(JsonDirectory);
            AssetDatabase.Refresh();
            Load();
        }

        public static void Save()
        {
            SettingJson data = new();
            if (setting != null)
            {
                string path = AssetDatabase.GetAssetPath(setting);
                data.SettingPath = path;
                string json = JsonUtility.ToJson(data, true);

                File.WriteAllText(Path.Combine(Application.dataPath, JsonDirectory.Replace("Assets/", ""), SettingKey + ".json"), json);
                AssetDatabase.Refresh();

                DataPrefs.SetString(SettingKey, path);
            }
        }

        public static void Load()
        {
            if (DataPrefs.HasKey(SettingKey))
            {
                string settingPath = DataPrefs.GetString(SettingKey, string.Empty);
                setting = AssetDatabase.LoadAssetAtPath<SkillPrefabSO>(settingPath);
                Debug.Assert(setting != null, $"해당 경로에 {nameof(SkillPrefabSO)} 데이터가 존재하지 않습니다.");
            }
        }
        
#else
        static SettingProviderHelper()
        {
            Load();
        }

        public static void Load()
        {
            var settingTextFile = Resources.Load<TextAsset>(JsonDirectory.Replace("Assets/","").Replace("Resources/", "").Replace(".json",""));
            if (settingTextFile != null)
            {
                string json = settingTextFile.text;
                var data = JsonUtility.FromJson<SettingJson>(json);
                var path = GetDataPath(data.SettingPath);
                setting = Resources.Load<UnitSOManager>(path);
            }
        }
#endif
        public static string GetDataPath(string path)
        {
            path = path.Replace("Assets/", "");
            path = path.Replace("Resources/", "");
            path = path.Replace(".asset", "");
            return path;
        }
    }
}