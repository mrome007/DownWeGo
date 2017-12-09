using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownFloor : MonoBehaviour
{
    private List<DownPlayer> player;
    private List<DownEnemy> enemy;

    public DownFloor UpDirection;
    public DownFloor DownDirection;
    public DownFloor LeftDirection;
    public DownFloor RightDirection;

    private enum DownFloorType
    {
        Falling,
        Stationary
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
        player = new List<DownPlayer>();
        enemy = new List<DownEnemy>();
    }

    private void Start()
    {
        currentTurn = 0;
    }

    public void HandleNextTurn()
    {
        if(floorType == DownFloorType.Stationary)
        {
            return;
        }

        if(player.Count > 0 || enemy.Count > 0)
        {
            currentTurn += player.Count + enemy.Count;
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

    private void GoDown()
    {
        StartCoroutine(DownMovement(GoDownDistance * (player.Count + enemy.Count)));
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

    public void InsertPlayer(DownPlayer dp)
    {
        player.Add(dp);
    }

    public void RemovePlayer(DownPlayer dp)
    {
        player.Remove(dp);
    }

    public void InsertEnemy(DownEnemy de)
    {
        enemy.Add(de);
    }

    public void RemoveEnemy(DownEnemy de)
    {
        enemy.Remove(de);
    }
}
