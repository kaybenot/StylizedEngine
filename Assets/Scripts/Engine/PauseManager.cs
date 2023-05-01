using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PauseManager : IPauseManager
{
    public Action<bool> OnPauseChanged { get; set; }

    [Inject] private IInputManager inputManager;
    
    public void Pause()
    {
        Time.timeScale = 0f;
        inputManager.InputBlocked = true;
        OnPauseChanged?.Invoke(true);
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        inputManager.InputBlocked = false;
        OnPauseChanged?.Invoke(false);
    }
}
