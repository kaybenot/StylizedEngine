using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MenuUI : MonoBehaviour
{
    [Header("Button setup")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;

    [Header("Menu settings")]
    [SerializeField] private int startScene = 3;

    [Inject] private ISceneManager sceneManager;
    
    private void Start()
    {
        startButton.onClick.AddListener(OnStartPressed);
        settingsButton.onClick.AddListener(OnSettingsPressed);
        exitButton.onClick.AddListener(OnExitPressed);
    }

    private async void OnStartPressed()
    {
        var progress = new Progress<float>();
        var task = sceneManager.LoadSceneAddative(startScene, progress, true, true);

        while (task.Status != UniTaskStatus.Succeeded)
        {
            await UniTask.Yield();
            // TODO: Raport progress
        }

        await UniTask.Create(() =>
        {
            Platform.Instance.StartGame();
            return UniTask.CompletedTask;
        });
        
        UIManager.Instance.HideScreen();
    }

    private void OnSettingsPressed()
    {
        throw new NotImplementedException("Settings not implemented!");
    }

    private void OnExitPressed()
    {
        Application.Quit();
    }
}
