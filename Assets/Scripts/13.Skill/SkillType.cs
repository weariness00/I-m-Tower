using UnityEngine;

namespace Skill
{
    public enum SkillType
    {
        [InspectorName("공격형")] Attack,
        [InspectorName("강화형")] Reinforce,
        [InspectorName("방해형")] Obstruct,
    }
}