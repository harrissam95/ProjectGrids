using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class HexGrid : MonoBehaviour
{
    public HexCell cellPrefab;
    public Text cellLabelPrefab;
    public int width = 20;
    public int height = 10;
    public float offset;

    public Material defaultMaterial;

    private HexCell[] cells;
    private Canvas gridCanvas;
    //private HexMesh hexMesh;

    private void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        //hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[height * width];

        for (int z = 0, i = 0; z < height; z++)
        {
            for(int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        Debug.Log("Index: " + index);
        return cells[index];
    }

    private void CreateCell (int x, int z, int i)
    {
        Vector3 position;
        position.x = ((x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f)) * offset;
        position.y = 0f;
        position.z = (z * (HexMetrics.outerRadius * 1.5f)) * offset;

        HexCell cell = cells[i] = Instantiate(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.GetComponentInChildren<MeshRenderer>().material = defaultMaterial;

        if(x > 0)
        {
            //connect E - W
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if(z > 0)
        {
            if((z & 1) == 0)
            {
                //connect NW - SE
                cell.SetNeighbor(HexDirection.SE, cells[i - width]);
                if(x > 0)
                {
                    //connect NE - SW
                    cell.SetNeighbor(HexDirection.SW, cells[i - width - 1]);
                }
            }
            else
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - width]);
                if(x < width - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - width + 1]);
                }
            }
        }

        Text label = Instantiate(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        //label.text = cell.coordinates.ToStringOnSeparateLines();
        cell.uiRect = label.rectTransform;
    }

    public void FindDistancesTo (HexCell cell)
    {
        StopAllCoroutines();
        StartCoroutine(Search(cell));
    }

    IEnumerator Search(HexCell cell)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance = int.MaxValue;
        }

        WaitForSeconds delay = new WaitForSeconds(1 / 60f);
        Queue<HexCell> frontier = new Queue<HexCell>();
        cell.Distance = 0;
        frontier.Enqueue(cell);
        while (frontier.Count > 0)
        {
            yield return delay;
            HexCell current = frontier.Dequeue();
            for(HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = current.GetNeighbor(d);
                if (neighbor == null || neighbor.Distance != int.MaxValue)
                {
                    continue;
                }
                if(current.GetEdgeType(neighbor) == "cliff")
                {
                    continue;
                }
                neighbor.Distance = current.Distance + 1;
                frontier.Enqueue(neighbor);
            }
        }
    }

    public void Save(BinaryWriter writer)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Save(writer);
        }
    }

    public void Load(BinaryReader reader)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Load(reader);
        }
    }
}
