using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public abstract DownFloor CurrentFloor { get; protected set; }

    public abstract void SetCurrentFloor(DownFloor floor);
        
    public abstract bool MovePlayer(int direction = -1);

    protected bool MoveTo(int newDirection)
    {
        var moved = false;
        switch(newDirection)
        {
            case 0:
                if(CurrentFloor.UpDirection != null)
                {
                    CurrentFloor.RemovePlayer(this);
                    CurrentFloor = CurrentFloor.UpDirection;
                    moved = true;
                }
                break;
            case 1:
                if(CurrentFloor.DownDirection != null)
                {
                    CurrentFloor.RemovePlayer(this);
                    CurrentFloor = CurrentFloor.DownDirection;
                    moved = true;
                }
                break;
            case 2:
                if(CurrentFloor.LeftDirection != null)
                {
                    CurrentFloor.RemovePlayer(this);
                    CurrentFloor = CurrentFloor.LeftDirection;
                    moved = true;
                }
                break;
            case 3:
                if(CurrentFloor.RightDirection != null)
                {
                    CurrentFloor.RemovePlayer(this);
                    CurrentFloor = CurrentFloor.RightDirection;
                    moved = true;
                }
                break;
            default:
                break;
        }

        if(moved)
        {
            CurrentFloor.InsertPlayer(this);
            transform.position = CurrentFloor.transform.position;
            transform.parent = CurrentFloor.transform;
        }

        return moved;
    }
}
