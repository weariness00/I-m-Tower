using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization.Tables;
using Util;

namespace Manager
{
    public class LocalizationManager : Singleton<LocalizationManager>
    {
        public UnityEvent<Locale> onChangeLanguage = new();

        public void OnEnable()
        {
            LocalizationSettings.SelectedLocaleChanged += OnLocalChanged;
        }

        private void OnLocalChanged(Locale locale)
        {
            onChangeLanguage.Invoke(locale);
            
            Debug.Log($"Locale Changed To : {locale.Identifier}");
        }
    }

    public static class LocalizationExtension
    {
        public static bool GetVariableLocalizedString(this LocalizeStringEvent localizeStringEvent, string key, out LocalizedString localizedString)
        {
            if (localizeStringEvent.StringReference.TryGetValue(key, out IVariable variable) &&
                variable is LocalizedString ls)
            {
                localizedString = ls;
                return true;
            }

            localizedString = null;
            return false;
        }
        
        public static string Localize(string key, string tableName)
        {
            // 현재 로케일 기준 테이블을 동기 접근
            var table = LocalizationSettings.StringDatabase.GetTable(tableName);
            if (table == null)
            {
                Debug.LogWarning($"Table not found: {tableName}");
                return key;
            }

            var entry = table.GetEntry(key);
            if (entry == null)
            {
                Debug.LogWarning($"Key not found: {key}");
                return key;
            }

            return entry.GetLocalizedString();
        }
    }
}

