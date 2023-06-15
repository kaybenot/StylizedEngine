using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [field: SerializeField] public Vector2 Position { get; private set; }
    [field: SerializeField] public float Width { get; private set; }

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
