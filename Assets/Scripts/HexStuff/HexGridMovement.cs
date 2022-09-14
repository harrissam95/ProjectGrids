using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class HexGridMovement : MonoBehaviour
{

    public HexGrid hexGrid;
    //Using this for movement on a unit within the combat hex grid
    private HexControls playerInput;
    private HexCell searchFromCell;
    private bool isLeftShiftPressed;
    private bool isLeftCtrlPressed;

    private void Awake()
    {
        playerInput = new HexControls();
        playerInput.AllControls.LeftShift.started += OnLeftShift;
        playerInput.AllControls.LeftShift.canceled += OnLeftShift;

        playerInput.AllControls.LeftCtrl.started += OnLeftCtrl;
        playerInput.AllControls.LeftCtrl.canceled += OnLeftCtrl;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject())
        {
            OnMouseClick();
        }
    }

    private void OnMouseClick()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(inputRay, out RaycastHit hit))
        {
            HexCell clickedCell = hexGrid.GetCell(hit.point);
            if (isLeftShiftPressed)
            {
                if (searchFromCell)
                {
                    searchFromCell.DisableHighlight();
                }
                searchFromCell = clickedCell;
                searchFromCell.EnableHighlight(Color.blue);
                hexGrid.FindDistancesTo(clickedCell);
            }
            else if (isLeftCtrlPressed)
            {
                hexGrid.SpawnEnemy(clickedCell);
            }
        }
    }

    private void OnLeftShift(InputAction.CallbackContext context)
    {
        isLeftShiftPressed = context.ReadValueAsButton();
    }

    private void OnLeftCtrl(InputAction.CallbackContext context)
    {
        isLeftCtrlPressed = context.ReadValueAsButton();
    }

    private void OnEnable()
    {
        playerInput.AllControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.AllControls.Disable();
    }

}
