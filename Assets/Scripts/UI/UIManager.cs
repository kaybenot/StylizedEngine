using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public enum UIScreen
{
    Menu,
    None
}

public enum UIWindow
{
    Console
}

[RequireComponent(typeof(PlayerInput))]
public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject console;

    private GameObject lastScreen = null;
    private List<UIWindow> openWindows = new();

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
        var contains = openWindows.Contains(UIWindow.Console);
        
        switch (window)
        {
            case UIWindow.Console:
                console.SetActive(!contains);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(window), window, null);
        }

        if (contains)
            openWindows.Remove(window);
        else
            openWindows.Add(window);
    }

    public void ToggleConsole(InputAction.CallbackContext context)
    {
        if (context.started)
            ToggleWindow(UIWindow.Console);
    }
}
