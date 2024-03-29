using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GridControls : MonoBehaviour
{
    public int width = 5;
    public int height = 5;
    public GameObject nodePrefab;
    //public List<List<GameObject>> grid;
    public GameObject[,] grid;

    public void GenerateGrid()
    {
        grid = new GameObject[width, height];
        GameObject objToSpawn;
        ClearGrid();
        for (int i = 0; i < height; i++) 
        {
            //List<GameObject> list = new List<GameObject>();
            objToSpawn = new GameObject("Row" + i);
            objToSpawn.transform.parent = gameObject.transform;
            for (int j = 0; j < width; j++)
            {
                GameObject gO = Instantiate(nodePrefab, new Vector3((i * 2) - (width / 2) - 2, (j * 2) - (height / 2) - 2, 0), Quaternion.identity);
                gO.transform.parent = objToSpawn.transform;
                gO.GetComponent<GridNode>().Position = new Vector2(i, j);

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

    public GameObject SetCurrentNode(int x, int y)
    {
        return grid[x, y];
    }

    public int Cost(GridNode b)
    {
        // Will add in a set for Walls
        return 1;
    }


    public GameObject GetNextNode(Vector2 moveVec, GridNode currentNode)
    {
        GameObject nextNode = null;

        if(moveVec == Vector2.right)
        {
            nextNode = currentNode.GetNeighbor(0);
        }
        else if (moveVec == -Vector2.right)
        {
            nextNode = currentNode.GetNeighbor(2);

        }
        else if (moveVec == Vector2.up)
        {
            nextNode = currentNode.GetNeighbor(3);

        }
        else if (moveVec == -Vector2.up)
        {
            nextNode = currentNode.GetNeighbor(1);

        }

        return nextNode;


    }


}
