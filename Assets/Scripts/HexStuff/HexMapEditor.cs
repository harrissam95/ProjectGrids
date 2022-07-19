using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.IO;

public class HexMapEditor : MonoBehaviour
{
    public Material[] materials;
    public HexGrid hexGrid;
    public Canvas hexCanvas;
    public int activeElevation;

    private Material activeColor;
    private bool colorToggle;
    private bool elevationToggle = true;
    private bool editToggle = true;

    private void Awake()
    {
        SelectColor(0);
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            OnMouseClick();
        }
    }

    private void OnMouseClick()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(inputRay, out RaycastHit hit))
        {
            HexCell currentCell = hexGrid.GetCell(hit.point);
            if (editToggle)
            {
                EditCell(currentCell);
            }
            else
            {
                hexGrid.FindDistancesTo(currentCell);
            }
        }
    }

    private void EditCell(HexCell cell)
    {
        if (colorToggle)
        {
            cell.GetComponentInChildren<MeshRenderer>().material = activeColor;
        }
        if (elevationToggle)
        {
            cell.Elevation = activeElevation;
        }
    }

    public void SelectColor(int index)
    {
        colorToggle = index >= 0;
        if (colorToggle)
        {
            activeColor = materials[index];
        }
    }

    public void SetElevation(float elevation)
    {
        activeElevation = (int)elevation;
    }

    public void SetElevationToggle(bool toggle)
    {
        elevationToggle = toggle;
    }

    public void SetEditToggle(bool toggle)
    {
        editToggle = toggle;
    }

    public void SetCoordinatesToggle(bool toggle)
    {
        hexCanvas.gameObject.SetActive(toggle);
    }

    public void Save()
    {
        Debug.Log(Application.persistentDataPath);
        // C:/Users/Sam/AppData/LocalLow/SamteriaYT/ProjectGrids
        string path = Path.Combine(Application.persistentDataPath, "test.map");
        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            hexGrid.Save(writer);
        }
    }

    public void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, "test.map");
        using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
        {
            hexGrid.Load(reader);
        }
    }
}
