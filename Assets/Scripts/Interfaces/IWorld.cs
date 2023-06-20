using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorld
{
    WorldData WorldData { get; set; }

    void Initialize();
    void Free();

    IChunk GetChunkAtPosition(float x, float z);
    /// <summary>
    /// Gets chunk root position from point on world.
    /// </summary>
    /// <returns>Chunk root position</returns>
    (float x, float z) GetChunkCoordinates(float x, float z);
}
