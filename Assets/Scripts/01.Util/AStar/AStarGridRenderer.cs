using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Util.AStar
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class AStarGridRenderer : MonoBehaviour
    {
        private static readonly string path = "Assets/AStar"; 
        public AStarGrid targetGrid;
        public Vector3 offset;

        [NonSerialized] private MeshRenderer meshRenderer;
        [NonSerialized] private MeshFilter meshFilter;
        public Material lineMaterial;
        private GameObject wireObject;

        public void OnEnable()
        {
            if (targetGrid == null)
            {
                Debug.LogWarning("Target Grid가 지정되지 않아 AStar의 Grid를 시각화 하지 못했습니다.");
                gameObject.SetActive(false);
            }
        }

        public void CreateMesh()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
            CreateGridMesh();
            CreateWireMesh();
        }

        private void CreateGridMesh()
        {
            Mesh gridMesh = new Mesh();
            var halfSize = targetGrid.nodeSize / 2;
            var point1 = new Vector3(-halfSize,  0, halfSize) + offset;
            var point2 = new Vector3( halfSize,  0, halfSize) + offset;
            var point3 = new Vector3(-halfSize,  0,-halfSize) + offset;
            var point4 = new Vector3( halfSize,  0,-halfSize) + offset;
            List<int> triangles = new();
            List<Vector3> vertices = new();
            List<Vector3> normals = new();
            foreach (var node in targetGrid.gridNodeArray)
            {
                var position = targetGrid.GridToWorld(node.position);
                vertices.Add(position + point1);
                vertices.Add(position + point2);
                vertices.Add(position + point3);
                vertices.Add(position + point4);
                
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);
                normals.Add(Vector3.up);

                var vertexIndex = vertices.Count - 4;
                triangles.Add(vertexIndex + 0);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 2);
                
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 3);
                triangles.Add(vertexIndex + 2);
            }

            gridMesh.name = "A Star Grid Mesh";
            gridMesh.SetVertices(vertices);
            gridMesh.SetNormals(normals);
            gridMesh.SetTriangles(triangles, 0);
            gridMesh.RecalculateBounds();

            meshFilter.mesh = gridMesh;
            
            SaveMesh(gridMesh);
        }

        private void CreateWireMesh()
        {
            Mesh wireMesh = new Mesh();
            List<Vector3> lineVertices = new();
            List<int> lineIndices = new();

            var halfSize = targetGrid.nodeSize / 2;
            var point1 = new Vector3(-halfSize,  0, halfSize) + offset;
            var point2 = new Vector3( halfSize,  0, halfSize) + offset;
            var point3 = new Vector3(-halfSize,  0,-halfSize) + offset;
            var point4 = new Vector3( halfSize,  0,-halfSize) + offset;
            foreach (var node in targetGrid.gridNodeArray)
            {
                var position = targetGrid.GridToWorld(node.position);

                Vector3 p1 = position + point1;
                Vector3 p2 = position + point2;
                Vector3 p3 = position + point3;
                Vector3 p4 = position + point4;

                lineVertices.Add(p1); lineVertices.Add(p2);
                lineVertices.Add(p1); lineVertices.Add(p3);
                lineVertices.Add(p4); lineVertices.Add(p2);
                lineVertices.Add(p4); lineVertices.Add(p3);

                for (int i = 0; i < 8; i++) lineIndices.Add(lineIndices.Count);
            }

            wireMesh.name = "Wire Mesh";
            wireMesh.SetVertices(lineVertices);
            wireMesh.SetIndices(lineIndices, MeshTopology.Lines, 0);

            if (wireObject == null)
            {
                wireObject = new GameObject("GridLines", typeof(MeshFilter), typeof(MeshRenderer));
                wireObject.transform.SetParent(transform, false);
            }
            wireObject.GetComponent<MeshFilter>().mesh = wireMesh;
            wireObject.GetComponent<MeshRenderer>().material = lineMaterial;
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            SaveMesh(wireMesh);
        }

        private void SaveMesh(Mesh mesh)
        {
            // 이미 있는 경우 → 덮어쓰기
            mesh.name = $"{name} {mesh.name}.asset";
            var file = Path.Combine(path, mesh.name);
            if (File.Exists(file))
            {
                Debug.Log($"Overwriting existing mesh asset at {file}");
                Mesh existingMesh = AssetDatabase.LoadAssetAtPath<Mesh>(file);

                if (existingMesh != null)
                {
                    EditorUtility.CopySerialized(mesh, existingMesh); // 기존 에셋에 덮어쓰기
                    EditorUtility.SetDirty(existingMesh);
                }
                else
                {
                    Debug.LogWarning("File exists but could not load as Mesh. Re-creating asset.");
                    AssetDatabase.CreateAsset(mesh, file);
                }
            }
            else
            {
                Debug.Log($"Creating new mesh asset at {file}");
                AssetDatabase.CreateAsset(mesh, file);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}