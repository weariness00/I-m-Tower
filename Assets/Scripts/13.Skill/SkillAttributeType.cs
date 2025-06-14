using UnityEngine;

namespace Skill
{
    public enum SkillAttributeType
    {
        [InspectorName("불")] Fire,
        [InspectorName("물")] Water,
        [InspectorName("바람")] Wind,
        [InspectorName("땅")] Earth,
        [InspectorName("빛")] Light,
        [InspectorName("어둠")] Dark,
        [InspectorName("무")] None,
    }
}