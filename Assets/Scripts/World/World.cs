using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour, IWorld
{
    [field: SerializeField] public WorldData WorldData { get; set; }

    private Dictionary<(float x, float z), IChunk> chunks;
    private List<IChunk> loadedChunks;
    private Player player;
    
    private void Awake()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        HandleChunkDrawing();
    }

    public void Initialize()
    {
        chunks = new Dictionary<(float x, float z), IChunk>();
        loadedChunks = new List<IChunk>();
        
        GameManager.Instance.OnPlayerSpawn += OnPlayerSpawn;

        // Register chunks
        foreach (var chunk in GetComponentsInChildren<IChunk>())
        {
            chunks.Add((chunk.Position.x, chunk.Position.y), chunk);
            chunk.Initialize();
            chunk.Deactivate();
        }
    }
    
    public void Free()
    {
        // Is it really needed?
    }

    public IChunk GetChunkAtPosition(float x, float z)
    {
        return chunks[GetChunkCoordinates(x, z)];
    }

    public (float x, float z) GetChunkCoordinates(float x, float z)
    {
        return (Mathf.Floor(x / WorldData.ChunkWidth) * WorldData.ChunkWidth, Mathf.Floor(z / WorldData.ChunkWidth) * WorldData.ChunkWidth);
    }

    private void HandleChunkDrawing()
    {
        var currentChunkPos = GetChunkCoordinates(player.transform.position.x, player.transform.position.z);

        foreach (var chunk in loadedChunks)
            chunk.Deactivate();
        loadedChunks.Clear();

        for (var x = currentChunkPos.x - Settings.Instance.GameplaySettings.RenderDistance * WorldData.ChunkWidth;
             x < currentChunkPos.x + Settings.Instance.GameplaySettings.RenderDistance * WorldData.ChunkWidth; x += WorldData.ChunkWidth)
        for (var z = currentChunkPos.z - Settings.Instance.GameplaySettings.RenderDistance * WorldData.ChunkWidth;
             z < currentChunkPos.z + Settings.Instance.GameplaySettings.RenderDistance * WorldData.ChunkWidth; z += WorldData.ChunkWidth)
        {
            if (!chunks.ContainsKey((x, z)))
                continue;

            var chunk = chunks[(x, z)];
            chunk.Activate();
            loadedChunks.Add(chunk);
        }
    }
    
    private void OnPlayerSpawn()
    {
        player = FindObjectOfType<Player>();
    }
}
