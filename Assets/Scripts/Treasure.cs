using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : Character_Base
{
    override public void Init(int x = 0, int y = 0)
    {
        base.Init(x, y);

        currentNode.SetCurrentCharacter(gameObject);
    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.gameObject.GetComponent<Player_Movement>().HasTreasure = true;
            Destroy(this.gameObject);
        }
    }


}
