using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SessionObject : MonoBehaviour, ISessionObject
{
    public Guid ID { get; set; }

    [Inject] private ISession session;

    public bool SpawnFromData(Transform parent = null)
    {
        var data = session.GetData<ObjectData>(ID);
        if (data == null)
            return false;
        
        var obj = Instantiate(data.Prefab, parent);
        obj.AddComponent<SessionObject>();
        
        return true;
    }
}
