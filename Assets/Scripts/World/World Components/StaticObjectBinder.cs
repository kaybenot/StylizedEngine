using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjectBinder : MonoBehaviour, IWorldComponent
{
    public void Initialize(IWorld world)
    {
        // Bind static objects to chunks based on bounds
        var chunks = world.GetChunks();
        var staticWorldObjects = world.GetStaticWorldObjects();
        
        foreach (var obj in staticWorldObjects)
        {
            foreach (var chunk in chunks)
            {
                if (obj.Bounds.Intersects(chunk.Bounds))
                    chunk.BindStaticObject(obj);
            }
        }
    }
}
