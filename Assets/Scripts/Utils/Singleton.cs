using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

public abstract class Singleton<T> : Singleton where T : MonoBehaviour
{
    [CanBeNull] private static T instance;
    
    private static GameObject s_gameObject;
    
    protected virtual void Awake()
    {
        s_gameObject = gameObject;
    }

    [NotNull]
    public static T Instance
    {
        get
        {
            if (instance)
                return instance;

            if (s_gameObject)
            {
                instance = s_gameObject.GetComponent<T>();
                if (!instance)
                    instance = s_gameObject.AddComponent<T>();
                return instance!;
            }
            
            var objs = FindObjectsOfType<T>();
            if (objs.Length > 0)
            {
                Debug.LogError($"Multiple {nameof(Singleton)}<{typeof(T)}>! Remove additional ones.");
                return objs[0];
            }
            
            if (!s_gameObject)
                instance = new GameObject($"{nameof(Singleton)}<{typeof(T)}>").AddComponent<T>();
            
            return instance!;
        }
    }

    public static bool HasInstance
    {
        get
        {
            if (instance)
                return true;
        
            if (s_gameObject)
            {
                instance = s_gameObject.GetComponent<T>();
                if (instance)
                    return true;
            }
        
            var objs = FindObjectsOfType<T>();
            if (objs.Length > 0)
                return true;

            return false;
        }
    }
}

public abstract class Singleton : MonoBehaviour
{
}
