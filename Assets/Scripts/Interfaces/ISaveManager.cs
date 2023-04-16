using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public interface ISaveManager
{
    /// <summary>
    /// Saves file at given directory path. Overwrites it if exists.
    /// Do not provide extensions.
    /// </summary>
    /// <param name="relativeDirectory">Relative directory path</param>
    /// <param name="fileName">File name without extension</param>
    /// <param name="objectToSave">Serializable object to be saved</param>
    /// <returns>True if object has been saved</returns>
    bool SaveJson(string fileName, object objectToSave, string relativeDirectory = null);
    /// <summary>
    /// Loads file and converts it to given type.
    /// Do not provide extensions.
    /// </summary>
    /// <param name="relativeDirectory">Relative directory path</param>
    /// <param name="fileName">File name without extension</param>
    /// <typeparam name="T">Object type</typeparam>
    /// <returns>Object or null</returns>
    [CanBeNull] T LoadJson<T>(string fileName, string relativeDirectory = null);
}
