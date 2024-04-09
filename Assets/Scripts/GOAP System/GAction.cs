using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GAction : MonoBehaviour
{
    // Name of the action
    public string actionName = "Action";    // Name of the action
    public float cost = 1f;                 // Cost of the action

    public GameObject target;               // Target where the action is going to take place
    public string targetTag;                // Store the tag
    public int turns = 0;             // Duration the action should take
    public WorldState[] preconditions;      // An array of WorldStates of preconditions
    public WorldState[] afterEffects;       // An array of WorldStates of afterEffects

    public Dictionary<string, int> beforeAction;    // Dictionary of preconditions
    public Dictionary<string, int> afterAction;     // Dictionary of effects

    public WorldStates agentBeliefs;        // State of the agent
    public Guard_Basic agent;               // The Movement Component of the Agent

    public bool running = false;            // Are we currently performing an action?


    public GAction()
    {
        // Set up the preconditions and effects
        beforeAction = new Dictionary<string, int>();
        afterAction = new Dictionary<string, int>();
    }

    public void Awake()
    {
        // Get the agent's Movement Script
        agent = GetComponent<Guard_Basic>();

        // Check validity of preConditions
        if (preconditions != null)
        {
            foreach(WorldState state in preconditions)
            {
                // Add each item to our Dictionary
                beforeAction.Add(state.key, state.value);
            }
        }

        // Check validity of afterEffects
        if (afterEffects != null)
        {
            foreach (WorldState state in afterEffects)
            {
                // Add each item to our Dictionary
                afterAction.Add(state.key, state.value);
            }
        }
    }

    public bool IsAchievable()
    {
        return true;
    }    

    public bool IsAchievableGiven(Dictionary<string, int> conditions)
    {
        foreach(var b in beforeAction)
        {
            if(!conditions.ContainsKey(b.Key))
            {
                return false;
            }
        }
        return true;
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
