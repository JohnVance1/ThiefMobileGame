using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

public class GuardState_SearchForCulprit : GuardStateBase
{
    public GuardState_SearchForCulprit(bool needsExitTime, Guard_Basic Guard) : base(needsExitTime, Guard)
    {

    }
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Searching For Culprit");

    }
}
