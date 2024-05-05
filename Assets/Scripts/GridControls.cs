using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridControls : MonoBehaviour
{
    public int width = 5;
    public int height = 5;
    public GameObject nodePrefab;
    //public List<List<GameObject>> grid;
    public GameObject[,] grid;
    public List<Vector2> wallPositions;

    public Vector2 startNode;
    public Vector2 treasureNode;
    public Vector2 playerNode;


    public void GenerateGrid()
    {
        grid = new GameObject[width, height];
        GameObject objToSpawn;
        ClearGrid();
        for (int i = 0; i < height; i++) 
        {
            objToSpawn = new GameObject("Row" + i);
            objToSpawn.transform.parent = gameObject.transform;
            for (int j = 0; j < width; j++)
            {
                GameObject gO = Instantiate(nodePrefab, new Vector3((i * 2) - (width / 2) - 2, (j * 2) - (height / 2) - 2, 0), Quaternion.identity);
                gO.transform.parent = objToSpawn.transform;
                GridNode node = gO.GetComponent<GridNode>();
                node.Position = new Vector2(i, j);
                if(wallPositions.Contains(node.Position))
                {
                    node.cost = 100;
                    node.IsWall = true;
                }
                else
                {
                    node.cost = 1;
                    node.IsWall = false;

                }
                //list.Add(gO);
                grid[i, j] = gO;
            }
            //grid.Add(list);
        }
        SetNeighbors();
    }

    public void ClearGrid()
    {
        if(grid != null)
        {
            //grid.Clear();
            grid = new GameObject[width, height];

        }
        if (gameObject.transform.childCount > 0)
        {
            for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(gameObject.transform.GetChild(i).gameObject);
            }
        }

    }
    public void SetStartNode(int x, int y)
    {
        grid[x, y].GetComponent<GridNode>().IsStartNode = true;
        startNode = new Vector2(x, y);
    }

    public void SetPlayerNode(int x, int y)
    {
        playerNode = new Vector2(x, y);
    }

    public void SetTreasureNode(int x, int y)
    {
        treasureNode = new Vector2(x, y);
    }

    private void Awake()
    {
        GenerateGrid();
    }
        
    /// <summary>
    /// i = y, j = x
    /// </summary>
    public void SetNeighbors()
    {
        for(int i = 0; i < height; i++) 
        {
            for (int j = 0; j < width; j++)
            {
                if(i < height - 1)
                {
                    grid[i, j].GetComponent<GridNode>().AddNeighbor(grid[i + 1, j], 0);

                }
                if (j > 0)
                {
                    grid[i, j].GetComponent<GridNode>().AddNeighbor(grid[i, j - 1], 1);

                }
                if (i > 0)
                {
                    grid[i, j].GetComponent<GridNode>().AddNeighbor(grid[i - 1, j], 2);

                }
                if (j < width - 1)
                {
                    grid[i, j].GetComponent<GridNode>().AddNeighbor(grid[i, j + 1], 3);
                        
                }

            }
        }
    }

    public GameObject[] GetNeighbors(GridNode node)
    {
        return node.GetNeighbors();
    }

    public GridNode GetNodeAt(Vector2 position)
    {
        return grid[(int)position.x, (int)position.y].GetComponent<GridNode>();
    }

    public GameObject SetCurrentNode(int x, int y)
    {
        return grid[x, y];
    }

    public Collider2D GetColliderAt(Vector2 position)
    {
        return grid[(int)position.x, (int)position.y].GetComponent<Collider2D>();
    }

    public int Cost(GridNode b)
    {
        // Will add in a set for Walls
        return b.cost;
    }


    public GameObject GetNextNode(Vector2 moveVec, GridNode currentNode)
    {
        GameObject nextNode = null;

        if(moveVec == Vector2.right)
        {
            if(!currentNode.GetNeighbor(0).GetComponent<GridNode>().IsWall)
            {
                nextNode = currentNode.GetNeighbor(0);
            }
        }
        else if (moveVec == -Vector2.right)
        {
            if (!currentNode.GetNeighbor(2).GetComponent<GridNode>().IsWall)
            {
                nextNode = currentNode.GetNeighbor(2);
            }
        }
        else if (moveVec == Vector2.up)
        {
            if (!currentNode.GetNeighbor(3).GetComponent<GridNode>().IsWall)
            {
                nextNode = currentNode.GetNeighbor(3);
            }
        }
        else if (moveVec == -Vector2.up)
        {
            if (!currentNode.GetNeighbor(1).GetComponent<GridNode>().IsWall)
            {
                nextNode = currentNode.GetNeighbor(1);
            }
        }

        return nextNode;


    }

    public bool IsNodeWall(Vector2 nodeLoc)
    {
        if(wallPositions.Contains(nodeLoc))
        {
            return true;
        }
        return false;
    }

    public bool IsInRange(Vector2 nodeLoc)
    {
        if (nodeLoc.x < width &&
            nodeLoc.x >= 0 &&
            nodeLoc.y < height &&
            nodeLoc.y >= 0)
        {
            return true;
        }
        return false;
    }


}
