using System.Collections.Generic;
using UnityEngine;

namespace Util.AStar
{
    public static class AStarGridManager
    {
        private static List<AStarGrid> gridList = new();
        
        public static void Subscribe(AStarGrid grid)
        {
            gridList.Add(grid);
        }
        
        public static void Unsubscribe(AStarGrid grid)
        {
            gridList.Remove(grid);
        }
        
        public static AStarGrid GetGrid(Vector3 worldPos)
        {
            foreach (var grid in gridList)
            {
                if (grid.IsPointInGrid(worldPos))
                {
                    return grid;
                }
            }
            return null;
        }
    }
}

