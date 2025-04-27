using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Util.AStar
{
    public class AStarAgent : MonoBehaviour
    {
        public PathFindDirection findingDirection;
        public List<int2> path;
        private int2 currentPoint;

#if UNITY_EDITOR
        [InspectorReadOnly][SerializeField] private Vector3 destination;
#endif

        public void SetDestination(Vector3 destination)
        {
            AStarManager.Instance.RequestPath(transform.position, destination, this, SetPath);
        }
        public void Find(Vector3 startPoint, Vector3 endPoint)
        {
            AStarManager.Instance.RequestPath(startPoint, endPoint, this, SetPath);
        }
        
        public void SetPath(NativeList<int2> newPath)
        {
            path.Clear();
            path = new();
            foreach (var value in newPath)
                path.Add(value);

            if (newPath.Length > 0)
            {
                currentPoint = path[0];
            }
        }

        public void Move(float speed)
        {
            var grid = AStarGridManager.GetGrid(transform.position);
            var currentPosition = grid.GridToWorld(currentPoint);

#if UNITY_EDITOR
            destination = grid.GridToWorld(currentPoint);
#endif
            
            transform.position = Vector3.MoveTowards(transform.position, currentPosition, speed);
            
            if (Vector3.Distance(transform.position, currentPosition) < 0.1f)
            {
                    path.RemoveAt(0);
                    if (path.Count > 0)
                    {
                        currentPoint = path[0];
                    }
            }
        }
    }
}