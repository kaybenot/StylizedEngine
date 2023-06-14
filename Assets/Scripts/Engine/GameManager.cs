using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : Singleton<GameManager>
{
    public Action OnGameReady
    {
        get => onGameReady;
        set
        {
            if (readyCalled)
                value?.Invoke();
            else
                onGameReady += value;
        }
    }

    public Action OnPlayerSpawn
    {
        get => onPlayerSpawn;
        set
        {
            if (playerSpawned)
                value?.Invoke();
            else
                onPlayerSpawn += value;
        }
    }

    public bool GameStarted => readyCalled;
    public bool PlayerSpawned => playerSpawned;

    private Action onGameReady;
    private Action onPlayerSpawn;
    private bool readyCalled = false;
    private bool playerSpawned;

    protected override void Awake()
    {
        base.Awake();

        onGameReady += OnGameReadyFunction;
    }

    private void OnGameReadyFunction()
    {
        readyCalled = true;
    }
}
