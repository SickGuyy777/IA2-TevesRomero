using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotCam : MonoBehaviour
{
    public Vector2 turn;
    public float sensibility=.5f;
    public float maxVerticalAngle = 80f; // Define el ángulo máximo vertical
    void Update()
    {
        turn.y += Input.GetAxis("Mouse Y") * sensibility;
        turn.x += Input.GetAxis("Mouse X") * sensibility;
        //transform.localRotation=Quaternion.Euler(-turn.y,turn.x,0);
        turn.y = Mathf.Clamp(turn.y, -maxVerticalAngle, maxVerticalAngle);

        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }
}
