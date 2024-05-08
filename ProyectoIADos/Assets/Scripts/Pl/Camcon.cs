using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camcon : MonoBehaviour
{
    public Transform target;
    public float pLerp=.02f;
    public float rLerp=.01f;

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, pLerp);
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, rLerp);
    }
}
