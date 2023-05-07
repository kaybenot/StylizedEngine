using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

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

public enum UIPopup
{
    // To be implemented
}

public interface IUIManager
{
    /// <summary>
    /// Current UI screen.
    /// </summary>
    UIScreen CurrentScreen { get; }
    IEnumerable<UIWindow> OpenWindows { get; }
    
    /// <summary>
    /// Shows a UI screen. Hides actually displayed if there is one.
    /// </summary>
    /// <param name="screen">Screen to be displayed</param>
    void ShowScreen(UIScreen screen);
    /// <summary>
    /// Hides currently displayed screen if there is one.
    /// </summary>
    void HideScreen();
    /// <summary>
    /// Toggles window visibility.
    /// </summary>
    /// <param name="window">Window to be shown/hidden</param>
    /// <returns>Current state of the window (after toggling)</returns>
    bool ToggleWindow(UIWindow window);
    void HideAllWindows();
    /// <summary>
    /// Reports progress to progress bar.
    /// </summary>
    /// <param name="val">0-1 range of progress</param>
    /// <param name="text">Text to be displayed on progress bar,
    /// null means no text update (previously is going to be displayed)</param>
    void ReportProgress(float val, string text = null);
    /// <summary>
    /// Logs to console.
    /// </summary>
    /// <param name="log">Text to be logged (rich text supported)</param>
    void LogConsole(string log);
    
    void RegisterScreen(UIScreen screen, GameObject go);
    void RegisterWindow(UIWindow window, GameObject go);
}
