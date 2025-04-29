using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Util.AStar
{
    public class AStarGrid : MonoBehaviour
    {
        public Vector3 center = Vector3.zero;
        public Vector3 size = Vector3.one;
        public float nodeSize = 1.5f;
        public LayerMask obstacleMask = int.MaxValue;

        [ReadOnly, SerializeField] private Node[] gridNodeArray;
        [NonSerialized] public NativeArray<Node> gridNodeNativeArray;
        [NonSerialized] public int3 gridSize;

        private void Awake()
        {
            gridNodeNativeArray = gridNodeArray.ToNativeArray(Allocator.Persistent);
            AStarGridManager.Subscribe(this);
        }

        private void OnDestroy()
        {
            AStarGridManager.Unsubscribe(this);
            if (gridNodeNativeArray.IsCreated)
                gridNodeNativeArray.Dispose();
        }

        public void CalculateGridSize()
        {
            gridSize = new int3(
                Mathf.CeilToInt(size.x / nodeSize),
                0,
                Mathf.CeilToInt(size.z / nodeSize)
            );
        }

        public void CreateGrid()
        {
            gridNodeArray = new Node[gridSize.x * gridSize.z];
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int z = 0; z < gridSize.z; z++)
                {
                    Vector3 worldPos = GridToWorld(x, z);
                    bool walkable = !Physics.CheckSphere(worldPos, nodeSize * 0.5f, obstacleMask);
                    Node node = new Node
                    {
                        position = new int2(x, z),
                        walkable = walkable,
                        gCost = float.MaxValue,
                        hCost = 0f,
                        parent = new int2(-1, -1)
                    };
                    gridNodeArray[GetIndex(x, z)] = node;
                }
            }
        }

        public int GetIndex(int x, int z)
        {
            return x + gridSize.x * z;
        }

        public Vector3 GridToWorld(int2 gridPos) => GridToWorld(gridPos.x, gridPos.y);

        public Vector3 GridToWorld(int x, int z)
        {
            Vector3 start = center - size * 0.5f;
            return new Vector3(x * nodeSize, 0, z * nodeSize) + start;
        }

        
        public int2 WorldToGrid(Vector3 worldPos)
        {
            Vector3 start = center - size * 0.5f;
            Vector3 localPos = worldPos - start - new Vector3(nodeSize * 0.5f, 0, nodeSize * 0.5f);
            int x = Mathf.RoundToInt(localPos.x / nodeSize);
            int z = Mathf.RoundToInt(localPos.z / nodeSize);
            return new int2(x, z);
        }

        public bool IsPointInGrid(Vector3 point)
        {
            int2 gridPos = WorldToGrid(point);
            return gridPos.x >= 0 && gridPos.x < gridSize.x && gridPos.y >= 0 && gridPos.y < gridSize.z;
        }
    }
} 