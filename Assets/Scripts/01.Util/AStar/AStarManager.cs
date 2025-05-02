using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

namespace Util.AStar
{
    public class AStarManager : Singleton<AStarManager>
    {
        public void RequestPath(Vector3 startWorld, Vector3 targetWorld, AStarAgent agent, System.Action<NativeList<int2>> callback)
        {
            AStarGrid grid = AStarGridManager.GetGrid(startWorld);

            var startNode = grid.WorldToGrid(startWorld);
            var endNode = grid.WorldToGrid(targetWorld);

            NativeList<int2> pathResult = new NativeList<int2>(Allocator.Persistent);
            
            PathFindingJob job = new PathFindingJob
            {
                gridSize = grid.gridSize,
                nodeSize = grid.nodeSize,
                gridOffset = grid.center,
                startPos = startNode,
                endPos = endNode,
                nodes = grid.gridNodeNativeArray,
                resultPath = pathResult,
                allowedDirection = agent.findingDirection.directions.ToNativeArray(Allocator.TempJob),
            };

            JobHandle handle = job.Schedule();
            handle.Complete();
            callback?.Invoke(pathResult);
            pathResult.Dispose();
        }
    }
}

