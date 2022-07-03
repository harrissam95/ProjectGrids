using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HexMapEditor : MonoBehaviour
{
    public Color[] colors;
    public Material[] materials;
    public HexGrid hexGrid;
    public int activeElevation;

    private Material activeColor;

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
            EditCell(hexGrid.GetCell(hit.point));
        }
    }

    public void EditCell(HexCell cell)
    {
        cell.GetComponentInChildren<MeshRenderer>().material = activeColor;
        cell.Elevation = activeElevation;
    }

    public void SelectColor(int index)
    {
        activeColor = materials[index];
    }

    public void SetElevation(float elevation)
    {
        activeElevation = (int)elevation;
    }

}
