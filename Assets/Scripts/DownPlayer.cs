using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownPlayer : MonoBehaviour 
{
    [SerializeField]
    private DownFloor currentFloor;

    public void MovePlayer(int direction)
    {
        currentFloor.RemovePlayer();
        switch(direction)
        {
            case 0:
                if(currentFloor.UpDirection != null)
                {
                    currentFloor = currentFloor.UpDirection;
                }
                break;
            case 1:
                if(currentFloor.DownDirection != null)
                {
                    currentFloor = currentFloor.DownDirection;
                }
                break;
            case 2:
                if(currentFloor.LeftDirection != null)
                {
                    currentFloor = currentFloor.LeftDirection;
                }
                break;
            case 3:
                if(currentFloor.RightDirection != null)
                {
                    currentFloor = currentFloor.RightDirection;
                }
                break;
            default:
                break;
        }
        currentFloor.InsertPlayer(this);
        transform.position = currentFloor.transform.position;
        transform.parent = currentFloor.transform;
    }
}
