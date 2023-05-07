using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PauseManager : IPauseManager
{
    public Action<bool> OnPauseChanged { get; set; }

    public bool Paused => pauseCounter > 0;

    [Inject] private IInputManager inputManager;

    private int pauseCounter;

    public void Pause()
    {
        if (pauseCounter <= 0)
        {
            Time.timeScale = 0f;
            inputManager.InputBlocked = true;
            OnPauseChanged?.Invoke(true);
        }
        pauseCounter++;
    }

    public void Unpause()
    {
        pauseCounter--;
        if (pauseCounter > 0)
            return;
        
        Time.timeScale = 1f;
        inputManager.InputBlocked = false;
        OnPauseChanged?.Invoke(false);
    }

    public void ForceUnpause()
    {
        pauseCounter = 0;
        Time.timeScale = 1f;
        inputManager.InputBlocked = false;
        OnPauseChanged?.Invoke(false);
    }
}
