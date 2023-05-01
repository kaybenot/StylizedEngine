using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

public interface ISceneManager
{
    /// <summary>
    /// Loads an additional scene async.
    /// </summary>
    /// <param name="index">Scene build index</param>
    /// <param name="progress">Progress indicator</param>
    /// <param name="setAsActive">Marks scene as active when true</param>
    /// <param name="swapActive">If true, unloads currently active scene</param>
    /// <returns>UniTask with current progress</returns>
    UniTask LoadSceneAdditive(int index, [CanBeNull] IProgress<float> progress, bool setAsActive = false, bool swapActive = false);
    UniTask UnloadActiveScene(IProgress<float> progress);
}
