using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectData
{
    public SerializableGuid ID = Guid.NewGuid();
    public Vector3 Position;
    public Quaternion Rotation;
    public GameObject Prefab;
}
