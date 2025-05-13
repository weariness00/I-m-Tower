using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using Util;

namespace Skill
{
    [CreateAssetMenu(fileName = "Skill Prefab List", menuName = "Game/Skill/Prefab List", order = 0)]
    public class SkillPrefabSO : ScriptableObject
    {
        public static SkillPrefabSO Instance => Skill.SettingProviderHelper.setting;

        public static readonly string SkillNameTableKey = "Skill Name";
        public SkillBase[] skillArray;

        public void Init()
        {
            Array.Sort(skillArray, (a,b) => a.id.CompareTo(b.id));
        }

        public static SkillBase GetSkill(int id)
        {
            var index = Array.BinarySearch(Instance.skillArray, id);
            return index >= 0 ? Instance.skillArray[index] : null;
        }

        public static SkillBase GetRandomSkill() => Instance.skillArray.Random();

        public static SkillBase GetRandomSKillExcept(SkillBase[] exceptSkillArray)
        {
            // skill이 200개 이상일 경우에는 hashset을 사용하여 성능을 개선
            var arr = Instance.skillArray.Except(exceptSkillArray).ToArray();
            return arr.Random();
        }
    }
}