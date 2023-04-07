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
        var forward = mainCamera.transform.forward;
        forward.z = 0f;
        forward.Normalize();

        input *= new Vector2(forward.x, forward.z);
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
