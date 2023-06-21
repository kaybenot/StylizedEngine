using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour, IChunk
{
    [field: SerializeField] public Vector2 Position { get; private set; }
    [field: SerializeField] public Bounds Bounds { get; private set; }
    [field: SerializeField] public float Width { get; private set; }
    public bool Active { get; private set; } = true;
    public List<IChunkComponent> ChunkComponents { get; } = new();
    public List<IStaticWorldObject> StaticObjects { get; } = new();

    public void Initialize()
    {
        AddChildChunkComponents();

        foreach (var component in ChunkComponents)
            component.Initialize(this);
    }
    
    public void Create(Vector2 position, float width)
    {
        Position = position;
        Width = width;
        Bounds = new Bounds(new Vector3(position.x + width / 2f, 0f, position.y + width / 2f),
            new Vector3(width, 1000f, width));
    }

    public void BindStaticObject(IStaticWorldObject obj)
    {
        StaticObjects.Add(obj);
        
        // Deactivate bound object - now is handled by chunk
        obj.Deactivate();
    }

    public void ClearStaticObjects()
    {
        StaticObjects.Clear();
    }

    public void Activate()
    {
        gameObject.SetActive(true);

        foreach (var obj in StaticObjects)
            obj.Activate();
        
        foreach (var component in ChunkComponents)
            component.ChunkLoaded();

        Active = true;
    }

    public void Deactivate()
    {
        foreach (var component in ChunkComponents)
            component.ChunkUnloaded();

        foreach (var obj in StaticObjects)
            obj.Deactivate();
        
        gameObject.SetActive(false);
        Active = false;
    }

    public void RegisterComponent(IChunkComponent component)
    {
        ChunkComponents.Add(component);
    }

    public void UnregisterComponent(IChunkComponent component)
    {
        if (ChunkComponents.Contains(component))
            ChunkComponents.Remove(component);
    }

    private void AddChildChunkComponents()
    {
        foreach (var component in GetComponentsInChildren<IChunkComponent>())
            RegisterComponent(component);
    }
}
