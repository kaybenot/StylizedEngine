using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(Player))]
public class PlayerController : ZenAutoInjecter
{
    [Inject] private IInputManager inputManager;

    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();

        GameManager.Instance.OnGameReady += () =>
        {
            inputManager.OnMoveInput += OnMove;
            inputManager.OnRun += OnRun;
        };
    }

    private void OnMove(Vector2 input)
    {
        player.Move(new Vector3(input.x, 0f, input.y));
    }

    private void OnRun(bool running)
    {
        player.Running = running;
    }
}
