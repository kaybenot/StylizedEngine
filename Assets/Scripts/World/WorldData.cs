using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StylizedEngine/WorldData", fileName = "World Data")]
public class WorldData : ScriptableObject
{
    [Header("World object settings")]
    public GameObject worldPrefab;
    public string Name = "World";
}
