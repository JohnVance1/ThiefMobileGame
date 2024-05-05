using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNode : Character_Base
{
    override public void Init(int x = 0, int y = 0)
    {
        base.Init(x, y);

        currentNode.SetCurrentCharacter(gameObject);
    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if(collision.gameObject.GetComponent<Player_Movement>().HasTreasure)
            {
                Debug.Log("You Escaped with the Treasure!");
            }
            Destroy(this.gameObject);
        }
    }
}
