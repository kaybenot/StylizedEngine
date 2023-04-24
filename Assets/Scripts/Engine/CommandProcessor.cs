using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandProcessor : ICommandProcessor
{
    private readonly Queue<string> commandQueue = new();
    private readonly List<ICommandListener> listeners = new();

    public void Push(string command)
    {
        commandQueue.Enqueue(command);
    }

    public void ProcessCommand()
    {
        if (commandQueue.Count <= 0)
            return;
        
        var cmd = ReinterprateCommand(commandQueue.Dequeue());

        foreach (var listener in listeners.Where(l => l.Name == cmd.listenerName))
            listener.ProcessCommand(cmd.listenerCommand);
    }

    public void AddListener(ICommandListener listener)
    {
        // Currently do not support multiple listeners with the same name (maybe next time)
        if (listeners.Any(l => l.Name == listener.Name))
        {
            Debug.LogError($"Tried to add multiple listeners with the same name: {listener.Name}");
            return;
        }
        
        listeners.Add(listener);
    }

    public bool HasPendingCommands()
    {
        return commandQueue.Count > 0;
    }

    private static (string listenerName, string listenerCommand) ReinterprateCommand(string command)
    {
        var split = command.Split('.', 2);
        return (split[0], split[1]);
    }
}
