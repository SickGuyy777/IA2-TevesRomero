using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchLogic : MonoBehaviour
{
    public YellowAgent ags;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("YellowTeam"))
        {
                ags.ImSpot = true;
        }
    }
}
