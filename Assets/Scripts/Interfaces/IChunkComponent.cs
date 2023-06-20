using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChunkComponent
{
    void Initialize(IChunk chunk);
    void ChunkLoaded();
    void ChunkUnloaded();
}
