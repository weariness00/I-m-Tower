using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
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
}

