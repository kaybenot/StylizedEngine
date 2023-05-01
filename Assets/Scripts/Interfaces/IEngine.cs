using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IEngine
{
    bool Loaded { get; }
    bool GameLoaded { get; }
    
    UniTask Load();
    UniTask Unload();
    UniTask LoadGame();
    UniTask UnloadGame(IProgress<float> progress);
    void ProcessPendingCommands();
}
