using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

/// <summary>
/// Class defining objects bound with session.
/// The objects must be prefabs!
/// Objects can be created from data, or create data on their own when placed on scene.
/// </summary>
public class SessionObject : ZenAutoInjecter
{
    [field: SerializeField] public SerializableGuid ID { get; set; }
    [SerializeField] private GameObject prefab;

    private void OnValidate()
    {
        if (ID == Guid.Empty)
            ID = Guid.NewGuid();
    }
    
    /// <summary>
    /// Creates object data if there is not a one.
    /// </summary>
    /// <returns>Default data, this can be an inheritor of ObjectData</returns>
    public virtual ObjectData CreateDefaultData()
    {
        return new ObjectData(new GameObject("Unnamed Session Object"));
    }

    /// <summary>
    /// Called during session initialization or instantly when during game.
    /// </summary>
    public virtual void OnSessionInitialized()
    {
    }

    /// <summary>
    /// Undefined behaviour if SessionObject is not a prefab!
    /// </summary>
    /// <returns>Prefab of this SessionObject</returns>
    protected GameObject GetPrefab()
    {
        if (prefab == null)
            Debug.LogError($"{gameObject.name} does not have an assigned prefab! GetPrefab is going to return null.");
        return prefab;
    }
}
