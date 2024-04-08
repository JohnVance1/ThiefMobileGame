using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : Character_Base
{
    private void Start()
    {
        
    }





    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.gameObject.GetComponent<Player_Movement>().HasTreasure = true;
            Destroy(collision.gameObject);
        }
    }


}
