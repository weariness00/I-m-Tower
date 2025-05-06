using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Skill.UI
{
    public class SkillSelectUI : MonoBehaviour
    {
        public SkillBase skill;
        
        public Button button;
        
        public Image icon;
        public TMP_Text nameText;
        public TMP_Text explainText;
        public LocalizeStringEvent nameLSE;
        public LocalizeStringEvent explainLSE;

        public void SetSKill(SkillBase _skill)
        {
            skill = _skill;
            skill.Init();
            icon.sprite = skill.icon;
            nameText.text = skill.skillName;
            explainText.text = skill.Explain();
            
            nameLSE.StringReference.TableEntryReference = skill.skillName;
            nameLSE.RefreshString();
            
            explainLSE.StringReference.TableEntryReference = skill.skillName;
            explainLSE.RefreshString();
        }
    }
}