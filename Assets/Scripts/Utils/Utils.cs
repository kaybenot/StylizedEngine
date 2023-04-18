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
        var rootObjs = scene.GetRootGameObjects();
        var types = rootObjs.Select(obj => obj.GetComponentInChildren<T>()).ToArray();
        return types.Any() ? types.First() : null;;
    }
    
    public static List<T> FindTypesOnScene<T>(Scene scene) where T : class
    {
        var rootObjs = scene.GetRootGameObjects();
        return rootObjs.Select(obj => obj.GetComponentInChildren<T>()).ToList();
    }
}
