using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Skill.UI
{
    public partial class SkillInfoSummeryUI : MonoBehaviour
    {
        public int skillID;
        [Tooltip("스킬 아이콘")]
        public Image icon;
        [Tooltip("스킬 이름")]
        public TMP_Text nameText;
        [Tooltip("스킬 레벨 텍스트 \"LV.현재/최대\"")]
        public TMP_Text levelText;
    }
    
    public partial class SkillInfoSummeryUI : IComparable
    {
        public int CompareTo(object obj)
        {
            if (obj is int otherID)
            {
                return skillID.CompareTo(otherID);
            }

            return 0;
        }
    }
}