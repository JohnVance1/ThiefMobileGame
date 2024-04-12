using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchForTreasure : GAction
{
    public override bool PrePerform()
    {
        return true;
    }

    private void Update()
    {
        if (running)
        {

        }
    }

    public override bool PostPerform()
    {
        return true;
    }
}
