using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAgent : GAgent
{
    private void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("IsPatrolling", 1, true);
        goals.Add(s1, 3);
    }

}
