using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Timeline;

public class ThirdPersonMovement : MonoBehaviour 
{

    public CharacterController controller;

    public Transform cam;

    public float speed = 6;
    public float jumpSpeed = 8;
    public float gravity = 9.8f;
    public float vSpeed = 0; //Current Vertical Speed

    public float turnSpeedTime = 0.1f;
    private float turnSmoothVelocity;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveDirection = new Vector3(0f, 0f, 0f);
        bool jump = Input.GetKeyDown("space");
        if (controller.isGrounded)
        {
            vSpeed = 0;
            if (jump)
            {
                vSpeed = jumpSpeed;
            }
        }
        if (!controller.isGrounded)
        {
            //apply Gravity
            vSpeed -= gravity + Time.deltaTime;
        }

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSpeedTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;  
        }

        Vector3 vel;
        if (vSpeed != 0)
        {
            vel = (moveDirection.normalized * speed * Time.deltaTime) + (Vector3.up * vSpeed * Time.deltaTime);
        } else
        {
            vel = (moveDirection.normalized * speed * Time.deltaTime);
        }

        controller.Move(vel);
    }
}
