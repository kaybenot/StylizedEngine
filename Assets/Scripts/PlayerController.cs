using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private Player player;
    private Vector3 input;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            player.Move(Vector3.zero);
            input = Vector3.zero;
            return;
        }
        
        if (!context.performed)
            return;

        var moveInput = context.ReadValue<Vector2>();
        input = new Vector3(moveInput.x, 0f, moveInput.y);
        player.Move(input);
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            player.Running = true;
            player.Move(input);
        }
        if (context.canceled)
        {
            player.Running = false;
            player.Move(input);
        }
    }
}
