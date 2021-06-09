using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public Vector3Int gridSize = new Vector3Int(20,0, 20);
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }
    private void CreateShape()
    {
        vertices = new Vector3[(gridSize.x + 1) * (gridSize.z + 1)];

        for(int i = 0, z = 0; z < gridSize.z; z++)
        {
            for(int x = 0;x <= gridSize.x; x++)
            {
                float y = Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * 2f;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }
        triangles = new int[gridSize.x * gridSize.z * 6];

        int vert = 0, tris = 0;
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int z = 0; z < gridSize.z; z++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + gridSize.x + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + gridSize.x + 1;
                triangles[tris + 5] = vert + gridSize.x + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }
    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
