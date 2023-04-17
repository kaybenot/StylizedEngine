using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EngineSceneManager : ISceneManager
{
    public async UniTask LoadSceneAddative(int index, bool swapActive = false)
    {
        var asyncOp = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        await UniTask.WaitUntil(() => asyncOp.isDone);

        if (!swapActive)
            return;
        
        var asyncOpUnload = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        await UniTask.WaitUntil(() => asyncOpUnload.isDone);
    }
}
