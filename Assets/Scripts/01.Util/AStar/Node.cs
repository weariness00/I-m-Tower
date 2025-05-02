using System;
using Unity.Mathematics;

namespace Util.AStar
{
    [Serializable]
    public struct Node
    {
        public int2 position;
        public bool walkable;

        public float gCost;
        public float hCost;
        public int2 parent;

        public float FCost => gCost + hCost;
    }
}