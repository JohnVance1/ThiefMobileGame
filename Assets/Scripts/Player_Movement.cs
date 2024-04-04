using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.Users;
using System.IO;
using Unity.VisualScripting;
using System;

public class Player_Movement : Character_Base
{
    
    public PlayerControls input;
    private InputAction move;
    private GameObject nextPos;

    private float distanceTime;
    private float dist;
    private float rotDist;
    private float fraction;
    private Vector3 agentPos;

    public event Action OnPlayerMoved;

    public bool Moved { get; private set; }
    public bool IsMoving { get; set; }
    public bool CanMove { get; set; }


    private void Start()
    {
        currentObj = grid.SetCurrentNode(2, 2);
        currentNode = currentObj.GetComponent<GridNode>();
        speed = 5;
    }

    private void Awake()
    {
        input = new PlayerControls();
        Moved = false;
        CanMove = true;
        IsMoving = false;
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
        if(CanMove)
        {
            Moved = false;
            if (movement != Vector2.zero)
            {
                nextPos = grid.GetNextNode(movement, currentNode);

                if (nextPos != null)
                {
                    CanMove = false;
                    currentObj = nextPos.gameObject;
                    agentPos = transform.position;
                    dist = Vector3.Distance(agentPos, currentObj.transform.position);
                    fraction = 0;
                    currentNode.ClearCharacter();
                    IsMoving = true;
                }
            }
            
        }
        if (IsMoving == true)
        {
            MovePlayer();

        }


    }

    public void MovePlayer()
    {
        if (Vector3.Distance(transform.position, currentObj.transform.position) > 0.1)
        {
            float currentDist = speed * Time.deltaTime;

            fraction += (currentDist * dist) / dist;
            Mathf.Clamp(fraction, 0, 1);
            transform.position = Vector3.Lerp(agentPos, currentObj.transform.position, fraction);


        }
        else
        {
            transform.position = currentObj.transform.position;
            currentNode = currentObj.GetComponent<GridNode>();
            currentNode.SetCurrentCharacter(gameObject);
            Moved = true;
            IsMoving = false;
            OnPlayerMoved();
            //CanMove = true;
        }



    }

}
