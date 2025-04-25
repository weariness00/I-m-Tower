using UnityEngine;

namespace Game.Age
{
    public enum AgeType
    {
        [InspectorName("원시 시대")] Primitive,
        [InspectorName("태고 시대")] Primordial,
        [InspectorName("고대 문명 시대")] Ancient,
        [InspectorName("중세 시대")] Medieval,
        [InspectorName("근대 시대")] Modern,
        [InspectorName("현대 시대")] Contemporary,
        [InspectorName("미래 시대")] Future,
        [InspectorName("무한의 시대")] Endless,
    }
}