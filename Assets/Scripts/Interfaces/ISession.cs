using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

public interface ISession
{
    Guid ID { get; }
    bool Initialized { get; }
    Action OnInitialized { get; set; }
    
    void New();
    /// <summary>
    /// Loads session.
    /// </summary>
    /// <param name="relativeDirectory">Relative directory to saves folder</param>
    /// <param name="fileName">File name to be saved</param>
    /// <returns>True if loaded successfully</returns>
    bool Load(string relativeDirectory, string fileName);
    void Unload();
    /// <summary>
    /// Saves session
    /// </summary>
    /// <param name="relativeDirectory">Relative directory to saves folder</param>
    /// <param name="fileName">File name to be saved</param>
    /// <returns>True if loaded successfully</returns>
    bool Save(string relativeDirectory, string fileName);
    /// <summary>
    /// Initializes session after creating or loading one.
    /// Creates objects from data.
    /// After initializing, added SessionObjects are going to be initialized when added.
    /// </summary>
    void Initialize();
    [CanBeNull] T GetData<T>(Guid id) where T : class;
    bool ContainsData(Guid id);
    int GetDataCount();
    /// <summary>
    /// Adds data to the session.
    /// </summary>
    /// <param name="data">Data to be added</param>
    /// <returns>Created object from data or null (when data existed)</returns>
    [CanBeNull] SessionObject TryAddData(ObjectData data);
    IEnumerable<SessionObject> GetAllSessionObjects();
    [CanBeNull] SessionObject GetSessionObject(Guid id);
}
