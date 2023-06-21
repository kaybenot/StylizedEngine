using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StaticWorldObject : ZenAutoInjecter, IStaticWorldObject
{
    public Bounds Bounds
    {
        get
        {
            if (meshRenderer != null)
                return meshRenderer.bounds;
            
            meshRenderer = GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null)
                return meshRenderer.bounds;
                
            Debug.LogError($"Could not find a MeshRenderer in object {name}!");
            return new Bounds();
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public void DestroyImmediate()
    {
        GameObject.DestroyImmediate(this);
    }

    private MeshRenderer meshRenderer = null;
}
