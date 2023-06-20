using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChunkComponent
{
    void Initialize();
    void ChunkLoaded();
    void ChunkUnloaded();
}
