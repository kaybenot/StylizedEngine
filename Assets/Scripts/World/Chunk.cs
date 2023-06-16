using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [field: SerializeField] public Vector2 Position { get; private set; }
    [field: SerializeField] public float Width { get; private set; }

    private GrassSpawner grassSpawner;

    private void Awake()
    {
        grassSpawner = GetComponentInChildren<GrassSpawner>();
    }

    private void Start()
    {
        grassSpawner.Spawn();
    }

    public void Initialize(Vector2 position, float width)
    {
        Position = position;
        Width = width;
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
