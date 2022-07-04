using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HexMapEditor : MonoBehaviour
{
    public Material[] materials;
    public HexGrid hexGrid;
    public Canvas hexCanvas;
    public int activeElevation;

    private Material activeColor;
    private bool applyColor;
    private bool applyElevation = true;

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

    private void EditCell(HexCell cell)
    {
        if (applyColor)
        {
            cell.GetComponentInChildren<MeshRenderer>().material = activeColor;
        }
        if (applyElevation)
        {
            cell.Elevation = activeElevation;
        }
    }

    public void SelectColor(int index)
    {
        applyColor = index >= 0;
        if (applyColor)
        {
            activeColor = materials[index];
        }
    }

    public void SetElevation(float elevation)
    {
        activeElevation = (int)elevation;
    }

    public void SetApplyElevation(bool toggle)
    {
        applyElevation = toggle;
    }

    public void SetShowCoordinates(bool toggle)
    {
        hexCanvas.gameObject.SetActive(toggle);
    }
}
