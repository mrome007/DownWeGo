using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownEnemy : MonoBehaviour 
{
    private DownFloor currentFloor;

    public void SetCurrentFloor(DownFloor current)
    {
        currentFloor = current;
    }

    public bool MoveEnemy()
    {
        var moved = false;

        var randomMovement = Random.Range(0, 4);

        switch(randomMovement)
        {
            case 0:
                if(currentFloor.UpDirection != null)
                {
                    currentFloor.RemoveEnemy(this);
                    currentFloor = currentFloor.UpDirection;
                    moved = true;
                }
                break;
            case 1:
                if(currentFloor.DownDirection != null)
                {
                    currentFloor.RemoveEnemy(this);
                    currentFloor = currentFloor.DownDirection;
                    moved = true;
                }
                break;
            case 2:
                if(currentFloor.LeftDirection != null)
                {
                    currentFloor.RemoveEnemy(this);
                    currentFloor = currentFloor.LeftDirection;
                    moved = true;
                }
                break;
            case 3:
                if(currentFloor.RightDirection != null)
                {
                    currentFloor.RemoveEnemy(this);
                    currentFloor = currentFloor.RightDirection;
                    moved = true;
                }
                break;
            default:
                break;
        }

        if(moved)
        {
            currentFloor.InsertEnemy(this);
            transform.position = currentFloor.transform.position;
            transform.parent = currentFloor.transform;
        }

        return moved;
    }
}
