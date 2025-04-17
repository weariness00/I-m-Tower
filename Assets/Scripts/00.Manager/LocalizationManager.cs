using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
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
        public static bool GetLocalizedString(this LocalizeStringEvent localizeStringEvent, string key, out LocalizedString localizedString)
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
    }
}

