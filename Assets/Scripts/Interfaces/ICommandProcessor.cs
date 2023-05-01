using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// This interface should be used under Game layer, not Engine.
/// When reloading the game, it should reset!
/// </summary>
public interface ICommandProcessor
{
    void Reset();
    void Push(string command);
    /// <summary>
    /// Processes a pending command, if there is any.
    /// </summary>
    /// <returns>Command log, or empty string</returns>
    [NotNull] string ProcessCommand();
    void AddListener(ICommandListener listener);
    bool HasPendingCommands();
}
