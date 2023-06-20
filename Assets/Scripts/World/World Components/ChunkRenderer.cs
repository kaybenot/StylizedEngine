using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkRenderer : MonoBehaviour, IWorldComponent
{
    private List<IChunk> loadedChunks;
    private IWorld world;
    private Player player;

    private void FixedUpdate()
    {
        HandleChunkDrawing();
    }

    public void Initialize(IWorld _world)
    {
        loadedChunks = new List<IChunk>();

        world = _world;

        GameManager.Instance.OnPlayerSpawn += () => player = FindObjectOfType<Player>();
    }
    
    private void HandleChunkDrawing()
    {
        var currentChunkPos = world.GetChunkCoordinates(player.transform.position.x, player.transform.position.z);

        foreach (var chunk in loadedChunks)
            chunk.Deactivate();
        loadedChunks.Clear();

        var chunkWidth = world.WorldData.ChunkWidth;

        for (var x = currentChunkPos.x - Settings.Instance.GameplaySettings.RenderDistance * chunkWidth;
             x < currentChunkPos.x + Settings.Instance.GameplaySettings.RenderDistance * chunkWidth; x += chunkWidth)
        for (var z = currentChunkPos.z - Settings.Instance.GameplaySettings.RenderDistance * chunkWidth;
             z < currentChunkPos.z + Settings.Instance.GameplaySettings.RenderDistance * chunkWidth; z += chunkWidth)
        {
            var chunk = world.GetChunkAtPosition(x, z);
            if (chunk == null)
                continue;

            chunk.Activate();
            loadedChunks.Add(chunk);
        }
    }
}
