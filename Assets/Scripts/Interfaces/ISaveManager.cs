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
    /// <param name="directory">Directory path</param>
    /// <param name="fileName">File name</param>
    /// <param name="objectToSave">Serializable object to be saved</param>
    /// <returns></returns>
    bool SaveJson(string directory, string fileName, object objectToSave);
    /// <summary>
    /// Loads file and converts it to given type.
    /// Do not provide extensions.
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="fileName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [CanBeNull] T LoadJson<T>(string directory, string fileName);
}
