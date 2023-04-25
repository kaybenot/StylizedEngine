using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum UIScreen
{
    Menu,
    None
}

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject menu;
    [SerializeField] private Canvas canvas;
    [SerializeField] private EventSystem eventSystem;

    private GameObject lastScreen = null;

    public void SetCursorPos(float x, float y)
    {
    }
    
    public void ShowScreen(UIScreen screen)
    {
        if (lastScreen != null)
            lastScreen.SetActive(false);
        
        switch (screen)
        {
            case UIScreen.Menu:
                menu.SetActive(true);
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
}
