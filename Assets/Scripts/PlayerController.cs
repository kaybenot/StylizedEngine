using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{
    private static readonly int speedX = Animator.StringToHash("speedX");
    private static readonly int speedY = Animator.StringToHash("speedY");
    
    private Player player;
    private Animator animator;
    private Vector2 input = Vector2.zero;
    private Camera mainCamera;

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
        var forwardCamera = mainCamera.transform.forward;
        forwardCamera.y = 0f;
        forwardCamera.Normalize();
        var rightCamera = mainCamera.transform.right;
        rightCamera.y = 0f;
        rightCamera.Normalize();

        var forwardPlayer = player.transform.forward;
        forwardPlayer.y = 0f;
        forwardPlayer.Normalize();

        player.transform.forward = Vector3.RotateTowards(player.transform.forward,
            (rightCamera * input.x + forwardCamera * input.y + Vector3.up * player.transform.forward.y).normalized,
            player.RotationSpeed * Mathf.Deg2Rad, 1f);
    }

    private void UpdateAnimator()
    {
        animator.SetFloat(speedX, input.x * player.Speed);
        animator.SetFloat(speedY, input.y * player.Speed);
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
}
