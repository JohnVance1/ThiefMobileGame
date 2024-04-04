using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> guards;
    public GridControls grid;

    public Player_Movement playerMove;
    public bool CanMovePlayer;

    private void OnEnable()
    {
        foreach(GameObject guard in guards)
        {
            guard.GetComponent<Guard_Basic>().flashLight.onColliderHit += CheckFlashLightCollider;
        }
    }

    private void OnDisable()
    {
        foreach (GameObject guard in guards)
        {
            guard.GetComponent<Guard_Basic>().flashLight.onColliderHit -= CheckFlashLightCollider;
        }
    }


    private void Start()
    {
        playerMove = player.GetComponent<Player_Movement>();
        playerMove.OnPlayerMoved += MoveWorld;
    }


    private void FixedUpdate()
    {
       

    }

    public void MoveWorld()
    {
        StartCoroutine(MoveAgents());

    }

    public IEnumerator MoveAgents()
    {
        playerMove.CanMove = false;
        yield return new WaitForSeconds(1);
        foreach (GameObject go in guards)
        {
            go.GetComponent<Guard_Basic>().MoveAgent();
        }
        playerMove.CanMove = true;
    }

    public void CheckFlashLightCollider(Collider2D col)
    {
        if(col.tag == "Player")
        {
            PlayerDetected();
        }
    }

    public void PlayerDetected()
    {
        Debug.Log("Hit Player");
    }



}
