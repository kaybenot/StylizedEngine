using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
