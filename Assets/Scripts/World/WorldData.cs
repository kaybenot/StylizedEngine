using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StylizedEngine/WorldData", fileName = "World Data")]
public class WorldData : ScriptableObject
{
    [Header("World object settings")]
    public string Name = "New World";
    public float ChunkWidth = 20f;
    public float ChunkHeight = 50f;
    public int ChunkCountX = 10;
    public int ChunkCountZ = 10;
    public GameObject Prefab;
}
