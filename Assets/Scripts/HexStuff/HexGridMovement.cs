using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class HexGridMovement : MonoBehaviour
{

    public HexGrid hexGrid;
    //Using this for movement on a unit within the combat hex grid
    private CharacterActions playerInput;
    private HexCell searchFromCell;
    private bool isShiftPressed;

    private void Awake()
    {
        playerInput = new CharacterActions();
        playerInput.CharacterControls.Run.started += OnShift;
        playerInput.CharacterControls.Run.canceled += OnShift;
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
            HexCell currentCell = hexGrid.GetCell(hit.point);
            if (isShiftPressed)
            {
                if (searchFromCell)
                {
                    searchFromCell.DisableHighlight();
                }
                searchFromCell = currentCell;
                searchFromCell.EnableHighlight(Color.blue);
                hexGrid.FindDistancesTo(currentCell);
            }
        }
    }

    private void OnShift(InputAction.CallbackContext context)
    {
        isShiftPressed = context.ReadValueAsButton();
    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }

}
