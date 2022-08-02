using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{

    //CharacterActions being the input action class generated.
    private CharacterActions playerInput;
    private CharacterController characterController;

    private Vector2 currentMovementInput;
    private Vector3 currentMovement, currentRunMovement;
    private bool isMovementPressed, isRunPressed;
    private float rotationFactorPerFrame = 10.0f;
    private float runSpeed = 4.0f;
    private float walkSpeed = 2.0f;
    private float walkTilt = 7.0f;
    private float runTilt = 14.0f;

    private void Awake()
    {
        playerInput = new CharacterActions();
        characterController = GetComponent<CharacterController>();

        //based on CharacterControls set up earlier in CharacterActions class
        //this will be called when WASD is held. context gives access to input data
        //when started callback occurs.
        playerInput.CharacterControls.Move.started += OnMovementInput;
        playerInput.CharacterControls.Move.canceled += OnMovementInput;
        //Performed is for things like controllers which have more values than 
        //just 0 or 1(think slight tilt of joystick) or even for multi-key movement
        //like W and A on the keyboard to move diagonally
        playerInput.CharacterControls.Move.performed += OnMovementInput;

        playerInput.CharacterControls.Run.started += OnRun;
        playerInput.CharacterControls.Run.canceled += OnRun;
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * walkSpeed;
        //character will move on x/z plane
        currentMovement.z = currentMovementInput.y * walkSpeed;
        currentRunMovement.x = currentMovementInput.x * runSpeed;
        currentRunMovement.z = currentMovementInput.y * runSpeed;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    private void HandleRotation()
    {
        //change in position the player should point to
        //Vector3 playerTilt = transform.rotation.eulerAngles;
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        //current rotation of the player
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            //create new rotation based on where player is currently moving
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            //Spherically interpolates
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);

            
            //have player shift forward slightly as they move, moreso as they run
            if (isMovementPressed && isRunPressed)
            {
                Vector3 playerTilt = transform.rotation.eulerAngles;
                playerTilt.x = runTilt;
                transform.rotation = Quaternion.Euler(playerTilt);
            }
            else if (isMovementPressed && !isRunPressed)
            {
                Vector3 playerTilt = transform.rotation.eulerAngles;
                playerTilt.x = walkTilt;
                transform.rotation = Quaternion.Euler(playerTilt);
            }
        }
        else
        {
            //have player righted back to 0 X
            Vector3 playerTilt = transform.rotation.eulerAngles;
            playerTilt.x = 0.0f;
            transform.rotation = Quaternion.Euler(playerTilt);
        }
    }

    private void HandleGravity()
    {
        //considered "floating" if 0 downward movement, so groundedGravity offsets this
        if (characterController.isGrounded)
        {
            float groundedGravity = -.001f;
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        }
        else
        {
            float gravity = -9.8f;
            currentMovement.y += gravity * Time.deltaTime;
            currentRunMovement.y += gravity * Time.deltaTime;
        }
    }

    private void Update()
    {
        HandleRotation();
        HandleGravity();

        if (isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else
        {
            characterController.Move(currentMovement * Time.deltaTime);
        }
        
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
