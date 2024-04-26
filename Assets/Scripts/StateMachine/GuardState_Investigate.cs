using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityHFSM;
using static UnityEngine.GraphicsBuffer;

public class GuardState_Investigate : GuardStateBase
{
    private Vector2 position;

    public GuardState_Investigate(bool needsExitTime, Guard_Basic Guard, Vector2 position) : base(needsExitTime, Guard)
    {
        this.position = position;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Investigating");
        Guard.Pathfinding(position);

    }

    public override void OnLogic()
    {
        base.OnLogic();
        Debug.Log("Investigating: OnLogic");


    }
}
