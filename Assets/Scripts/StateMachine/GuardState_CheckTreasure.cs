using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityHFSM;

public class GuardState_CheckTreasure : GuardStateBase
{
    public GuardState_CheckTreasure(bool needsExitTime, Guard_Basic Guard) : base(needsExitTime, Guard)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Checking Treasure");
        Guard.Pathfinding(GameManager.instance.treasureStart);


    }

    public override void OnLogic()
    {
        base.OnLogic();
        Debug.Log("Checking Treasure: OnLogic");

    }
}
