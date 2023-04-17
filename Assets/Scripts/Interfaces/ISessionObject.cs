using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISessionObject
{
    Guid ID { get; set; }
    
    bool SpawnFromData(Transform parent = null);
}
