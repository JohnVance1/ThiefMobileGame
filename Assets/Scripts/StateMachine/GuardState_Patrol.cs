using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityHFSM;

public class GuardState_Patrol : GuardStateBase
{
    Vector2 position;
    Vector2 aimDirection;
    GridControls controls;
    int height;
    int width;

    public GuardState_Patrol(bool needsExitTime, Guard_Basic Guard, Vector2 position, Vector2 aimDirection, GridControls controls) 
        : base(needsExitTime, Guard)
    {
        this.position = position;
        this.aimDirection = aimDirection;
        this.controls = controls;
        height = 3; 
        width = 3;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Patrolling");

        GetRandomLocAroundStartNode();
    }

    public override void OnLogic()
    {
        base.OnLogic();
        Debug.Log("Patrolling: OnLogic");

    }

    public void ChangeCenterPosition(Vector2 center)
    {
        this.position = center;
    }

    public void GetRandomLocAroundStartNode(Vector2 aim)
    {
        Vector2 pathfindingLoc;
        Vector2 aimLoc;

        int x;
        int y;
        this.aimDirection = aim;

        do
        {
            x = Random.Range(-width, width);
            y = Random.Range(-height, height);

            pathfindingLoc = new Vector2(x + position.x, y + position.y);

            aimLoc = new Vector2(x, y);

        } while (((controls.IsNodeWall(pathfindingLoc) ||
        controls.IsNodeOccupied(pathfindingLoc) ||
        !controls.IsInRange(pathfindingLoc)) || 
        pathfindingLoc == controls.treasureNode /*||
        Vector2.Dot(aimDirection, aimLoc) >= 0*/));

        Guard.Pathfinding(pathfindingLoc);

    }

    public void GetRandomLocAroundStartNode()
    {
        Vector2 pathfindingLoc;
        int x;
        int y;

        do
        {
            x = Random.Range(-width, width);
            y = Random.Range(-height, height);

            pathfindingLoc = new Vector2(x + position.x, y + position.y);
                        

        } while (((controls.IsNodeWall(pathfindingLoc) ||
        controls.IsNodeOccupied(pathfindingLoc) ||
        !controls.IsInRange(pathfindingLoc)) ||
        pathfindingLoc == controls.treasureNode));

        Guard.Pathfinding(pathfindingLoc);

    }
}
