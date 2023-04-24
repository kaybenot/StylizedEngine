using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandProcessor
{
    void Push(string command);
    void ProcessCommand();
    void AddListener(ICommandListener listener);
    bool HasPendingCommands();
}
