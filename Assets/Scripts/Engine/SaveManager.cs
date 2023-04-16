using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class SaveManager : ISaveManager
{
    public bool SaveJson(string directory, string fileName, object objectToSave)
    {
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        var json = JsonUtility.ToJson(objectToSave, true);
        if (json == null)
            return false;
        
        File.WriteAllText($"{directory}/{fileName}.json", json);
        return true;
    }

    public T LoadJson<T>(string directory, string fileName)
    {
        return JsonUtility.FromJson<T>($"{directory}/{fileName}.json");
    }
}
