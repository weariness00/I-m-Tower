using System;

namespace Status
{
    [Serializable]
    public partial class StatModifier
    {
        public enum ModifierType { Flat, Percent }

        public float value = default;
        public ModifierType type;

        public bool isActive;

        public StatModifier(ModifierType t)
        {
            value = default;
            type = ModifierType.Flat;
        }
        
        public StatModifier(ModifierType t, float value)
        {
            this.value = value;
            type = t;
        }
    }
}