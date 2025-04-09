namespace Unit
{
    [System.Flags]
    public enum UnitType
    {
        Ground = 1 << 0,
        Air = 1 << 1,
    }
}