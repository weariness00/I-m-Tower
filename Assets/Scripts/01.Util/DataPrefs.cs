using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Util
{
    [InitializeOnLoad]
    public static class DataPrefs
    {
        private static string filePath = Path.Combine(Application.dataPath, "Resources", $"DataPrefs.json");
        private static string fileName = "DataPrefs";
        private static Dictionary<string, string> prefs = new Dictionary<string, string>();

        static DataPrefs()
        {
            LoadPrefs();
        }
        
        public static bool HasKey(string key) => prefs.ContainsKey(key);
        
        public static void SetString(string key, string value)
        {
            prefs[key] = value;
            SavePrefs();
        }

        public static string GetString(string key, string defaultValue = "")
        {
            return prefs.TryGetValue(key, out var pref) ? pref : defaultValue;
        }

        public static void SetInt(string key, int value)
        {
            prefs[key] = value.ToString();
            SavePrefs();
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            if (prefs.ContainsKey(key) && int.TryParse(prefs[key], out int value))
            {
                return value;
            }

            return defaultValue;
        }

        public static void SetFloat(string key, float value)
        {
            prefs[key] = value.ToString();
            SavePrefs();
        }

        public static float GetFloat(string key, float defaultValue = 0.0f)
        {
            if (prefs.ContainsKey(key) && float.TryParse(prefs[key], out float value))
            {
                return value;
            }

            return defaultValue;
        }

        public static void SetBool(string key, bool value)
        {
            prefs[key] = value.ToString();
            SavePrefs();
        }

        public static bool GetBool(string key, bool defaultValue = true)
        {
            if (prefs.ContainsKey(key) && bool.TryParse(prefs[key], out bool value))
            {
                return value;
            }

            return defaultValue;
        }

        private static void SavePrefs()
        {
            string json = JsonUtility.ToJson(new Serialization<string, string>(prefs));
            File.WriteAllText(filePath, json);
        }

        public static void LoadPrefs()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                prefs = JsonUtility.FromJson<Serialization<string, string>>(json).ToDictionary();
            }
        }

        [System.Serializable]
        private class Serialization<TKey, TValue> // Helper class to serialize the dictionary
        {
            public List<TKey> keys = new List<TKey>();
            public List<TValue> values = new List<TValue>();

            public Serialization(Dictionary<TKey, TValue> dict)
            {
                foreach (KeyValuePair<TKey, TValue> pair in dict)
                {
                    keys.Add(pair.Key);
                    values.Add(pair.Value);
                }
            }

            public Dictionary<TKey, TValue> ToDictionary()
            {
                Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
                for (int i = 0; i < keys.Count; i++)
                {
                    dict[keys[i]] = values[i];
                }

                return dict;
            }
        }
    }
}