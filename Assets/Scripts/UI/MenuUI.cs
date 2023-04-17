using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuUI : MonoBehaviour
{
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

    private void StartButton()
    {
        
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
