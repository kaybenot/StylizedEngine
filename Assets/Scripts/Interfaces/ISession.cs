using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

public interface ISession
{
    void New();
    /// <summary>
    /// Loads session.
    /// </summary>
    /// <param name="relativeDirectory">Relative directory to saves folder</param>
    /// <param name="fileName">File name to be saved</param>
    /// <returns>True if loaded successfully</returns>
    bool Load(string relativeDirectory, string fileName);
    /// <summary>
    /// Saves session
    /// </summary>
    /// <param name="relativeDirectory">Relative directory to saves folder</param>
    /// <param name="fileName">File name to be saved</param>
    /// <returns>True if loaded successfully</returns>
    bool Save(string relativeDirectory, string fileName);
    [CanBeNull] T GetData<T>(Guid id) where T : class;
    bool ContainsData(Guid id);
    bool TryAddData(ObjectData data);
    /// <summary>
    /// Spawns object using provided data.
    /// </summary>
    /// <param name="data">Data of object to be spawned</param>
    /// <param name="parent">Parent transform of spawned object</param>
    /// <returns>Spawned object or null</returns>
    [CanBeNull] SessionObject TrySpawnObject(ObjectData data, Transform parent = null);
    IEnumerable<ObjectData> FindNotSpawnedObjects();
    IEnumerable<SessionObject> GetAllSessionObjects();
    [CanBeNull] SessionObject GetSessionObject(Guid id);
}
