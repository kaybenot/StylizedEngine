using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class Player : MonoBehaviour, IMovable
{
    public float MoveSpeed = 1.5f;
    public float RunSpeed = 3f;
    
    public bool Running { get; set; }
    
    private static readonly int speedX = Animator.StringToHash("speedX");
    private static readonly int speedY = Animator.StringToHash("speedY");

    private Animator animator;
    private Camera playerCamera;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        // TODO: Temporary implementation, should take into consideration multiple players
        playerCamera = Camera.main;
    }

    public void Move(Vector3 direction)
    {
        var speed = Running ? RunSpeed : MoveSpeed;
        
        Rotate(direction);

        animator.SetFloat(speedY, Mathf.Abs(direction.magnitude * speed));
        
        // Currently not used
        animator.SetFloat(speedX, Mathf.Abs(0f));
    }
    
    private void Rotate(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;
        
        var forwardCamera = playerCamera.transform.forward;
        forwardCamera.y = 0f;
        forwardCamera.Normalize();
        var rightCamera = playerCamera.transform.right;
        rightCamera.y = 0f;
        rightCamera.Normalize();

        var forwardPlayer = transform.forward;
        forwardPlayer.y = 0f;
        forwardPlayer.Normalize();
        
        transform.forward = rightCamera * direction.x + forwardCamera * direction.z + Vector3.up * transform.forward.y;
    }
}