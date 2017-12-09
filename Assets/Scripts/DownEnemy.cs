using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownEnemy : Player
{
    public override DownFloor CurrentFloor { get; protected set; }

    public override void SetCurrentFloor(DownFloor floor)
    {
        CurrentFloor = floor;
    }

    public override bool MovePlayer(int direction = -1)
    {
        var randomMovement = -1;
        if(direction == -1)
        {
            randomMovement = Random.Range(0, 4);
        }
            
        return MoveTo(randomMovement);
    }
}
