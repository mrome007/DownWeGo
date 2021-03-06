﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    public enum Type
    {
        Player,
        Enemy
    }

    public Type PlayerType;
    
    public abstract DownFloor CurrentFloor { get; protected set; }

    public abstract void SetCurrentFloor(DownFloor floor);
        
    public abstract bool MovePlayer(int direction = -1);

    private Collider playerCollider;

    private void Awake()
    {
        playerCollider = GetComponent<Collider>();
    }

    public void PlayerSelectable(bool isSelectable)
    {
        if(playerCollider != null)
        {
            playerCollider.enabled = isSelectable;
        }
    }

    protected bool MoveTo(int newDirection)
    {
        if(CurrentFloor == null)
        {
            return false;
        }

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
