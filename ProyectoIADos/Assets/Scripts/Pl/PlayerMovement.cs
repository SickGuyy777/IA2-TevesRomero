using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6;
    public float smothTime = 0.1f;
    float Velocity;
    private void Update()
    {
        float H = Input.GetAxisRaw("Horizontal");
        float V = Input.GetAxisRaw("Vertical");
        Vector3 dir =new Vector3(H,0f ,V).normalized;
        if(dir.magnitude >= 0.1f)
        {
            float tanangle=Mathf.Atan2(dir.x,dir.z)*Mathf.Rad2Deg;
            float ange = Mathf.SmoothDampAngle(transform.eulerAngles.y,tanangle,ref Velocity, smothTime);
            transform.rotation = Quaternion.Euler(0f, ange, 0f);
            Vector3 Dir=Quaternion.Euler(0f,tanangle,0f)*Vector3.forward;
            controller.Move(Dir*speed*Time.deltaTime);
        }
    }

}
