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
    /// Public workaround for EditorWindow.
    /// </summary>
    [Inject] public ISession Session;
    [Inject] private ICommandProcessor commandProcessor;

    private void Start()
    {
        InitializeEngine();
    }

    private void InitializeEngine()
    {
        FindAndAddCommandListeners();
        
        Session.New();
        Session.Initialize();
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
