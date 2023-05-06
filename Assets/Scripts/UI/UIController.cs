using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(PlayerInput))]
public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject console;
    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject debug;

    [Inject] private IUIManager uiManager;

    private void Awake()
    {
        uiManager.RegisterScreen(UIScreen.Menu, menu);
        uiManager.RegisterScreen(UIScreen.Pause, pauseMenu);
        
        uiManager.RegisterWindow(UIWindow.Console, console);
        uiManager.RegisterWindow(UIWindow.Loading, loading);
        uiManager.RegisterWindow(UIWindow.Debug, debug);
    }

    public void ToggleConsole(InputAction.CallbackContext context)
    {
        if (context.started && GameManager.HasInstance && GameManager.Instance.GameStarted)
            uiManager.ToggleWindow(UIWindow.Console);
    }

    public void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (context.started && GameManager.HasInstance && GameManager.Instance.GameStarted)
            uiManager.ShowScreen(uiManager.CurrentScreen == UIScreen.Pause ? UIScreen.None : UIScreen.Pause);
    }

    public void ToggleDebug(InputAction.CallbackContext context)
    {
        if (context.started && GameManager.HasInstance && GameManager.Instance.GameStarted)
            uiManager.ToggleWindow(UIWindow.Debug);
    }
}
