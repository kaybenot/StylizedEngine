using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISessionObject
{
    SerializableGuid ID { get; set; }
    
    bool SpawnFromData(Transform parent = null);
}
