using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Util.AStar
{
    public class AStarGrid : MonoBehaviour
    {
        public int gridSizeX = 20;
        public int gridSizeZ = 20;
        public float nodeSize = 1.5f;
        public Vector3 gridOffset = Vector3.zero;
        public LayerMask obstacleMask;

        public NativeArray<Node> gridNodeArray;

        private void Awake()
        {
            CreateGrid();
        }

        private void OnDestroy()
        {
            if (gridNodeArray.IsCreated)
                gridNodeArray.Dispose();
        }

        void CreateGrid()
        {
            gridNodeArray = new NativeArray<Node>(gridSizeX * gridSizeZ, Allocator.Persistent);
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 worldPos = GridToWorld(x, z) + new Vector3(nodeSize, 0, nodeSize) * 0.5f;
                    bool walkable = !Physics.CheckSphere(worldPos, nodeSize * 0.4f, obstacleMask);
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
            return x + gridSizeX * z;
        }

        public Vector3 GridToWorld(int x, int z)
        {
            return new Vector3(x * nodeSize, 0, z * nodeSize) + gridOffset;
        }

        public bool IsPointInGrid(Vector2 point)
        {
            Vector3 point3 = new Vector3(point.x, 0, point.y);
            Vector3 localPos = point3 - gridOffset;
            int x = Mathf.FloorToInt(localPos.x / nodeSize);
            int z = Mathf.FloorToInt(localPos.z / nodeSize);
            return x >= 0 && x < gridSizeX && z >= 0 && z < gridSizeZ;
        }
    }
}

