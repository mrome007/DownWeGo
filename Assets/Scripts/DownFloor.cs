using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownFloor : MonoBehaviour
{
    private List<Player> players;

    public DownFloor UpDirection;
    public DownFloor DownDirection;
    public DownFloor LeftDirection;
    public DownFloor RightDirection;

    public enum DownFloorType
    {
        Falling,
        Stationary,
        Win,
        Lose
    }

    [SerializeField]
    private DownFloorType floorType;

    [SerializeField]
    private int turnsToFall = 2;

    private int currentTurn = 0;

    private const float FallOffset = 0.01f;
    private const float FallTime = 2f;
    private const float GoDownDistance = 0.5f;
    private const float FallDistance = 30f;

    private void Awake()
    {
        players = new List<Player>();
    }

    private void Start()
    {
        currentTurn = 0;
    }

    public void HandleDownFloor()
    {
        if(floorType == DownFloorType.Stationary || floorType == DownFloorType.Win)
        {
            return;
        }

        if(players.Count > 0)
        {
            currentTurn += players.Count;
            if(currentTurn >= turnsToFall)
            {
                StartCoroutine(Fall());
            }
            else
            {
                GoDown();
            }
        }

        if(floorType == DownFloorType.Lose)
        {
            currentTurn++;
            if(currentTurn >= turnsToFall)
            {
                StartCoroutine(Fall());
            }
            else
            {
                GoDown();
            }
        }
    }

    public void SetFloorType(DownFloorType type)
    {
        floorType = type;
    }

    public bool IsWinningFloor()
    {
        return floorType == DownFloorType.Win; 
    }

    private void GoDown()
    {
        StartCoroutine(DownMovement(GoDownDistance * (players.Count)));
    }

    private IEnumerator Fall()
    {
        yield return StartCoroutine(DownMovement(FallDistance));
        Destroy(gameObject);
    }

    private IEnumerator DownMovement(float downDistance)
    {
        var targetYPos = transform.position.y - downDistance;
        var speed = downDistance / FallTime;
        while(transform.position.y >= targetYPos + FallOffset)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, targetYPos, transform.position.z);
    }

    public void InsertPlayer(Player dp)
    {
        if(dp.PlayerType == Player.Type.Enemy)
        {
            for(int index = 0; index < players.Count; index++)
            {
                bool hasMoved = false;
                var player = players[index];
                if(player.PlayerType == Player.Type.Enemy)
                {
                    continue;
                }
                for(int direction = 0; direction < 4; direction++)
                {
                    if(!hasMoved)
                    {
                        hasMoved = player.MovePlayer(direction);
                    }
                }
            }
        }
        players.Add(dp);
    }

    public void RemovePlayer(Player dp)
    {
        players.Remove(dp);
    }
}
