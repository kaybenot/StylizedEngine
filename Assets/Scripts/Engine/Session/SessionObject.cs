using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SessionObject : MonoBehaviour, ISessionObject
{
    [field: SerializeField] public SerializableGuid ID { get; set; }

    [Inject] private ISession session;

    private void OnValidate()
    {
        if (ID == Guid.Empty)
            ID = Guid.NewGuid();
    }

    public virtual void OnSessionInitialized()
    {
        
    }

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
