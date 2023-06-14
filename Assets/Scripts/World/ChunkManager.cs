using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class ChunkManager : ZenAutoInjecter
{ 
    public Vector3Int ChunkSize { get; private set; }
    public int RenderDistance { get; set; } = 1;
    public List<(int x, int z)> LoadedChunkPositions { get; private set; }

    [Inject] private ISession session;
    
    private Dictionary<(int x, int z), Chunk> chunks = new ();
    private Player player;

    private void Start()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        ManageChunks();
    }

    [CanBeNull] public Chunk GetChunk(int x, int z)
    {
        return chunks[(x, z)];
    }
    
    private void Initialize()
    {
        var floatChunkSize = GetComponentInChildren<Terrain>().terrainData.size;
        ChunkSize = new Vector3Int((int)floatChunkSize.x, (int)floatChunkSize.y, (int)floatChunkSize.z);

        foreach (var terrain in GetComponentsInChildren<Terrain>())
        {
            var pos = ((int)transform.position.x, (int)transform.position.z);
            
            if (chunks.ContainsKey(pos)) 
                chunks[pos].Terrain = terrain;
            else
            {
                var chunk = new Chunk();
                chunk.Terrain = terrain;
                
                chunks.Add(pos, chunk);
            }
        }

        session.OnInitialized += () => Object.FindObjectOfType<Player>();
    }

    private void ManageChunks()
    {
        for (var x = player.PositionInt.x - RenderDistance; x <= player.PositionInt.x + RenderDistance; x++)
        for (var z = player.PositionInt.z - RenderDistance; z <= player.PositionInt.z + RenderDistance; z++)
        {
            if (LoadedChunkPositions.Contains((x, z)))
                continue;

            var chunk = GetChunk(x, z);
            if(chunk == null)
                continue;
            
            chunk.LoadChunk();
            LoadedChunkPositions.Add((x, z));
        }
    }
}
