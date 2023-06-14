using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StylizedEngine/WorldContainer", fileName = "World Container")]
public class WorldContainer : ScriptableObject
{
    public List<WorldData> Worlds;
}
