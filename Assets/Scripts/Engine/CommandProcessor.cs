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
        commandQueue.Enqueue(command.ToLower());
    }

    public string ProcessCommand()
    {
        if (commandQueue.Count <= 0)
            return "";
        
        var cmd = ReinterprateCommand(commandQueue.Dequeue());

        if (!cmd.parseSuccess)
            return "Failed to parse command. A proper command should look like: listener.command param0 param1...";
        
        var _listeners = listeners.Where(l => l.Name == cmd.listenerName).ToArray();
        if (!_listeners.Any())
            return "There is no listener to that command.";
        
        var log = "";
        foreach (var listener in _listeners)
            log += $"{listener.ProcessCommand(cmd.listenerCommand)}\n";

        if (log.EndsWith('\n'))
            log.Remove(log.Length - 1);

        return log;
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

    private static (bool parseSuccess, string listenerName, string listenerCommand) ReinterprateCommand(string command)
    {
        var parseSuccess = false;
        var split = command.Split('.', 2);
        if (split.Length == 2)
            parseSuccess = true;
        
        return parseSuccess ? (true, split[0], split[1]) : (false, null, null);
    }
}
