using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    public RectTransform uiRect;
    public int elevation;

    [SerializeField] private HexCell[] neighbors;
    [SerializeField] private bool walled;
    [SerializeField] private int distance;

    public int Elevation
    {
        get
        {
            return elevation;
        }
        set
        {
            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * HexMetrics.elevationStep;
            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z = elevation * -HexMetrics.elevationStep;
            uiRect.localPosition = uiPosition;
        }
    }

    public bool Walled
    {
        get
        {
            return walled;
        }
        set
        {
            if(walled != value)
            {
                walled = value;
            }
        }
    }

    public int Distance
    {
        get
        {
            return distance;
        }
        set
        {
            distance = value;
            UpdateDistanceLabel();
        }
    }

    public HexCell PathFrom
    {
        get;
        set;
    }

    public Vector3 Position
    {
        get
        {
            return transform.localPosition;
        }
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(elevation);
    }

    public void Load(BinaryReader reader)
    {
        elevation = reader.ReadInt32();
    }

    public string GetEdgeType(HexDirection direction)
    {
        return HexMetrics.GetEdgeType(elevation, neighbors[(int)direction].elevation);
    }

    public string GetEdgeType(HexCell otherCell)
    {
        return HexMetrics.GetEdgeType(elevation, otherCell.elevation);
    }

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor (HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public void DisableHighlight()
    {
        Image highlight = uiRect.GetChild(0).GetComponent<Image>();
        highlight.enabled = false;
    }

    public void EnableHighlight(Color color)
    {
        Image highlight = uiRect.GetChild(0).GetComponent<Image>();
        highlight.color = color;
        highlight.enabled = true;
    }

    private void UpdateDistanceLabel()
    {
        Text label = uiRect.GetComponent<Text>();
        //label.text = distance.ToString();
        label.text = distance == int.MaxValue ? "" : distance.ToString();
    }
}
