using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[Serializable]
public class Chunk
{
    public Terrain Terrain { get; set; }

    [SerializeField] private List<ObjectData> objectDatas = new();

    [Inject] private ISession session;

    public void LoadChunk()
    {
        Terrain.gameObject.SetActive(true);
        
        foreach (var objData in objectDatas)
        {
            session.TryAddData(objData);
        }
    }

    public void UnloadChunk()
    {
        Terrain.gameObject.SetActive(false);
    }

    public bool TryAddData(ObjectData data)
    {
        if (objectDatas.Contains(data))
            return false;

        objectDatas.Add(data);
        return true;
    }
}
