using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public interface ICommandProcessor
{
    void Push(string command);
    /// <summary>
    /// Processes a pending command, if there is any.
    /// </summary>
    /// <returns>Command log, or empty string</returns>
    [NotNull] string ProcessCommand();
    void AddListener(ICommandListener listener);
    bool HasPendingCommands();
}
