using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class Player : SessionObject, IMovable
{
    public float MoveSpeed = 1.5f;
    public float RunSpeed = 3f;

    public bool Running
    {
        get => running;
        set
        {
            running = value;
            UpdateMove();
        }
    }

    public Vector3Int PositionInt => Vector3Int.FloorToInt(transform.position);
    
    private static readonly int speedX = Animator.StringToHash("speedX");
    private static readonly int speedY = Animator.StringToHash("speedY");

    private Animator animator;
    private Camera playerCamera;
    private Vector3 lastMoveDirection;
    private bool running = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnSessionInitialized()
    {
        base.OnSessionInitialized();

        // TODO: Temporary implementation, should take into consideration multiple players
        var cameras = Utils.FindTypesOnScene<Camera>(gameObject.scene);
        foreach (var cam in cameras)
            if (cam.CompareTag("MainCamera"))
                playerCamera = cam;
    }

    /// <summary>
    /// Call it when something when something related with movement state changes. (ex. camera angle)
    /// </summary>
    public void UpdateMove()
    {
        Move(lastMoveDirection);
    }

    public void Move(Vector3 direction)
    {
        var speed = Running ? RunSpeed : MoveSpeed;
        lastMoveDirection = direction;
        
        Rotate(direction);

        animator.SetFloat(speedY, Mathf.Abs(direction.magnitude * speed));
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
