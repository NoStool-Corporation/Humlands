using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages the Mesh of a single chunk
/// </summary>
public class MeshData
{   
    public List<Vector3> vertices { get;} = new List<Vector3>(); 
    public List<int> triangles { get; } = new List<int>();
    public List<Vector2> uv = new List<Vector2>();

    public List<Vector3> colVertices { get; } = new List<Vector3>();
    public List<int> colTriangles { get; } = new List<int>();
    /// <summary>
    /// Whether to use the render data for collision or not.
    /// Should only be set at the start of the Blockdata method in the subclasses of Block
    /// </summary>
    public bool useRenderDataForCol;

    /// <summary>
    /// Adds the two newest triangles to from a square
    /// </summary>
    public void AddQuadTriangles()
    {
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);
        if (useRenderDataForCol)
        {
            colTriangles.Add(colVertices.Count - 4);
            colTriangles.Add(colVertices.Count - 3);
            colTriangles.Add(colVertices.Count - 2);
            colTriangles.Add(colVertices.Count - 4);
            colTriangles.Add(colVertices.Count - 2);
            colTriangles.Add(colVertices.Count - 1);
        }
    }
    /// <summary>
    /// Add a new vertex to the vertex array
    /// </summary>
    /// <param name="vertex"></param>
    public void AddVertex(Vector3 vertex)
    {
        vertices.Add(vertex);
        if (useRenderDataForCol)
        {
            colVertices.Add(vertex);
        }
    }

}
