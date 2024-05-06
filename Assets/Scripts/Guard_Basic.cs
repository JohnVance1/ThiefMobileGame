using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityHFSM;

public enum GuardState
{
    Patrolling,
    GuardTreasure,
    SearchForCulprit,
    InvestigateNoise


}

public enum StateEvent
{
    TreasureSafe,
    NoiseInView,
    TreasureMissing

}



public class Guard_Basic : Character_Base
{
    public Vector2 StartNodePosition;
    public Vector2 GoalNodePosition;
    private List<GridNode> path = new List<GridNode>();

    private StateMachine<GuardState, StateEvent> fsm;

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
    public bool AtEndOfPath {  get; private set; }
    public List<Collider2D> collidersInLight;
    public bool DetectedPlayer { get; private set; }


    private void OnEnable()
    {
        flashLight.onColliderHit += DetectInLight;
    }

    private void OnDisable()
    {
        flashLight.onColliderHit -= DetectInLight;

    }


    override public void Init(int x = 0, int y = 0)
    {         
        base.Init((int)StartNodePosition.x, (int)StartNodePosition.y);
        fsm = new StateMachine<GuardState, StateEvent>();

        GameManager.instance.NoiseAt();

        fsm.AddState(GuardState.Patrolling, new GuardState_Patrol(false, this, StartNodePosition, aimDir, grid));
        fsm.AddState(GuardState.InvestigateNoise, new GuardState_Investigate(false, this, GoalNodePosition));
        fsm.AddState(GuardState.SearchForCulprit, new GuardState_SearchForCulprit(false, this, StartNodePosition, grid));
        fsm.AddState(GuardState.GuardTreasure, new GuardState_CheckTreasure(false, this));


        fsm.SetStartState(GuardState.InvestigateNoise);


        fsm.AddTriggerTransition(StateEvent.NoiseInView, new Transition<GuardState>(GuardState.InvestigateNoise, GuardState.GuardTreasure));
        fsm.AddTriggerTransition(StateEvent.TreasureSafe, new Transition<GuardState>(GuardState.GuardTreasure, GuardState.Patrolling));
        fsm.AddTriggerTransition(StateEvent.TreasureMissing, new Transition<GuardState>(GuardState.GuardTreasure, GuardState.SearchForCulprit));
        fsm.AddTriggerTransition(StateEvent.TreasureMissing, new Transition<GuardState>(GuardState.Patrolling , GuardState.SearchForCulprit));



        fsm.Init();



        collidersInLight = new List<Collider2D>();
        DetectedPlayer = false;
        aimDir = Vector3.up;
        flashLight.SetAimDirection(aimDir);
        flashLight.SetOrigin(transform.position);
        flashLight.InitLight();
        
        //Pathfinding(GoalNodePosition);
        speed = 2;
    }
    
    public void MoveAgent()
    {
        if (path.Count == 0)
        {
            OnDoneMoving();
            return;
        }

        AtEndOfPath = false;

        previousObj = currentObj;
        currentObj = path[0].gameObject;

        prevAimDir = aimDir;
        tempAimDir = aimDir;
        aimDir = currentObj.transform.position - transform.position;
        rotDist = Vector3.Distance(aimDir, prevAimDir);
        agentPos = transform.position;
        dist = Vector3.Distance(agentPos, currentObj.transform.position);
        fraction = 0;
        collidersInLight.Clear();
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

    public void IsNoiseInLight(Vector2 noiseLoc)
    {
        if(DoesLightContainNodeAt(noiseLoc))
        {
            fsm.Trigger(StateEvent.NoiseInView);
        }
    }

    public void DetectInLight(Collider2D col)
    {
        //if(!collidersInLight.Contains(col))
        //{
        //    collidersInLight.Add(col);
        //}

        if (col.tag == "Player")
        {
            DetectedPlayer = true;
        }
    }

    public List<Collider2D> GetObjsInLight()
    {
        return collidersInLight;
    }

    public bool IsTargetInRange(Vector2 target)
    {
        if(new Vector2(position.x + 2, position.y) == target ||
            new Vector2(position.x - 2, position.y) == target ||
            new Vector2(position.x, position.y + 2) == target ||
            new Vector2(position.x, position.y - 2) == target)
        {
            return true;
        }
        return false;
    }

    public bool DoesLightContainNodeAt(Vector2 noiseLoc)
    {
        foreach(GameObject collider in flashLight.objectsInView)
        {
            GridNode node = collider.GetComponent<GridNode>();
            if(node != null)
            {
                if(node.Position == noiseLoc)
                {
                    return true;
                }
            }
        }

        return false;
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

            

            if(path.Count == 0)
            {
                AtEndOfPath = true;
                if (fsm.ActiveStateName == GuardState.Patrolling)
                {
                    ((GuardState_Patrol)fsm.ActiveState).ChangeCenterPosition(grid.treasureNode);
                    ((GuardState_Patrol)fsm.ActiveState).GetRandomLocAroundStartNode(aimDir);
                }
            }

            if (fsm.ActiveStateName == GuardState.InvestigateNoise)
            {
                IsNoiseInLight(GoalNodePosition);
            }
            else if (fsm.ActiveStateName == GuardState.GuardTreasure)
            {
                if(DoesLightContainNodeAt(grid.treasureNode))
                {
                    if(grid.GetNodeAt(grid.treasureNode).IsOccupied)
                    {
                        fsm.Trigger(StateEvent.TreasureSafe);
                        ((GuardState_Patrol)fsm.ActiveState).ChangeCenterPosition(grid.treasureNode);
                        ((GuardState_Patrol)fsm.ActiveState).GetRandomLocAroundStartNode(aimDir);

                    }
                    else
                    {
                        fsm.Trigger(StateEvent.TreasureMissing);
                    }
                }
            }
            else if (fsm.ActiveStateName == GuardState.Patrolling)
            {
                if (DoesLightContainNodeAt(grid.treasureNode) && !grid.GetNodeAt(grid.treasureNode).IsOccupied)
                {
                    fsm.Trigger(StateEvent.TreasureMissing);
                }
            }

            fsm.OnLogic();
            OnDoneMoving();
        }
    }

    
    private void Update()
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

    public void Pathfinding(Vector2 goal)
    {
        int x1 = (int)currentNode.Position.x;
        int y1 = (int)currentNode.Position.y;
        int x2 = (int)goal.x;
        int y2 = (int)goal.y;

        //GoalNodePosition = goal;

        path = AStar.Search(grid,
            grid.grid[x1, y1].GetComponent<GridNode>(),
            grid.grid[x2, y2].GetComponent<GridNode>(),
            aimDir);

        if(path.Count > 0)
            AtEndOfPath = false;

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

        Gizmos.color = Color.grey;
        foreach (GameObject col in flashLight.objectsInView)
        {
            Vector2 pos = new Vector2(col.transform.position.x, col.transform.position.y);

            Gizmos.DrawSphere(pos, 0.2f);


        }

    }

}
