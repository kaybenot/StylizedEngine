using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private static bool spawned = false;

    private void Awake()
    {
        SpawnPlayer();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "spawner.png");
    }

    public void SpawnPlayer()
    {
        if (spawned)
        {
            Debug.LogError("Spawning multiple players is not implemented!");
            return;
        }

        Instantiate(playerPrefab, transform.position, Quaternion.identity);
        spawned = true;
    }
}
