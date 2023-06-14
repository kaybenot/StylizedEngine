using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

/// <summary>
/// This class takes care of all engine flow.
/// </summary>
public class Platform : Singleton<Platform>
{
    /// <summary>
    /// Should not be used in other files, just inject it.
    /// This field is public because it is a workaround for EditorWindow.
    /// </summary>
    [Inject] public ISession Session;
    [Inject] private IEngine engine;

    private async void Start()
    {
        await engine.Load();
    }

    private void Update()
    {
        engine.ProcessPendingCommands();
    }

    public async void StartGame()
    {
        await engine.LoadGame();
    }
}
