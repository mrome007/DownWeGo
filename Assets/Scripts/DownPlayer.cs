using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownPlayer : MonoBehaviour 
{
    [SerializeField]
    private DownFloor currentFloor;

    public bool MovePlayer(int direction)
    {
        var moved = false;
        switch(direction)
        {
            case 0:
                if(currentFloor.UpDirection != null)
                {
                    currentFloor.RemovePlayer(this);
                    currentFloor = currentFloor.UpDirection;
                    moved = true;
                }
                break;
            case 1:
                if(currentFloor.DownDirection != null)
                {
                    currentFloor.RemovePlayer(this);
                    currentFloor = currentFloor.DownDirection;
                    moved = true;
                }
                break;
            case 2:
                if(currentFloor.LeftDirection != null)
                {
                    currentFloor.RemovePlayer(this);
                    currentFloor = currentFloor.LeftDirection;
                    moved = true;
                }
                break;
            case 3:
                if(currentFloor.RightDirection != null)
                {
                    currentFloor.RemovePlayer(this);
                    currentFloor = currentFloor.RightDirection;
                    moved = true;
                }
                break;
            default:
                break;
        }

        if(moved)
        {
            currentFloor.InsertPlayer(this);
            transform.position = currentFloor.transform.position;
            transform.parent = currentFloor.transform;
        }
        return moved;
    }
}
