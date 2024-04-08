using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Guard_Basic : Character_Base
{
    public Vector2 StartNodePosition;
    public Vector2 GoalNodePosition;
    private List<GridNode> path = new List<GridNode>();

    private Vector3 aimDir;
    private Vector3 prevAimDir;
    private Vector3 tempAimDir;

    private bool NewPosFound;
    private bool CanMove;

    [SerializeField]
    public FlashLight flashLight;

    private Vector3 agentPos;
    private GameObject previousObj;

    private float distanceTime;
    private float dist;
    private float rotDist;
    private float fraction;

    public event Action OnDoneMoving;

    private void Awake()
    {
        
    }

    private void Start()
    {
        //currentObj = grid.SetCurrentNode((int)StartNodePosition.x, (int)StartNodePosition.y);
        //transform.position = currentObj.transform.position;
        //currentNode = currentObj.GetComponent<GridNode>();
        //currentNode.currentCharacter = this.gameObject;
        
        
    }

    override public void Init(int x = 0, int y = 0)
    {         
        base.Init((int)StartNodePosition.x, (int)StartNodePosition.y);

        aimDir = Vector3.up;
        flashLight.InitLight();
        flashLight.SetAimDirection(aimDir);
        flashLight.SetOrigin(transform.position);
        Pathfinding();
        speed = 2;
    }
    
    public void MoveAgent()
    {
        if(path.Count == 0)
            return;

        previousObj = currentObj;
        currentObj = path[0].gameObject;

        prevAimDir = aimDir;
        tempAimDir = aimDir;
        aimDir = currentObj.transform.position - transform.position;
        rotDist = Vector3.Distance(aimDir, prevAimDir);
        agentPos = transform.position;
        dist = Vector3.Distance(agentPos, currentObj.transform.position);
        fraction = 0;
        NewPosFound = true;
        CanMove = false;

    }

    /// <summary>
    /// Rotates the Guard towards the new movement direction before moving it
    /// </summary>
    /// <param name="prevAim"></param>
    /// <param name="currentAim"></param>
    public void RotateGuard()
    {
        if (Vector3.Distance(tempAimDir, aimDir) > 0.1)
        {
            prevAimDir.Normalize();
            tempAimDir.Normalize();
            aimDir.Normalize();
            float currentDist = speed * Time.deltaTime;

            fraction += (currentDist * rotDist) / rotDist;
            Mathf.Clamp(fraction, 0, 1);
            tempAimDir = Vector3.Lerp(prevAimDir, aimDir, fraction).normalized;
            flashLight.SetAimDirection(tempAimDir);
            //flashLight.UpdateLight();
        }
        else
        {
            currentNode.ClearCharacter();
            fraction = 0;
            flashLight.SetAimDirection(aimDir);
            CanMove = true;
        }

    }

    /// <summary>
    /// Logic for moving the Guard between it's previous point and it's current point
    /// </summary>
    public void MoveGuard()
    {
        if (Vector3.Distance(transform.position, currentObj.transform.position) > 0.1)
        {
            float currentDist = speed * Time.deltaTime;

            fraction += (currentDist * dist) / dist;
            Mathf.Clamp(fraction, 0, 1);
            transform.position = Vector3.Lerp(agentPos, currentObj.transform.position, fraction);
            flashLight.SetOrigin(transform.position);
            //flashLight.UpdateLight();

        }
        else
        {
            transform.position = currentObj.transform.position;
            currentNode = currentObj.GetComponent<GridNode>();
            path.RemoveAt(0);
            NewPosFound = false;
            CanMove = false;
            OnDoneMoving();
        }
    }

    private void FixedUpdate()
    {
        if(NewPosFound)
        {
            if(CanMove)
            {
                MoveGuard();
            }
            else
            {
                RotateGuard();
            }


        }
    }

    public void Pathfinding()
    {
        int x1 = (int)StartNodePosition.x;
        int y1 = (int)StartNodePosition.y;
        int x2 = (int)GoalNodePosition.x;
        int y2 = (int)GoalNodePosition.y;

        path = AStar.Search(grid,
            grid.grid[x1, y1].GetComponent<GridNode>(),
            grid.grid[x2, y2].GetComponent<GridNode>());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(new Vector2((StartNodePosition.x * 2) - (grid.width / 2) - 2, (StartNodePosition.y * 2) - (grid.height / 2) - 2), 0.2f);

        foreach (GridNode n in path)
        {
            // The Goal node is RED
            if (n.Position == GoalNodePosition)
            {
                Gizmos.color = Color.red;
            }
            // Every other node in the path is YELLOW
            else
            {
                Gizmos.color = Color.yellow;
            }
            Vector2 pos = new Vector2((n.Position.x * 2) - (grid.width / 2) - 2, (n.Position.y * 2) - (grid.height / 2) - 2);
            Gizmos.DrawSphere(pos, 0.2f);
        }
    }

}
