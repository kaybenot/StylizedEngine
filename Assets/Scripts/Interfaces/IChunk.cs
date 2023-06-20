using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChunk
{
    Vector2 Position { get; }
    float Width { get; }
    bool Active { get; }
    List<IChunkComponent> ChunkComponents { get; }

    /// <summary>
    /// Called during world initialization.
    /// </summary>
    void Initialize();
    
    /// <summary>
    /// Function used when creating world in world editor. Do not use ingame!
    /// TODO: Make it less hacky
    /// </summary>
    void Create(Vector2 position, float width);
    
    void Activate();
    void Deactivate();

    void RegisterComponent(IChunkComponent component);
    void UnregisterComponent(IChunkComponent component);
}
