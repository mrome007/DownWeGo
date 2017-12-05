using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnsController : MonoBehaviour 
{
    [SerializeField]
    private List<FallingFloor> FallingFloors;


    private void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            NextTurn();
        }
    }

    private void NextTurn()
    {
        for(int index = 0; index < FallingFloors.Count; index++)
        {
            FallingFloors[index].HandleNextTurn();
        }
    }
}
