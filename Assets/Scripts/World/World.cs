using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public WorldData Data;

    private Dictionary<(float x, float z), Chunk> chunks;
    private List<Chunk> loadedChunks;
    private Player player;
    
    private void Awake()
    {
        chunks = new Dictionary<(float x, float z), Chunk>();
        loadedChunks = new List<Chunk>();
        
        GameManager.Instance.OnPlayerSpawn += OnPlayerSpawn;

        // Register chunks
        foreach (var chunk in GetComponentsInChildren<Chunk>())
        {
            chunks.Add((chunk.Position.x, chunk.Position.y), chunk);
            chunk.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        HandleChunkDrawing();
    }
    
    /// <summary>
    /// Gets chunk position from world point.
    /// </summary>
    public (float x, float z) GetChunkPosition(float x, float z)
    {
        return (Mathf.Floor(x / Data.ChunkWidth) * Data.ChunkWidth, Mathf.Floor(z / Data.ChunkWidth) * Data.ChunkWidth);
    }

    public Chunk GetChunkOnPoint(float x, float z)
    {
        return chunks[GetChunkPosition(x, z)];
    }

    private void HandleChunkDrawing()
    {
        var currentChunk = GetChunkOnPoint(player.transform.position.x, player.transform.position.z);
        
        // TODO: Proper chunk unloading
        foreach (var chunk in loadedChunks)
            chunk.SetActive(false);
        loadedChunks.Clear();

        if (!loadedChunks.Contains(currentChunk))
        {
            currentChunk.SetActive(true);
            loadedChunks.Add(currentChunk);
        }
    }
    
    private void OnPlayerSpawn()
    {
        player = FindObjectOfType<Player>();
    }
}
