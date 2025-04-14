using System;
using UnityEngine;

namespace Skill
{
    [CreateAssetMenu(fileName = "Skill Prefab List", menuName = "Game/Skill/Prefab List", order = 0)]
    public class SkillPrefabSO : ScriptableObject
    {
        public static SkillPrefabSO Instance => Skill.SettingProviderHelper.setting;
        
        public SkillBase[] skillArray;

        private void Init()
        {
            Array.Sort(skillArray, (a,b) => a.id.CompareTo(b.id));
        }

        public static SkillBase GetSkill(int id)
        {
            var index = Array.BinarySearch(Instance.skillArray, id);
            return index >= 0 ? Instance.skillArray[index] : null;
        }
    }
}