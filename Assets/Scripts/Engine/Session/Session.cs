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
    [SerializeField] public Guid ID { get; private set; }

    [SerializeField] private List<ObjectData> datas;
    [SerializeField] private SerializableGuid id;
    private SessionObject[] sessionObjectsCache;

    [Inject] private ISaveManager saveManager;

    public bool Initialized { get; private set; } = false;
    public Action OnInitialized { get; set; }

    public void New()
    {
        ID = Guid.NewGuid();
        datas = new List<ObjectData>();
        
        Initialize();
    }

    public bool Load(string directory, string fileName)
    {
        var loadedSession = saveManager.LoadJson<Session>(fileName, directory);
        if (loadedSession == null)
            return false;

        ID = loadedSession.ID;
        datas = loadedSession.datas;
        
        Initialize();

        return true;
    }

    public void Unload()
    {
        ID = Guid.Empty;
        datas = null;
        OnInitialized = null;
        Initialized = false;
        sessionObjectsCache = null;
    }

    public bool Save(string directory, string fileName)
    {
        return saveManager.SaveJson(fileName, this, directory);
    }

    public void Initialize()
    {
        foreach (var sessionObject in GetAllSessionObjects())
            if (!ContainsData(sessionObject.ID))
                TryAddData(sessionObject.CreateDefaultData());
        
        foreach (var data in datas)
            CreateObjectFromData(data);

        Initialized = true;
        OnInitialized?.Invoke();
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

    public int GetDataCount()
    {
        return datas.Count;
    }

    public SessionObject TryAddData(ObjectData data)
    {
        if (ContainsData(data.ID))
            return null;
        
        datas.Add(data);

        return CreateObjectFromData(data);
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

    private SessionObject CreateObjectFromData(ObjectData data)
    {
        var obj = Object.Instantiate(data.Prefab, data.Position, data.Rotation, data.Parent).GetComponent<SessionObject>();
        obj.ID = data.ID;
        
        obj.OnSessionInitialized();

        return obj;
    }
}
