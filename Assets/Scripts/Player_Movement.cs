using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.Users;
public class Player_Movement : Character_Base
{
    
    public PlayerControls input;
    private InputAction move;

    private bool Moved;

    private void Start()
    {
        currentObj = grid.SetCurrentNode(2, 2);
        currentNode = currentObj.GetComponent<GridNode>();
    }

    private void Awake()
    {
        input = new PlayerControls();
        Moved = false;
    }   

    private void OnEnable()
    {
        move = input.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();

    }

    private void FixedUpdate()
    {
        Vector2 movement = move.ReadValue<Vector2>();
        //horizontal = Input.GetAxisRaw("Horizontal");

        //vertical = Input.GetAxisRaw("Vertical");

        if(movement != Vector2.zero && Moved == false)
        {
            GameObject nextPos = grid.GetNextNode(movement, currentNode);

            if (nextPos != null)
            {
                transform.position = nextPos.transform.position;
                currentObj = nextPos.gameObject;
                currentNode.ClearCharacter();
                currentNode = nextPos.GetComponent<GridNode>();
                currentNode.ClearCharacter();
                Moved = true;
            }
        }
        else if(movement == Vector2.zero)
        {
            Moved = false;

        }








        //rb.velocity = movement.normalized * newSpeed;



    }

}
