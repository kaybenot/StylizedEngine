using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Globals : Singleton<Globals>
{
    public string SavePath { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        
        SavePath = Application.persistentDataPath;
    }
}
