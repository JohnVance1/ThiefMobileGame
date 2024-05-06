using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityHFSM;

public class GuardState_SearchForCulprit : GuardStateBase
{
    Vector2 position;
    GridControls controls;
    int height;
    int width;


    public GuardState_SearchForCulprit(bool needsExitTime, Guard_Basic Guard, Vector2 position, GridControls controls)
        : base(needsExitTime, Guard)
    {
        this.position = position;
        this.controls = controls;
        height = 3;
        width = 3;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Searching For Culprit");

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
