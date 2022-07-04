using System.Collections.Generic;
using UnityEngine;

/*
 * This class is used exclusively for rendering and editing the triangle-into-hexagon mesh.
 * Now that HexCell and HexGrid use the new 3d hexagonal prism prefab, this class is not 
 * used and can be archived for potential future use.
 * 
 */

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
    private Mesh hexMesh;
    private MeshCollider meshCollider;
    private List<Vector3> vertices;
    private List<int> triangles;
    private List<Color> colors;

    private void Awake()
    {
        GetComponent<MeshFilter>().mesh = hexMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        hexMesh.name = "Hex Mesh";
        vertices = new List<Vector3>();
        colors = new List<Color>();
        triangles = new List<int>();
    }

    public void Triangulate(HexCell[] cells)
    {
        hexMesh.Clear();
        vertices.Clear();
        colors.Clear();
        triangles.Clear();

        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }
        hexMesh.vertices = vertices.ToArray();
        hexMesh.colors = colors.ToArray();
        hexMesh.triangles = triangles.ToArray();
        hexMesh.RecalculateNormals();
        meshCollider.sharedMesh = hexMesh;
    }

    private void Triangulate (HexCell cell)
    {
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            Triangulate(d, cell);
        }
    }

    private void Triangulate (HexDirection direction, HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        //loop through all 6 triangles of the hexagon
        for (int i = 0; i < 6; i++)
        {
            AddTriangle(center,
                        center + HexMetrics.GetFirstCorner(direction),
                        center + HexMetrics.GetSecondCorner(direction)
            );
            //ch2, used for blending neighbor colors
            //HexCell prevNeighbor = cell.GetNeighbor(direction.Previous()) ?? cell;
            //HexCell neighbor = cell.GetNeighbor(direction) ?? cell;
            //HexCell nextNeighbor = cell.GetNeighbor(direction.Next()) ?? cell;

            //removed color from HexCell.cs
            //AddTriangleColor(cell.color);
        }
        
    }

    private void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int vertexIndex = vertices.Count;
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
    }

    private void AddTriangleColor(Color color)
    {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
    }
}
