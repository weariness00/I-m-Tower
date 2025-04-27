using System.Numerics;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Util.AStar
{
    [BurstCompile]
    public struct PathFindingJob : IJob
    {
        public int3 gridSize;
        public float nodeSize;
        public float3 gridOffset;

        public int2 startPos;
        public int2 endPos;

        public NativeArray<Node> nodes;
        public NativeList<int2> resultPath;

        public NativeArray<bool> allowedDirection;

        public void Execute()
        {
            NativeList<int2> openSet = new NativeList<int2>(Allocator.Temp);
            NativeHashSet<int2> closedSet = new NativeHashSet<int2>(0, Allocator.Temp);

            int startIndex = GetIndex(startPos.x, startPos.y);
            Node startNode = nodes[startIndex];
            startNode.gCost = 0;
            nodes[startIndex] = startNode;

            openSet.Add(startPos);

            while (openSet.Length > 0)
            {
                int2 current = openSet[0];
                for (int i = 1; i < openSet.Length; i++)
                {
                    if (nodes[GetIndex(openSet[i].x, openSet[i].y)].FCost < nodes[GetIndex(current.x, current.y)].FCost)
                    {
                        current = openSet[i];
                    }
                }

                if (current.Equals(endPos))
                {
                    RetracePath(startPos, endPos);
                    break;
                }

                openSet.RemoveAtSwapBack(openSet.IndexOf(current));
                closedSet.Add(current);

                foreach (var neighbor in GetNeighbors(current))
                {
                    if (!IsWalkable(neighbor) || closedSet.Contains(neighbor))
                        continue;

                    float tentativeGCost = nodes[GetIndex(current.x, current.y)].gCost + math.distance(current, neighbor);
                    if (tentativeGCost < nodes[GetIndex(neighbor.x, neighbor.y)].gCost)
                    {
                        Node neighborNode = nodes[GetIndex(neighbor.x, neighbor.y)];
                        neighborNode.gCost = tentativeGCost;
                        neighborNode.hCost = math.distance(neighbor, endPos);
                        neighborNode.parent = current;
                        nodes[GetIndex(neighbor.x, neighbor.y)] = neighborNode;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            openSet.Dispose();
            closedSet.Dispose();
        }

        void RetracePath(int2 start, int2 end)
        {
            NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
            int2 current = end;

            while (!current.Equals(start))
            {
                path.Add(current);
                current = nodes[GetIndex(current.x, current.y)].parent;
            }

            for (int i = path.Length - 1; i >= 0; i--)
            {
                resultPath.Add(path[i]);
            }

            path.Dispose();
        }

        bool IsWalkable(int2 pos)
        {
            if (pos.x < 0 || pos.x >= gridSize.x || pos.y < 0 || pos.y >= gridSize.z)
                return false;
            return nodes[GetIndex(pos.x, pos.y)].walkable;
        }

        int GetIndex(int x, int z)
        {
            return x + gridSize.x * z;
        }

        NativeList<int2> GetNeighbors(int2 pos)
        {
            NativeList<int2> neighbors = new NativeList<int2>(Allocator.Temp);

            int2[] dirs = new int2[]
            {
                new int2(-1, 1),   new int2(0, 1),   new int2(1, 1),
                new int2(-1, 0),                         new int2(1, 0),
                new int2(-1, -1),  new int2(0, -1),  new int2(1, -1)
            };

            for (int i = 0; i < dirs.Length; i++)
            {
                if (!allowedDirection[i]) continue;
                int2 neighborPos = pos + dirs[i];
                if (IsWalkable(neighborPos))
                    neighbors.Add(neighborPos);
            }

            return neighbors;
        }
    }
}