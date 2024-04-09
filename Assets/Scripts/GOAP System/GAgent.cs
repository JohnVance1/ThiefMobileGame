using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class SubGoal
{

    // Dictionary to store our goals
    public Dictionary<string, int> sGoals;
    // Bool to store if goal should be removed
    public bool remove;

    // Constructor
    public SubGoal(string s, int i, bool r)
    {

        sGoals = new Dictionary<string, int>();
        sGoals.Add(s, i);
        remove = r;
    }
}
public class GAgent : MonoBehaviour
{
    public List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public WorldStates worldStates = new WorldStates();

    GPlanner planner;
    Queue<GAction> actionQueue;
    public GAction currentAction;
    SubGoal currentGoal;

    bool invoked = false;
    int turnCount = 0;

    public void Start()
    {
        GAction[] acts = this.GetComponents<GAction>();
        foreach(GAction act in acts) 
        {
            actions.Add(act);
        }
    }

    private void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }

    public void UpdatePlan()
    {
        // If the current Action is running and is not null
        // then complete the action after the duration is hit
        if (currentAction != null && currentAction.running)
        {
            // Check the agent has a goal and has reached that goal
            // Runs when the Agent has reached the end of their path
            if (currentAction.agent.AtEndOfPath)
            {
                if (!invoked)
                {
                    turnCount = currentAction.turns;
                    invoked = true;
                }

                if (turnCount > 0)
                {
                    turnCount--;
                }
                else
                {
                    CompleteAction();
                }
            }
            

        }


        if (planner != null || actionQueue == null)
        {
            planner = new GPlanner();

            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach (var sg in sortedGoals)
            {
                actionQueue = planner.Plan(actions, sg.Key.sGoals, null);
                if (actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }


        }

        // Do we have an actionQueue
        if (actionQueue != null && actionQueue.Count > 0)
        {
            // Check if currentGoal is removable
            if (currentGoal.remove)
            {
                // Remove it
                goals.Remove(currentGoal);

            }
            // Set planner = null so it will trigger a new one
            planner = null;
        }

        // Do we still have actions?
        if (actionQueue != null && actionQueue.Count > 0)
        {
            // Remove the top action of the queue and put it in currentAction
            currentAction = actionQueue.Dequeue();


            if (currentAction.PrePerform())
            {
                // Run the currentAction
                currentAction.running = true;
            }
            else
            {
                // Force a new Plan
                actionQueue = null;
            }
        }



    }


    void LateUpdate()
    {
        

    }
}
