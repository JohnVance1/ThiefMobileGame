using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateNoise : GAction
{
    public override bool PrePerform()
    {
        // Get the spot to go to and call the Pathfinding
        target = GameManager.instance.grid.playerNode;
        return true;
    }

    private void Update()
    {
        if(running)
        {

        }
    }

    public override bool PostPerform()
    {
        //GWorld.Instance.GetWorld().ModifyState("guardTreasure", 1);
        return true;
    }

    
}
