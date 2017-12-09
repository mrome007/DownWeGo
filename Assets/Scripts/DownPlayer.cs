using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownPlayer : Player
{
    public override DownFloor CurrentFloor { get; protected set; }

    public override void SetCurrentFloor(DownFloor floor)
    {
        CurrentFloor = floor;
    }

    public override bool MovePlayer(int direction = -1)
    {
        return MoveTo(direction);
    }
}
