using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;

    [Inject] private IPauseManager pauseManager;
    [Inject] private IEngine engine;
    [Inject] private IUIManager uiManager;

    private void Awake()
    {
        resumeButton.onClick.AddListener(Resume);
        menuButton.onClick.AddListener(BackToMenu);
    }

    private void OnEnable()
    {
        pauseManager.Pause();
    }

    private void OnDisable()
    {
        pauseManager.Unpause();
    }

    private void Resume()
    {
        uiManager.ShowScreen(UIScreen.None);
    }

    private async void BackToMenu()
    {
        uiManager.ToggleWindow(UIWindow.Loading);
        uiManager.ReportProgress(0f, "Unloading game");
        await engine.UnloadGame(new Progress<float>((progress) =>
            uiManager.ReportProgress(progress)));

        if (uiManager.OpenWindows.Contains(UIWindow.Loading))
            uiManager.ToggleWindow(UIWindow.Loading);
        uiManager.ShowScreen(UIScreen.Menu);
    }
}
