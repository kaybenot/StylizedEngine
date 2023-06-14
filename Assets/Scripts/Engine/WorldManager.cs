using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class WorldManager : IWorldManager
{
    public bool WorldLoaded { get; private set; }

    [Inject] private ISession session;

    private GameObject world;
    
    public WorldContainer GetWorldDataContainer()
    {
        return Resources.Load<WorldContainer>("Worlds/World Container");
    }

    public void LoadWorld(WorldData worldData, Transform parent = null)
    {
        // TODO: Resuming session
        session.New();

        world = Object.Instantiate(worldData.Prefab, parent);

        WorldLoaded = true;
    }

    public void UnloadWorld()
    {
        if (!WorldLoaded)
            return;
        
        session.Unload();
        
        Object.Destroy(world);
        world = null;

        WorldLoaded = false;
    }
}
