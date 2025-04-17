using System;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

namespace Skill.UI
{
    public class SkillSelectUI : MonoBehaviour
    {
        public SkillBase skill;
        
        public Button button;
        
        public Image icon;
        public TMP_Text explainText;
        public LocalizeStringEvent localizeStringEvent;

        public void Awake()
        {
            localizeStringEvent.StringReference.StringChanged += Localize;
        }

        public void OnDestroy()
        {
            localizeStringEvent.StringReference.StringChanged -= Localize;
        }

        private void Localize(string value)
        {
            LocalizedString localizedString;
            if (localizeStringEvent.GetLocalizedString("skillName", out localizedString))
            {
                localizedString.TableEntryReference = skill.skillName;
            }
            if (localizeStringEvent.GetLocalizedString("skillExplain", out localizedString))
            {
                localizedString.TableEntryReference = skill.skillName;
            }
        }
        
    }
}