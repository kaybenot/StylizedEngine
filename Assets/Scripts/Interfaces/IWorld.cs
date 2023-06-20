using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public interface IWorld
{
    WorldData WorldData { get; set; }
    List<IWorldComponent> WorldComponents { get; }

    void Initialize();
    void Free();

    void RegisterComponent(IWorldComponent component);
    void UnregisterComponent(IWorldComponent component);

    [CanBeNull] IChunk GetChunkAtPosition(float x, float z);
    /// <summary>
    /// Gets chunk root position from point on world.
    /// </summary>
    /// <returns>Chunk root position</returns>
    (float x, float z) GetChunkCoordinates(float x, float z);
}
