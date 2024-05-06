using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

public class GuardStateBase : State<GuardState>
{
    protected readonly Guard_Basic Guard;
    protected bool RequestedExit;
    protected float ExitTime;

    protected readonly Action<State<GuardState>> onEnter;
    protected readonly Action<State<GuardState>> onLogic;
    protected readonly Action<State<GuardState>> onExit;
    protected readonly Func<State<GuardState>, bool> canExit;

    public GuardStateBase(bool needsExitTime,
        Guard_Basic Guard,
        float ExitTime = 0.1f,
        Action<State<GuardState>> onEnter = null,
        Action<State<GuardState>> onLogic = null,
        Action<State<GuardState>> onExit = null,
        Func<State<GuardState>, bool> canExit = null)
    {
        this.Guard = Guard;
        this.onEnter = onEnter;
        this.onLogic = onLogic;
        this.onExit = onExit;
        this.canExit = canExit;
        this.ExitTime = ExitTime;
        this.needsExitTime = needsExitTime;

    }

    public override void OnEnter()
    {
        base.OnEnter();
        RequestedExit = false;
        onEnter?.Invoke(this);
    }

    public override void OnLogic()
    {
        base.OnLogic();
        //Debug.Log("Current State:\t" + name);
        if (RequestedExit && timer.Elapsed >= ExitTime)
        {
            fsm.StateCanExit();
        }
    }

    public override void OnExitRequest()
    {
        if (!needsExitTime || canExit != null && canExit(this))
        {
            fsm.StateCanExit();
        }
        else
        {
            RequestedExit = true;
        }
    }
}
