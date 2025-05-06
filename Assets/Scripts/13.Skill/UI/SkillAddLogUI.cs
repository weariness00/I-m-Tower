using System.Collections;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Skill.UI
{
    public class SkillAddLogUI : MonoBehaviour
    {
        public RectTransform rectTransform;
        public CanvasGroup canvasGroup;
        public Image icon;
        public TMP_Text logText;

        public void SetSkill(SkillBase skill)
        {
            icon.sprite = skill.icon;
            logText.text = $"[{LocalizationExtension.Localize(skill.skillName, SkillPrefabSO.SkillNameTableKey)}] Level({skill.status.level.Current})";
        }

        public IEnumerator AlphaEnumerator(float duration)
        {
            float t = 0;
            while (canvasGroup.alpha != 0)
            {
                t += Time.deltaTime / duration;
                canvasGroup.alpha = Mathf.Lerp(1, 0, t);
                yield return null;
            }
        }
    }
}