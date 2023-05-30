using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

[Serializable]
public class ObjectData
{
    public ObjectData(GameObject prefab)
    {
        Prefab = prefab;
        ID = Guid.NewGuid();
    }

    public SerializableGuid ID;
    public Vector3 Position;
    public Quaternion Rotation;
    /// <warning>
    /// Parent CANNOT be other session object!
    /// </warning>
    public Transform Parent;
    public GameObject Prefab;
}
