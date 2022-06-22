using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{

    //CharacterActions being the input action class generated.
    CharacterActions playerInput;
    CharacterController characterController;

    Vector2 currentMovementInput;
    Vector3 currentMovement, currentRunMovement;
    bool isMovementPressed, isRunPressed;
    float rotationFactorPerFrame = 10.0f;
    float runSpeed = 4.0f;
    float walkSpeed = 2.0f;
    float walkTilt = 7.0f;
    float runTilt = 14.0f;

    // Awake is called earlier than Start
    //great for initially setting reference variables.
    private void Awake()
    {
        playerInput = new CharacterActions();
        characterController = GetComponent<CharacterController>();

        //based on CharacterControls set up earlier in CharacterActions class
        //this will be called when WASD is held. context gives access to input data
        //when started callback occurs.
        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        //Performed is for things like controllers which have more values than 
        //just 0 or 1(think slight tilt of joystick) or even for multi-key movement
        //like W and A on the keyboard to move diagonally
        playerInput.CharacterControls.Move.performed += onMovementInput;

        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
    }//end Awake

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * walkSpeed;
        //character will move on x/z plane
        currentMovement.z = currentMovementInput.y * walkSpeed;
        currentRunMovement.x = currentMovementInput.x * runSpeed;
        currentRunMovement.z = currentMovementInput.y * runSpeed;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }//end onMovementInput

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }//end onRun

    void handleRotation()
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
    }//end handleRotation

    void handleGravity()
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
    }//end handleGravity

    // Update is called once per frame
    void Update()
    {
        handleRotation();
        handleGravity();

        if (isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else
        {
            characterController.Move(currentMovement * Time.deltaTime);
        }
        
    }//end Update

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
