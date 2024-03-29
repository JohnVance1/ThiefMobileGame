using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Guard_Basic : Character_Base
{

    public Vector2 StartNodePosition;
    public Vector2 GoalNodePosition;


    private void Start()
    {
        currentObj = grid.SetCurrentNode((int)StartNodePosition.x, (int)StartNodePosition.y);
        transform.position = currentObj.transform.position;
        currentNode = currentObj.GetComponent<GridNode>();
    }

    public void Pathfinding()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        int x1 = (int)StartNodePosition.x;
        int y1 = (int)StartNodePosition.y;
        int x2 = (int)GoalNodePosition.x;
        int y2 = (int)GoalNodePosition.y;

        List<GridNode> path = AStar.Search(grid,
            grid.grid[x1, y1].GetComponent<GridNode>(),
            grid.grid[x2, y2].GetComponent<GridNode>());

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
    }

}
