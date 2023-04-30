using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerSpawner : MonoBehaviour, ISpawner
{
    [SerializeField] private GameObject playerPrefab;

    [Inject] private ISession session;
    
    /// <summary>
    /// Temporary field ensuring that no more than 1 player is present
    /// </summary>
    private static bool spawned = false;

    private void Start()
    {
        session.OnInitialized += Spawn;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "spawner.png");
    }

    public void Spawn()
    {
        if (spawned)
            throw new NotImplementedException("Spawning multiple players is not implemented!");

        var player = session.TrySpawnObject(new ObjectData()
        {
            Prefab = playerPrefab,
            Position = transform.position,
            Rotation = Quaternion.identity
        });
        
        if (player != null)
            spawned = true;
    }
}
