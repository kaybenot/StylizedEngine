using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class Console : MonoBehaviour
{
    [SerializeField] private TMP_Text consoleOutput;
    [SerializeField] private TMP_InputField commandField;

    [Inject] private ICommandProcessor processor;

    private void Awake()
    {
        commandField.onSubmit.AddListener(OnCommandSent);
    }

    private async void OnEnable()
    {
        await UniTask.Yield(); // Strangely you have to wait one frame after enabling input field to activate it
        
        commandField.ActivateInputField();
    }

    public void LogConsole(string log)
    {
        consoleOutput.text += log + '\n';
    }

    public void ClearConsole()
    {
        consoleOutput.text = "";
    }

    private void OnCommandSent(string command)
    {
        processor.Push(command);
        commandField.text = "";
    }
}
