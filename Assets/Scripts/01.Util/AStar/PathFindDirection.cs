using System;

namespace Util.AStar
{
    [Flags]
    public enum PathFindDirection
    {
        UpLeft = 1 << 0,
        Up = 1 << 1,    
        UpRight = 1 << 2,
        Left = 1 << 3,
        Right = 1 << 4,
        DownLeft = 1 << 5,
        Down = 1 << 6,
        DownRight = 1 << 7,
    }
}