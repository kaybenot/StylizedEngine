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

    public bool GameStarted => readyCalled;

    private Action onGameReady;
    private bool readyCalled = false;

    protected override void Awake()
    {
        base.Awake();
        
        onGameReady += () => readyCalled = true;
    }
}
