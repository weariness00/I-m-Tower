using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Util;

namespace Manager   
{
#if UNITY_EDITOR
    public static class DebugSettingProvider
    {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var provider = new SettingsProvider(
                "Project/Managers/Debug", 
                SettingsScope.Project, 
                new []{ "Scriptable", "Manager", "Debug"})
            {
                guiHandler = (searchContext) =>
                {
                    // 설정 창에 표시할 UI
                    DebugSettingProviderHelper.IsDebug = EditorGUILayout.Toggle("Is Debug", DebugSettingProviderHelper.IsDebug); 
                    EditorGUILayout.LabelField("Debug Manager Data", EditorStyles.boldLabel);
                    var setting = DebugSettingProviderHelper.setting = (DebugScriptableObject)EditorGUILayout.ObjectField(
                        $"Debug Manager Data",
                        DebugSettingProviderHelper.setting,
                        typeof(DebugScriptableObject),
                        false
                    );

                    if (setting != null)
                    {
                        Editor.CreateEditor(setting).OnInspectorGUI();
                        
#if ENABLE_BUILD
                        Debug.Log("ENABLE_BUILD가 활성화되었습니다.");
#else
                        Debug.Log("ENABLE_BUILD가 정의되지 않았습니다.");
#endif
                    }
                    
                    // setting이 변경되었을 경우 Save() 호출
                    if (GUI.changed)
                    {
                        DebugSettingProviderHelper.Save();
                    }
                },
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

    public static class DebugSettingProviderHelper
    {
        private static bool _IsDebug = true;
        public static DebugScriptableObject setting;

        private static readonly string JsonDirectory = "Assets/Resources/Data/Json";
        private static readonly string SettingJsonPath = "Resources/Data/Json/Debug Manager Data.json";
        private static readonly string DefaultKey = "Game,Level List";
        private static readonly string DebugKey = "IsDebug";
        private static readonly string SettingKey = nameof(DebugScriptableObject);

#if UNITY_EDITOR
        public static bool IsDebug
        {
            get => _IsDebug;
            set
            {
                if (_IsDebug != value)
                    DataPrefs.SetBool(DefaultKey + DebugKey, value);
                _IsDebug = value;
            }
        }

        static DebugSettingProviderHelper()
        {
            IsDebug = DataPrefs.GetBool(DefaultKey + DebugKey);
            if (!Directory.Exists(JsonDirectory))
                Directory.CreateDirectory(JsonDirectory);
            AssetDatabase.Refresh();
            Load();
        }

        public static void Save()
        {
            if (setting != null)
            {
                string path = AssetDatabase.GetAssetPath(setting);
                SettingJson data = new();
                data.SettingPath = path;
                string json = JsonUtility.ToJson(data, true);

                File.WriteAllText(Path.Combine(Application.dataPath, SettingJsonPath), json);
                AssetDatabase.Refresh();

                DataPrefs.SetString(DefaultKey + SettingKey, path);
            }
        }

        public static void Load()
        {
            if (DataPrefs.HasKey(DefaultKey + SettingKey))
            {
                string settingPath = DataPrefs.GetString(DefaultKey + SettingKey, string.Empty);
                setting = AssetDatabase.LoadAssetAtPath<DebugScriptableObject>(settingPath);
                Debug.Assert(setting != null, $"해당 경로에 {nameof(DebugScriptableObject)} 데이터가 존재하지 않습니다.");
            }
        }
#else
        static DebugSettingProviderHelper()
        {
            Load();
        }

        public static void Load()
        {
            var settingTextFile = Resources.Load<TextAsset>(SettingJsonPath.Replace("Resources/", "").Replace(".json",""));
            if (settingTextFile != null)
            {
                string json = settingTextFile.text;
                var data = JsonUtility.FromJson<SettingJson>(json);
                var path = data.SettingPath;
                path = path.Replace("Assets/", "");
                path = path.Replace("Resources/", "");
                path = path.Replace(".asset", "");
                setting = Resources.Load<DebugScriptableObject>(path);
            }
        }
#endif
    }
}