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
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

        Move(targetAngle, direction.magnitude >= 0.05f);
        Rotate(targetAngle);
    }

    void Move(float targetAngle, bool toMove)
    {
        bool jump = Input.GetKeyDown("space");

        if (controller.isGrounded)
        {
            vSpeed = 0;
            if (jump)
            {
                vSpeed = jumpSpeed;
            }
        }
        else vSpeed -= gravity * Time.deltaTime;

        Vector3 gravityMove = new Vector3(0f, vSpeed, 0f);
        Vector3 move;
        if (toMove) move = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        else move = new Vector3(0f, 0f, 0f);

        controller.Move(move.normalized * speed * Time.deltaTime + gravityMove * Time.deltaTime);
    }

    void Rotate(float targetAngle)
    {
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSpeedTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}
