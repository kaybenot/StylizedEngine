using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStaticWorldObject
{
    Bounds Bounds { get; }

    void Activate();
    void Deactivate();
    void DestroyImmediate();
}
