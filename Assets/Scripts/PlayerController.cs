using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private static readonly int speedX = Animator.StringToHash("speedX");
    private static readonly int speedY = Animator.StringToHash("speedY");
    
    private Player player;
    private Animator animator;
    private Vector2 input = Vector2.zero;
    private Camera mainCamera;
    private bool running = false;

    private void Awake()
    {
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();

        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        RotatePlayer();
        UpdateAnimator();
    }

    private void RotatePlayer()
    {
        if (input == Vector2.zero)
            return;
        
        var forwardCamera = mainCamera.transform.forward;
        forwardCamera.y = 0f;
        forwardCamera.Normalize();
        var rightCamera = mainCamera.transform.right;
        rightCamera.y = 0f;
        rightCamera.Normalize();

        var forwardPlayer = player.transform.forward;
        forwardPlayer.y = 0f;
        forwardPlayer.Normalize();

        // player.transform.forward = Vector3.RotateTowards(player.transform.forward,
        //     (rightCamera * input.x + forwardCamera * input.y + Vector3.up * player.transform.forward.y).normalized,
        //     player.RotationSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime, 1f);
        player.transform.forward = rightCamera * input.x + forwardCamera * input.y + Vector3.up * player.transform.forward.y;
    }

    private void UpdateAnimator()
    {
        var speed = running ? player.RunSpeed : player.Speed;

        animator.SetFloat(speedX, Mathf.Abs(input.x * speed));
        animator.SetFloat(speedY, Mathf.Abs(input.y * speed));
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            input = Vector2.zero;
            return;
        }
        
        if (!context.performed)
            return;

        input = context.ReadValue<Vector2>();
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.started)
            running = true;
        if (context.canceled)
            running = false;
    }
}
