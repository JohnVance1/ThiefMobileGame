using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : MonoBehaviour
{
    public GameObject currentCharacter;
    public bool IsOccupied { get; set; }
    public bool IsWall { get; set; }

    [SerializeField]
    private Sprite sprite;

    //public int x { get; set; }
    //public int y { get; set; }
    public Vector2 Position { get; set; }

    public float Priority { get; set; }


    [SerializeField]
    private GameObject[] neighbors = new GameObject[4];
    public int neighborNum { get; private set; }


    private void Start()
    {
        neighborNum = 0;
    }

    private void Update()
    {
        IsOccupied = currentCharacter != null ? true : false;
    }

    public void SetCurrentCharacter(GameObject character)
    {
        currentCharacter = character;
    }

    public void ClearCharacter()
    {
        currentCharacter = null;
    }

    public void AddNeighbor(GameObject node, int loc)
    {
        neighbors[loc] = node;
        neighborNum++;
    }

    public GameObject GetNeighbor(int loc)
    {
        return neighbors[loc];
    }

    public GameObject[] GetNeighbors()
    {
        return neighbors;
    }


}
