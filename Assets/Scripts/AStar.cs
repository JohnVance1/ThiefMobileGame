using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PriorityQueue;

/// <summary>
/// Implementation of Amit Patel's A* Pathfinding algorithm studies
/// https://www.redblobgames.com/pathfinding/a-star/introduction.html
/// </summary>
public static class AStar
{
    /// <summary>
    /// Returns the best path as a List of Nodes
    /// </summary>
    public static List<GridNode> Search(GridControls graph, GridNode start, GridNode goal)
    {
        Dictionary<GridNode, GridNode> came_from = new Dictionary<GridNode, GridNode>();
        Dictionary<GridNode, float> cost_so_far = new Dictionary<GridNode, float>();

        List<GridNode> path = new List<GridNode>();

        PriorityQueue<GridNode, float> frontier = new PriorityQueue<GridNode, float>(0);
        frontier.Enqueue(start, 0);

        came_from.Add(start, start);
        cost_so_far.Add(start, 0);

        GridNode current = null;
        while (frontier.Count > 0)
        {
            current = frontier.Dequeue();
            if (current == goal) break; // Early exit

            foreach (GameObject nextNode in graph.GetNeighbors(current))
            {
                if (nextNode != null)
                {
                    GridNode next = nextNode.GetComponent<GridNode>();
                    float new_cost = cost_so_far[current] + graph.Cost(next);
                    if (!cost_so_far.ContainsKey(next) || new_cost < cost_so_far[next])
                    {
                        cost_so_far[next] = new_cost;
                        came_from[next] = current;
                        float priority = new_cost + Heuristic(next, goal);
                        frontier.Enqueue(next, priority);
                        next.Priority = new_cost;
                    }
                }
            }
        }

        while (current != start)
        {
            path.Add(current);
            current = came_from[current];
        }
        path.Reverse();

        return path;
    }

    public static float Heuristic(GridNode a, GridNode b)
    {
        return Mathf.Abs(a.Position.x - b.Position.x) + Mathf.Abs(a.Position.y - b.Position.y);
    }
}
