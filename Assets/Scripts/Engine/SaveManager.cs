using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class SaveManager : ISaveManager
{
    public bool SaveJson(string fileName, object objectToSave, string relativeDirectory = null)
    {
        var relativePath = relativeDirectory == null ? Globals.Instance.SavePath :
            $"{Globals.Instance.SavePath}/{relativeDirectory}";
        
        if (!Directory.Exists(relativePath))
            Directory.CreateDirectory(relativePath);

        var json = JsonUtility.ToJson(objectToSave, true);
        if (json == null)
            return false;
        
        File.WriteAllText($"{relativePath}/{fileName}.json", json);
        return true;
    }

    public T LoadJson<T>(string fileName, string relativeDirectory = null)
    {
        var relativePath = relativeDirectory == null ? Globals.Instance.SavePath : 
            $"{Globals.Instance.SavePath}/{relativeDirectory}";
        
        return JsonUtility.FromJson<T>($"{relativeDirectory}/{fileName}.json");
    }
}
