using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.Tilemaps;

public class GridMovement : MonoBehaviour
{
    //public Tilemap map;
    MouseMovement mouseMovement;
    CharacterController characterController;
    private Vector3 destination;
    private Vector3 mousePosition;
    [SerializeField] private float speed;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mouseMovement = new MouseMovement();
    }//end Awake

    private void Start()
    {
        //destination = transform.position;
        mouseMovement.Mouse.MouseLeftClick.performed += OnMouseClick;

    }//end Start

    private void Update()
    {
        //moveCharacter();
    }//end Update

    private void OnMouseClick(InputAction.CallbackContext context)
    {
        mousePosition = mouseMovement.Mouse.MousePosition.ReadValue<Vector3>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Debug.Log("Mouse Position X: " + mousePosition.x);
        Debug.Log("Mouse Position Y: " + mousePosition.y);
        //Debug.Log("Mouse Position Z: " + mousePosition.z);
        destination.x = mousePosition.x;
        destination.z = mousePosition.y;

        MoveCharacter();
        //Vector3Int gridPos = map.WorldToCell(mousePosition);
        ////make sure we are clicking the cell
        //if (map.HasTile(gridPos))
        //{
        //    destination = mousePosition;
        //}

    }//end onMouseClick

    private void MoveCharacter()
    {
        //if(Vector3.Distance(transform.position, destination) > 0.1f)
        //{
            //transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            characterController.Move(destination * Time.deltaTime);
        //}
    }//end movecharacter

    private void OnEnable()
    {
        mouseMovement.Enable();
    }//end onEnable

    private void OnDisable()
    {
        mouseMovement.Disable();
    }//end onDisable




    //Vector3 up = Vector3.zero,
    //right = new Vector3(0, 90, 0),
    //down = new Vector3(0, 180, 0),
    //left = new Vector3(0, 270, 0),
    //currentdirection = Vector3.zero;

    //Vector3 nextPos, destination, direction;
    //float speed = 3.0f;

    //private void Start()
    //{
    //    currentdirection = up;
    //    nextPos = Vector3.forward;
    //    destination = transform.position;
    //}

    //private void Update()
    //{
    //    Move();
    //}

    //void Move()
    //{
    //    transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

    //    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
    //    {
    //        nextPos = Vector3.forward;
    //    }
    //}
}
