using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownFloor : MonoBehaviour
{
    [SerializeField]
    private DownFloor up;
    [SerializeField]
    private DownFloor down;
    [SerializeField]
    private DownFloor left;
    [SerializeField]
    private DownFloor right;
    [SerializeField]
    private DownPlayer player;

    public DownFloor UpDirection { get; private set; }
    public DownFloor DownDirection { get; private set; }
    public DownFloor LeftDirection { get; private set; }
    public DownFloor RightDirection { get; private set; }

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

    private void Start()
    {
        currentTurn = 0;
        UpDirection = up;
        DownDirection = down;
        LeftDirection = left;
        RightDirection = right;
    }

    public void HandleNextTurn()
    {
        if(floorType == DownFloorType.Stationary)
        {
            return;
        }

        if(player != null)
        {
            currentTurn++;
            if(currentTurn >= turnsToFall)
            {
                Fall();
            }
            else
            {
                GoDown();
            }
        }
    }

    private void GoDown()
    {
        StartCoroutine(DownMovement(GoDownDistance));
    }

    private void Fall()
    {
        StartCoroutine(DownMovement(FallDistance));
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
        player = dp;
    }

    public void RemovePlayer()
    {
        player = null;
    }
}
