using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Util.AStar
{
    public class AStarManager : MonoBehaviour
    {
        public void RequestPath(Vector3 startWorld, Vector3 targetWorld, System.Action<NativeList<int2>> callback)
        {
            AStarGrid grid = AStarGridManager.Instance.GetGrid(startWorld);
            
            var startNode = WorldToGrid(grid, startWorld);
            var endNode = WorldToGrid(grid, targetWorld);

            NativeList<int2> pathResult = new NativeList<int2>(Allocator.TempJob);

            PathFindingJob job = new PathFindingJob
            {
                gridSizeX = grid.gridSizeX,
                gridSizeZ = grid.gridSizeZ,
                nodeSize = grid.nodeSize,
                gridOffset = grid.gridOffset,
                startPos = startNode,
                endPos = endNode,
                nodes = grid.gridNodeArray,
                resultPath = pathResult
            };

            JobHandle handle = job.Schedule();

            StartCoroutine(CompleteJob(handle, pathResult, callback));
        }

        private System.Collections.IEnumerator CompleteJob(JobHandle handle, NativeList<int2> resultPath, System.Action<NativeList<int2>> callback)
        {
            yield return new WaitUntil(() => handle.IsCompleted);
            handle.Complete();

            callback?.Invoke(resultPath);
            resultPath.Dispose();
        }

        private int2 WorldToGrid(AStarGrid grid, Vector3 worldPos)
        {
            Vector3 localPos = worldPos - grid.gridOffset;
            int x = Mathf.FloorToInt(localPos.x / grid.nodeSize);
            int z = Mathf.FloorToInt(localPos.z / grid.nodeSize);
            return new int2(x, z);
        }
    }
}

