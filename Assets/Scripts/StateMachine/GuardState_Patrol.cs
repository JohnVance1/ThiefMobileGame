using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

public class GuardState_Patrol : GuardStateBase
{
    public GuardState_Patrol(bool needsExitTime, Guard_Basic Guard) : 
        base(needsExitTime, Guard)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Patrolling");

    }

    public override void OnLogic()
    {
        base.OnLogic();
    }
}
