using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    public WorldData Data => data;

    private ChunkManager chunkManager;
    private WorldData data;
    private GameObject worldInstanceObject;

    public World(WorldData worldData)
    {
        data = worldData;
        
        worldInstanceObject = UnityEngine.Object.Instantiate(data.worldPrefab);
        chunkManager = worldInstanceObject.GetComponentInChildren<ChunkManager>();
    }
}
