using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Platform : Singleton<Platform>
{
    /// <summary>
    /// Should not be used in other files, just inject it.
    /// This field is public because it is a workaround for EditorWindow.
    /// </summary>
    [Inject] public ISession Session;
    [Inject] private ICommandProcessor commandProcessor;
    [Inject] private ISceneManager sceneManager;

    private void Start()
    {
        InitializeEngine();
    }

    private void Update()
    {
        // Main platform loop
        while (commandProcessor.HasPendingCommands())
        {
            var log = commandProcessor.ProcessCommand();
            if (log != "")
                UIManager.Instance.LogConsole(log);
        }
    }

    public void StartGame()
    {
        FindAndAddCommandListeners();
        
        Session.New();
        Session.Initialize();
    }

    private async void InitializeEngine()
    {
        // Load UI
        await sceneManager.LoadSceneAddative(1, null);
        
        // Load Menu
        await sceneManager.LoadSceneAddative(2, null, true);
        
        UIManager.Instance.ShowScreen(UIScreen.Menu);
    }

    private void FindAndAddCommandListeners()
    {
        var type = typeof(ICommandListener);
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
            .Where(t => type.IsAssignableFrom(t) && !t.IsInterface);

        foreach (var t in types)
            commandProcessor.AddListener((ICommandListener)t);
    }
}
