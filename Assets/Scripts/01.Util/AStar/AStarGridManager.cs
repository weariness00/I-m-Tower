using System.Collections.Generic;
using UnityEngine;

namespace Util.AStar
{
    public class AStarGridManager : Singleton<AStarGridManager>
    {
        private List<AStarGrid> gridList = new();
        
        public void Subscribe(AStarGrid grid)
        {
            gridList.Add(grid);
        }
        
        public void Unsubscribe(AStarGrid grid)
        {
            gridList.Remove(grid);
        }
        
        public AStarGrid GetGrid(Vector3 worldPos)
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

