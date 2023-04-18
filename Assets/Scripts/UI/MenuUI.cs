using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Zenject;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private int startScene = 1;

    [Inject] private ISceneManager sceneManager;
    
    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var startButton = root.Q<Button>("play");
        var settingsButton = root.Q<Button>("settings");
        var exitButton = root.Q<Button>("exit");

        startButton.clicked += StartButton;
        settingsButton.clicked += SettingsButton;
        exitButton.clicked += ExitButton;
    }

    private async void StartButton()
    {
        var progress = new Progress<float>();
        var task = sceneManager.LoadSceneAddative(startScene, progress, true);

        while (task.Status != UniTaskStatus.Succeeded)
        {
            await UniTask.Yield();
            // TODO: Raport progress
        }
    }

    private void SettingsButton()
    {
        throw new NotImplementedException("Settings not implemented!");
    }

    private void ExitButton()
    {
        Application.Quit();
    }
}
