using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnsController : MonoBehaviour 
{
    [SerializeField]
    private List<DownFloor> fallingFloors;

    [SerializeField]
    private int numberOfActions = 3;

    private DownPlayer selectedPlayer;
    private int playerLayerMask;
    private Vector3 mousePosition;
    private bool playTurn = true;

    private void Start()
    {
        selectedPlayer = null;  
        playTurn = true;
        playerLayerMask = 1 << LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        if(!playTurn)
        {
            return;
        }
        
        if(Input.GetMouseButtonDown(0))
        {
            mousePosition = Input.mousePosition;
            SelectPlayer();
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(selectedPlayer != null)
            {
                var mousePositionLast = Input.mousePosition;
                var yDifference = mousePositionLast.y - mousePosition.y;
                var xDifference = mousePositionLast.x - mousePosition.x;
                if(Mathf.Abs(yDifference) > Mathf.Abs(xDifference))
                {
                    selectedPlayer.MovePlayer(yDifference > 0 ? 0 : 1);
                }
                else
                {
                    selectedPlayer.MovePlayer(xDifference > 0 ? 3 : 2);
                }
                selectedPlayer = null;
                numberOfActions--;
                if(numberOfActions == 0)
                {
                    numberOfActions = 3;
                    StartCoroutine(NextTurn());
                }
            }
        }
    }

    private IEnumerator NextTurn()
    {
        playTurn = false;
        for(int index = 0; index < fallingFloors.Count; index++)
        {
            fallingFloors[index].HandleNextTurn();
        }

        yield return new WaitForSeconds(2f);

        playTurn = true;
    }

    private void SelectPlayer()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100f, playerLayerMask))
        {
            selectedPlayer = hit.collider.GetComponent<DownPlayer>();
        }
    }
}
