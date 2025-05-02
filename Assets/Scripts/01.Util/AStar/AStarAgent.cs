using System;
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

        [Space] public bool isStop;

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
                isStop = false;
            }
        }

        public void Move(float speed)
        {
            if(isStop) return;

            var grid = AStarGridManager.GetGrid(transform.position);
            destination = grid.GridToWorld(currentPoint);
            transform.position = Vector3.MoveTowards(transform.position, destination, speed);
            
            if (Vector3.Distance(transform.position, destination) < 0.1f)
            {
                if (path.Count > 0)
                    path.RemoveAt(0);
                if (path.Count > 0)
                    currentPoint = path[0];
                else
                    isStop = true;
            }
        }

        public void Look()
        {
            if(isStop) return;
            transform.LookAt(destination);
        }
    }
}