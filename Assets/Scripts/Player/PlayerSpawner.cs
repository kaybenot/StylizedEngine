using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerSpawner : StaticWorldObject, ISpawner
{
    [SerializeField] private GameObject playerPrefab;

    [Inject] private ISession session;

    private void Start()
    {
        Spawn();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "spawner.png");
    }

    /// <summary>
    /// Spawning multiple players is undefined for now.
    /// </summary>
    public void Spawn()
    {
        session.TryAddData(new ObjectData(playerPrefab)
        {
            Position = transform.position,
            Rotation = Quaternion.identity
        });
        
        GameManager.Instance.OnPlayerSpawn?.Invoke();
        GameManager.Instance.OnPlayerSpawn = null;
    }
}
