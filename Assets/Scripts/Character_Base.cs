using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class Character_Base : MonoBehaviour
{
    public GameObject currentObj;
    protected GridNode currentNode;
    public GridControls grid;
    public int speed;
    public Vector2 position;


    virtual public void Init(int x = 0, int y = 0)
    {
        currentObj = grid.SetCurrentNode(x, y);
        transform.position = currentObj.transform.position;
        currentNode = currentObj.GetComponent<GridNode>();
        currentNode.currentCharacter = this.gameObject;
        position = currentNode.Position;
    }




}
