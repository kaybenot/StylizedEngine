using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorldManager
{
    WorldContainer GetWorldDataContainer();
    void LoadWorld(WorldData worldData, Transform parent = null);
    void UnloadWorld();
}
