using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ISceneManager
{
    /// <summary>
    /// Loads an additional scene async.
    /// </summary>
    /// <param name="index">Scene build index</param>
    /// <param name="swapActive">If true, unloads currently active scene</param>
    /// <returns>UniTask with current progress</returns>
    UniTask LoadSceneAddative(int index, bool swapActive = false);
}
