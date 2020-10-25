using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Timeline;

public class ThirdPersonMovement : MonoBehaviour 
{

    public CharacterController controller;

    public Transform cam;

    public float speed = 6;
    private float gravity = Physics.gravity.y;
    private float vSpeed = 0; //Current Vertical Speed

    public float turnSpeedTime = 0.1f;
    private float turnSmoothVelocity;
    public Animator _animator;

    //In meters
    public float JumpHeight = 1;

    public LayerMask Ground;
    public float GroundDistanceCheck = 0.2f;
    private bool _isGrounded = true;
    public Transform _groundChecker;

    // Update is called once per frame
    void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistanceCheck, Ground, QueryTriggerInteraction.Ignore);
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
        
        vSpeed += gravity * Time.deltaTime;

        if (_isGrounded || controller.isGrounded)
        {
            vSpeed = 0;
            
        } 

        if (jump && (_isGrounded || controller.isGrounded))
        {
            //Get vertical speed based on the height you want to be able to jump
            vSpeed = Mathf.Sqrt(JumpHeight * -2f * gravity); 
        }

        Vector3 gravityMove = new Vector3(0f, vSpeed, 0f);
        Vector3 move;
        if (toMove) move = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        else move = new Vector3(0f, 0f, 0f);
        _animator.SetFloat("Speed", move.magnitude);
        _animator.SetFloat("Vspeed", gravityMove.y);

        controller.Move(move.normalized * speed * Time.deltaTime + gravityMove * Time.deltaTime);
    }

    void Rotate(float targetAngle)
    {
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSpeedTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}
