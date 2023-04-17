using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

[Serializable]
public class Session : ISession
{
    public SerializableGuid ID;

    [SerializeField] private List<ObjectData> datas;
    private SessionObject[] sessionObjectsCache;

    [Inject] private ISaveManager saveManager;
    
    public void New()
    {
        ID = Guid.NewGuid();
        datas = new List<ObjectData>();
    }

    public bool Load(string directory, string fileName)
    {
        var loadedSession = saveManager.LoadJson<Session>(fileName, directory);
        if (loadedSession == null)
            return false;

        ID = loadedSession.ID;
        datas = loadedSession.datas;

        return true;
    }

    public bool Save(string directory, string fileName)
    {
        return saveManager.SaveJson(fileName, this, directory);
    }

    public T GetData<T>(Guid id) where T : class
    {
        var index = datas.FindIndex(obj => obj.ID == id);
        if (index < 0)
            return null;

        return (T)(object)datas[index];
    }

    public bool ContainsData(Guid id)
    {
        return datas.FindIndex(obj => obj.ID == id) > 0;
    }

    public bool TryAddData(ObjectData data)
    {
        if (ContainsData(data.ID))
            return false;
        
        datas.Add(data);
        return true;
    }

    public SessionObject TrySpawnObject(ObjectData data, Transform parent = null)
    {
        var obj =  Object.Instantiate(data.Prefab, parent).GetComponent<SessionObject>();
        obj.ID = data.ID;

        return obj;
    }

    public IEnumerable<ObjectData> FindNotSpawnedObjects()
    {
        var sceneObjs = Object.FindObjectsOfType<SessionObject>();
        return from data in datas where !sceneObjs.Select(obj => obj.ID).Contains(data.ID) select data;
    }

    public IEnumerable<SessionObject> GetAllSessionObjects()
    {
        sessionObjectsCache = Object.FindObjectsOfType<SessionObject>();
        return sessionObjectsCache;
    }

    public SessionObject GetSessionObject(Guid id)
    {
        var cacheIndex = sessionObjectsCache.ToList().FindIndex(obj => obj.ID == id);
        if (cacheIndex < 0)
        {
            GetAllSessionObjects();
            var index = sessionObjectsCache.ToList().FindIndex(obj => obj.ID == id);
            if (index < 0)
                return null;
        }
        return sessionObjectsCache[cacheIndex];
    }
}
