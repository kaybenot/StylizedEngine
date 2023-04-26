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
        commandField.onSubmit.AddListener((line) => processor.Push(line));
    }
}
