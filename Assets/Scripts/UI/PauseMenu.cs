using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;

    [Inject] private IPauseManager pauseManager;
    [Inject] private IEngine engine;

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
        UIManager.Instance.ShowScreen(UIScreen.None);
    }

    private async void BackToMenu()
    {
        UIManager.Instance.ToggleWindow(UIWindow.Loading);
        UIManager.Instance.ReportProgress(0f, "Unloading game");
        await engine.UnloadGame(new Progress<float>((progress) =>
            UIManager.Instance.ReportProgress(progress)));
        
        UIManager.Instance.ToggleWindow(UIWindow.Loading);
        UIManager.Instance.ShowScreen(UIScreen.Menu);
    }
}
