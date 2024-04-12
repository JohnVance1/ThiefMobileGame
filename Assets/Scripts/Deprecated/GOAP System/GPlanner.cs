using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;


public class GNode
{
    public GNode parent;
    public float cost;
    public Dictionary<string, int> state;
    public GAction action;

    public GNode(GNode parent, float cost, Dictionary<string, int> allStates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allStates);
        this.action = action;
    }

    public GNode(GNode parent, float cost, Dictionary<string, int> allStates, Dictionary<string, int> beliefStates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(allStates);
        foreach(var b in beliefStates)
        {
            if(!this.state.ContainsKey(b.Key))
            {
                this.state.Add(b.Key, b.Value);
            }
        }
        this.action = action;
    }
}



public class GPlanner
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="actions"></param>
    /// <param name="goal"></param>
    /// <param name="states"></param>
    /// <returns></returns>
    public Queue<GAction> Plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates beliefStates)
    {
        List<GAction> usableActions = new List<GAction>();
        foreach (GAction action in actions)
        {
            if(action.IsAchievable())
            {
                usableActions.Add(action);
            }
        }

        List<GNode> leaves = new List<GNode>();
        GNode start = new GNode(null, 0, GWorld.Instance.GetWorld().GetStates(), beliefStates.GetStates(), null);

        bool success = BuildGraph(start, leaves, usableActions, goal);
        if(!success)
        {
            Debug.Log("NO PLAN");
            return null;
        }

        GNode cheapest = null;
        foreach(GNode leaf in leaves)
        {
            if(cheapest == null)
            {
                cheapest = leaf;
            }
            else
            {
                if(leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }
        }

        List<GAction> result = new List<GAction>();
        GNode n = cheapest;
        while(n != null)
        {
            if(n.action != null)
            {
                result.Insert(0, n.action);

            }
            n = n.parent;
        }
        Queue<GAction> queue = new Queue<GAction>();
        foreach(GAction action in result)
        {
            queue.Enqueue(action);
        }

        Debug.Log("The Plan is: ");
        foreach(GAction action in queue)
        {
            Debug.Log($"Q: {action.actionName}");
        }

        return queue;


    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="leaves"></param>
    /// <param name="usableActions"></param>
    /// <param name="goal"></param>
    /// <returns></returns>
    private bool BuildGraph(GNode parent, List<GNode> leaves, List<GAction> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach(GAction action in usableActions)
        {
            if(action.IsAchievableGiven(parent.state))
            {
                Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                foreach(var effect in action.afterAction)
                {
                    if (!currentState.ContainsKey(effect.Key))
                    {
                        currentState.Add(effect.Key, effect.Value);
                    }
                }

                GNode node = new GNode(parent, parent.cost + action.cost, currentState, action);

                if(GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                    {
                        foundPath = true;
                    }
                }


            }
        }
        return foundPath;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="goal"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach(var g in goal)
        {
            if(!state.ContainsKey(g.Key))
            {
                return false;
            }
        }
        return true;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="actions"></param>
    /// <param name="removeMe"></param>
    /// <returns></returns>
    private List<GAction> ActionSubset(List<GAction> actions, GAction removeMe)
    {
        List<GAction> subset = new List<GAction>();
        foreach(GAction a in actions)
        {
            if(!a.Equals(removeMe))
            {
                subset.Add(a);
            }
        }
        return subset;
    }






}
