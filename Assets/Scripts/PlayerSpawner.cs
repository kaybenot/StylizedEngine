using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour, ISpawner
{
    [SerializeField] private GameObject playerPrefab;

    /// <summary>
    /// Temporary field ensuring that no more than 1 player is present
    /// </summary>
    private static bool spawned = false;

    private void Awake()
    {
        Spawn();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "spawner.png");
    }

    public void Spawn()
    {
        if (spawned)
            throw new NotImplementedException("Spawning multiple players is not implemented!");

        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        spawned = true;
    }
}
