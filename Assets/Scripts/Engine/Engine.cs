using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Engine : IEngine
{
    public bool Loaded { get; private set; }
    public bool GameLoaded { get; private set; }
    
    [Inject] private ICommandProcessor processor;
    [Inject] private ISceneManager sceneManager;
    [Inject] private IInputManager inputManager;
    [Inject] private IUIManager uiManager;
    [Inject] private IPauseManager pauseManager;
    [Inject] private IWorldManager worldManager;
    
    public async UniTask Load()
    {
        if (Loaded)
        {
            Debug.LogError("Tried to load Engine when there was an already loaded one!");
            return;
        }
        
        // Load UI
        await sceneManager.LoadSceneAdditive(1, null);
        
        // Load Menu
        await sceneManager.LoadSceneAdditive(2, null, true);
        
        uiManager.ShowScreen(UIScreen.Menu);

        Loaded = true;
    }

    public async UniTask Unload()
    {
        if (GameLoaded)
            await UnloadGame(null);

        // Unload Menu scene - unloading order is based on load order stack
        await sceneManager.UnloadActiveScene(null);
        
        // Unload UI scene
        await sceneManager.UnloadActiveScene(null);
        
        Loaded = false;
    }

    public UniTask LoadGame()
    {
        if (!Loaded)
        {
            Debug.LogError("Tried to load Game when there is no Engine loaded!");
            return UniTask.CompletedTask;
        }
        
        if (GameLoaded)
        {
            Debug.LogError("Tried to load Game when it has been already loaded and was not unloaded!");
            return UniTask.CompletedTask;
        }

        var worldContainer = worldManager.GetWorldDataContainer();
        if (worldContainer.Worlds.Count > 0 && worldContainer.Worlds[0] != null)
            worldManager.LoadWorld(worldContainer.Worlds[0]);

        GameManager.Instance.OnGameReady?.Invoke();
        GameLoaded = true;
        return UniTask.CompletedTask;
    }

    public async UniTask UnloadGame(IProgress<float> progress)
    {
        processor.Reset();
        inputManager.Reset();
        
        worldManager.UnloadWorld();

        // Unload Main scene
        await sceneManager.LoadSceneAdditive(2, progress, true, true);
        
        uiManager.HideAllWindows();
        pauseManager.ForceUnpause();
        
        GameLoaded = false;
    }

    public void ProcessPendingCommands()
    {
        if (!GameLoaded)
            return;
        
        while (processor.HasPendingCommands())
        {
            var log = processor.ProcessCommand();
            if (log != "")
                uiManager.LogConsole(log);
        }
    }
}
