using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StylizedEngine/GameplaySettings", fileName = "Gameplay Settings")]
public class GameplaySettings : ScriptableObject
{
    public int RenderDistance = 2;
    public int GrassDensity = 5;
}
