using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utils
{
    [CanBeNull] public static T FindTypeOnScene<T>(Scene scene) where T : class
    {
        var types = FindTypesOnScene<T>(scene);
        return types.Count > 0 ? types.First() : null;
    }
    
    public static List<T> FindTypesOnScene<T>(Scene scene) where T : class
    {
        var rootObjs = scene.GetRootGameObjects();
        var types = new List<T>();
        foreach (var obj in rootObjs)
            types.AddRange(obj.GetComponentsInChildren<T>());
        return types;
    }

    public static (string[] commandStack, string[] argumentStack) ParseCommand(string command)
    {
        var commandStackAndArgumentStack = command.Split(' ', 2);
        var commandStack = commandStackAndArgumentStack[0].Split('.');
        var argumentStack = commandStackAndArgumentStack[1].Split(' ');
        return (commandStack, argumentStack);
    }
}
