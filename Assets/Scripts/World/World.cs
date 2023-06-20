using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour, IWorld
{
    [field: SerializeField] public WorldData WorldData { get; set; }
    public List<IWorldComponent> WorldComponents { get; private set; } = new ();

    private Dictionary<(float x, float z), IChunk> chunks;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        chunks = new Dictionary<(float x, float z), IChunk>();

        AddChildWorldComponents();
        RegisterChunks();

        foreach (var component in WorldComponents)
            component.Initialize(this);
    }
    
    public void Free()
    {
        // Is it really needed?
    }

    public void RegisterComponent(IWorldComponent component)
    {
        WorldComponents.Add(component);
    }

    public void UnregisterComponent(IWorldComponent component)
    {
        if (WorldComponents.Contains(component))
            WorldComponents.Remove(component);
    }

    public IChunk GetChunkAtPosition(float x, float z)
    {
        return chunks.ContainsKey((x, z)) ? chunks[GetChunkCoordinates(x, z)] : null;
    }

    public (float x, float z) GetChunkCoordinates(float x, float z)
    {
        return (Mathf.Floor(x / WorldData.ChunkWidth) * WorldData.ChunkWidth, Mathf.Floor(z / WorldData.ChunkWidth) * WorldData.ChunkWidth);
    }

    private void AddChildWorldComponents()
    {
        foreach (var component in GetComponentsInChildren<IWorldComponent>())
            RegisterComponent(component);
    }

    private void RegisterChunks()
    {
        foreach (var chunk in GetComponentsInChildren<IChunk>())
        {
            chunks.Add((chunk.Position.x, chunk.Position.y), chunk);
            chunk.Initialize();
            chunk.Deactivate();
        }
    }
}
