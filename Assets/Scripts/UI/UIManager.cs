using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

public enum UIScreen
{
    Menu,
    Pause,
    None
}

public enum UIWindow
{
    Console,
    Loading,
    Debug
}

[RequireComponent(typeof(PlayerInput))]
public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject console;
    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject debug;

    [Inject] private IPauseManager pauseManager;

    private GameObject lastScreen = null;
    private Console consoleComponent;
    private Loading loadingComponent;
    private readonly List<UIWindow> openWindows = new();

    protected override void Awake()
    {
        base.Awake();

        consoleComponent = console.GetComponent<Console>();
        if (consoleComponent == null)
            Debug.LogError("UIManager did not find Console!");

        loadingComponent = loading.GetComponent<Loading>();
        if (loadingComponent == null)
            Debug.LogError("UIManager did not find Loading!");
    }

    public void ShowScreen(UIScreen screen)
    {
        if (lastScreen != null)
            lastScreen.SetActive(false);
        
        switch (screen)
        {
            case UIScreen.Menu:
                menu.SetActive(true);
                lastScreen = menu;
                break;
            case UIScreen.None:
                // Show empty screen
                lastScreen = null;
                break;
            case UIScreen.Pause:
                pauseMenu.SetActive(true);
                lastScreen = pauseMenu;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(screen), screen, null);
        }
    }

    public void HideScreen()
    {
        ShowScreen(UIScreen.None);
    }

    public void ToggleWindow(UIWindow window)
    {
        var contains = openWindows.Contains(window);
        
        switch (window)
        {
            case UIWindow.Console:
                console.SetActive(!contains);
                if (!contains) // opened
                    pauseManager.Pause();
                else // closed
                    pauseManager.Unpause();
                break;
            case UIWindow.Loading:
                loading.SetActive(!contains);
                break;
            case UIWindow.Debug:
                debug.SetActive(!contains);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(window), window, null);
        }

        if (contains)
            openWindows.Remove(window);
        else
            openWindows.Add(window);
    }

    public void ReportProgress(float val, string text = null)
    {
        loadingComponent.ReportProgress(val, text);
    }

    public void LogConsole(string log)
    {
        consoleComponent.LogConsole(log);
    }

    public void ToggleConsole(InputAction.CallbackContext context)
    {
        if (context.started && GameManager.HasInstance && GameManager.Instance.GameStarted)
            ToggleWindow(UIWindow.Console);
    }

    public void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (context.started && GameManager.HasInstance && GameManager.Instance.GameStarted)
            ShowScreen(lastScreen == pauseMenu ? UIScreen.None : UIScreen.Pause);
    }

    public void ToggleDebug(InputAction.CallbackContext context)
    {
        if (context.started && GameManager.HasInstance && GameManager.Instance.GameStarted)
            ToggleWindow(UIWindow.Debug);
    }
}
