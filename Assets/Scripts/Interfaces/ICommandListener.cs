using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommandListener
{
    string Name { get; }
    
    /// <summary>
    /// Processes one pending command. Should be called after initialized session!
    /// </summary>
    /// <param name="command">Command to be processed</param>
    void ProcessCommand(string command);
}
