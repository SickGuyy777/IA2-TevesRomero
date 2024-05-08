using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSpot : MonoBehaviour
{
    public YellowAgent ImSpot;
    private void OnTriggerEnter(Collider other)
    {
        YellowAgent otroAgente = other.gameObject.GetComponent<YellowAgent>();
        if (otroAgente != null)
        {
            if (ImSpot && !otroAgente.ImSpot)
            {
                ImSpot.CambiarPapeles(otroAgente);
            }
        }
    }
}
