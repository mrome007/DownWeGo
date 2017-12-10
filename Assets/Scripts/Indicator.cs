using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour 
{
    public void ShowIndicator(bool show)
    {
        gameObject.SetActive(show);
    }

    public void MoveIndicator(Vector3 pos)
    {
        transform.position = pos;
    }
}
