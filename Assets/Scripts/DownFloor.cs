using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownFloor : MonoBehaviour
{
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
    }

    public void HandleNextTurn()
    {
        if(floorType == DownFloorType.Stationary)
        {
            return;
        }
        
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
}
