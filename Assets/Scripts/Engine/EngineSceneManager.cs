using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EngineSceneManager : ISceneManager
{
    public async UniTask LoadSceneAddative(int index, IProgress<float> progress, bool swapActive = false)
    {
        var asyncOp = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        while (!asyncOp.isDone)
        {
            await UniTask.Yield();
            progress.Report(swapActive ? asyncOp.progress / 2f : asyncOp.progress);
        }

        if (!swapActive)
            return;
        
        var asyncOpUnload = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        while (!asyncOpUnload.isDone)
        {
            await UniTask.Yield();
            progress.Report(0.5f + asyncOpUnload.progress / 2f);
        }
    }
}
