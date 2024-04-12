using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public GameObject player;
    public List<GameObject> guards;
    public GameObject treasure;
    public GridControls grid;

    public Player_Movement playerMove;
    public bool CanMovePlayer;

    public int turnNum = 0;

    public Vector2 playerStart = new Vector2(0, 4);
    public Vector2 treasureStart = new Vector2(4, 4);

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        foreach(GameObject guard in guards)
        {
            Guard_Basic guard_B = guard.GetComponent<Guard_Basic>();
            guard_B.OnDoneMoving += PlayerFlag;
        }
    }

    private void OnDisable()
    {
        foreach (GameObject guard in guards)
        {
            Guard_Basic guard_B = guard.GetComponent<Guard_Basic>();
            guard_B.OnDoneMoving -= PlayerFlag;

        }
    }


    private void Start()
    {
        playerMove = player.GetComponent<Player_Movement>();
        playerMove.OnPlayerMoved += MoveWorld;

        InitilizeWorld();


    }

    private void Update()
    {
        foreach(GameObject guard in guards)
        {
            if(guard.GetComponent<Guard_Basic>().DetectedPlayer)
            {
                PlayerDetected();
            }
        }
    }


    public void InitilizeWorld()
    {
        grid.SetStartNode(0, 4);
        player.GetComponent<Player_Movement>().Init((int)playerStart.x, (int)playerStart.y);
        grid.SetPlayerNode((int)playerStart.x, (int)playerStart.y);


        treasure.GetComponent<Treasure>().Init((int)treasureStart.x, (int)treasureStart.y);
        grid.SetTreasureNode((int)treasureStart.x, (int)treasureStart.y);

        foreach (GameObject guard in guards)
        {
            guard.GetComponent<Guard_Basic>().Init();
        }


        
    }

    /// <summary>
    /// Need to update to detect in a radius
    /// </summary>
    /// <param name="loc">The location of where the noise is at</param>
    public void NoiseAt()
    {
        foreach (GameObject go in guards)
        {
            go.GetComponent<Guard_Basic>().GoalNodePosition = playerStart;

        }
    }

    public void MoveWorld()
    {
        StartCoroutine(MoveAgents());
        turnNum++;
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
       
    public void PlayerDetected()
    {
        Debug.Log("Hit Player");
    }



}
