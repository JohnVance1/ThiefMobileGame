using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> guards;
    public GameObject treasure;
    public GridControls grid;

    public Player_Movement playerMove;
    public bool CanMovePlayer;

    private void OnEnable()
    {
        foreach(GameObject guard in guards)
        {
            Guard_Basic guard_B = guard.GetComponent<Guard_Basic>();
            guard_B.flashLight.onColliderHit += CheckFlashLightCollider;
            guard_B.OnDoneMoving += PlayerFlag;
        }
    }

    private void OnDisable()
    {
        foreach (GameObject guard in guards)
        {
            Guard_Basic guard_B = guard.GetComponent<Guard_Basic>();
            guard_B.flashLight.onColliderHit -= CheckFlashLightCollider;
            guard_B.OnDoneMoving -= PlayerFlag;

        }
    }


    private void Start()
    {
        playerMove = player.GetComponent<Player_Movement>();
        playerMove.OnPlayerMoved += MoveWorld;

        InitilizeWorld();


    }


    private void FixedUpdate()
    {
       

    }

    public void InitilizeWorld()
    {
        foreach(GameObject guard in guards)
        {
            guard.GetComponent<Guard_Basic>().Init();
        }

        player.GetComponent<Player_Movement>().Init(0, 4);
        treasure.GetComponent<Treasure>().Init(4, 4);
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
    }

    public void PlayerFlag()
    {
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
