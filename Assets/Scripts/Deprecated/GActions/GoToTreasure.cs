using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTreasure : GAction
{
    public override bool PrePerform()
    {
        target = GameManager.instance.grid.treasureNode;
        return true;
    }

    private void Update()
    {
        if(running)
        {
            //if(agent.IsTargetInRange(target) && 
            //    !agent.DoesLightContainNode(GameManager.instance.treasure.GetComponent<Collider2D>()))
            //{

            //}
        }
    }

    public override bool PostPerform()
    {
        return true;
    }
}
