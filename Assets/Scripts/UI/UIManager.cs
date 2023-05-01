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
    None
}

public enum UIWindow
{
    Console,
    Loading
}

[RequireComponent(typeof(PlayerInput))]
public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject console;
    [SerializeField] private GameObject loading;

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
            default:
                throw new ArgumentOutOfRangeException(nameof(window), window, null);
        }

        if (contains)
            openWindows.Remove(window);
        else
            openWindows.Add(window);
    }

    public void RaportProgress(float val, string text = null)
    {
        loadingComponent.RaportProgress(val, text);
    }

    public void LogConsole(string log)
    {
        consoleComponent.LogConsole(log);
    }

    public void ToggleConsole(InputAction.CallbackContext context)
    {
        if (context.started)
            ToggleWindow(UIWindow.Console);
    }
}
