using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Platform : Singleton<Platform>
{
    /// <summary>
    /// Should not be used in other files, just inject it.
    /// Public workaround for EditorWindow.
    /// </summary>
    [Inject] public ISession Session;

    private void Start()
    {
        InitializeEngine();
    }

    private void InitializeEngine()
    {
        Session.New();
        Session.Initialize();
    }
}
