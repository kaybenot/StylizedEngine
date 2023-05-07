using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;


public class UIManager : IUIManager
{
    public UIScreen CurrentScreen { get; private set; } = UIScreen.None;
    public IEnumerable<UIWindow> OpenWindows =>
        from window in windows where window.Value.activeSelf select window.Key;
    
    private readonly Dictionary<UIWindow, GameObject> windows = new();
    private readonly Dictionary<UIScreen, GameObject> screens = new();
    
    [Inject] private IPauseManager pauseManager;

    private Console consoleComponent;
    private Loading loadingComponent;

    public void ShowScreen(UIScreen screen)
    {
        if (screen != UIScreen.None && !screens.ContainsKey(screen))
        {
            Debug.LogError("Tried to show screen which has not been registered yet!");
            return;
        }
        
        if (CurrentScreen != UIScreen.None)
            screens[CurrentScreen].SetActive(false);

        if (screen != UIScreen.None)
            screens[screen].SetActive(true);
        CurrentScreen = screen;
    }

    public void HideScreen()
    {
        ShowScreen(UIScreen.None);
    }

    public bool ToggleWindow(UIWindow window)
    {
        if (!windows.ContainsKey(window))
        {
            Debug.LogError("Tried to show screen which has not been registered yet!");
            return false;
        }

        var nextState = !windows[window].activeSelf;
        windows[window].SetActive(nextState);

        switch (window)
        {
            case UIWindow.Console:
            {
                if (nextState)
                    pauseManager.Pause();
                else
                    pauseManager.Unpause();
                break;
            }
            default:
                break;
        }
        
        return nextState;
    }

    public void HideAllWindows()
    {
        foreach (var (window, windowObj) in windows)
        {
            if (windowObj.activeSelf)
                windowObj.SetActive(false);
        }
    }

    public void ReportProgress(float val, string text = null)
    {
        loadingComponent.ReportProgress(val, text);
    }

    public void LogConsole(string log)
    {
        consoleComponent.LogConsole(log);
    }

    public void RegisterScreen(UIScreen screen, GameObject go)
    {
        if (screens.ContainsKey(screen))
        {
            Debug.LogError("Tried to register a screen which has already been registered!");
            return;
        }
        
        screens.Add(screen, go);
        
        switch (screen)
        {
            case UIScreen.Menu:
                break;
            case UIScreen.Pause:
                break;
            case UIScreen.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(screen), screen, null);
        }
    }

    public void RegisterWindow(UIWindow window, GameObject go)
    {
        if (windows.ContainsKey(window))
        {
            Debug.LogError("Tried to register a window which has already been registered!");
            return;
        }
        
        windows.Add(window, go);
        
        switch (window)
        {
            case UIWindow.Console:
                consoleComponent = go.GetComponent<Console>();
                break;
            case UIWindow.Loading:
                loadingComponent = go.GetComponent<Loading>();
                break;
            case UIWindow.Debug:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(window), window, null);
        }
    }
}
