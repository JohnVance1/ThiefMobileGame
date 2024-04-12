using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAgent : GAgent
{
    private new void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("guardTreasure", 1, true);
        goals.Add(s1, 3);

        SubGoal s2 = new SubGoal("findCulprit", 1, true);
        goals.Add(s2, 5);
    }
}


