using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Inject] private IInputManager inputManager;
    
    private Transform followTransform;
    private Player player;
    private float input;
    private float fixedCameraPositionY;

    private void Start()
    {
        GameManager.Instance.OnGameReady += OnGameReady;
    }

    private void OnGameReady()
    {
        player = FindObjectOfType<Player>();
        followTransform = player.transform;
        fixedCameraPositionY = transform.position.y;
        
        if (followTransform != null)
            TeleportCamera(followTransform.position);

        inputManager.OnLookInput += OnLook;
    }

    private void OnValidate()
    {
        // Teleport camera to player spawner position
        fixedCameraPositionY = transform.position.y;

        var scenePlayer = FindObjectOfType<PlayerSpawner>();
        if (scenePlayer != null)
            TeleportCamera(scenePlayer.transform.position);
    }

    private void Update()
    {
        if (followTransform == null)
            return;

        HandleCameraRotation();
        MoveCameraToPlayer();
    }

    public void TeleportCamera(Vector3 lookTarget)
    {
        transform.position = GetLookPositionWithOffset(lookTarget);
    }

    private void MoveCameraToPlayer()
    {
        transform.position = Vector3.Lerp(transform.position, GetLookPositionWithOffset(followTransform.position), Time.deltaTime);
    }

    private void HandleCameraRotation()
    {
        if (input == 0f)
            return;

        transform.RotateAround(followTransform.position, Vector3.up, input);
        player.UpdateMove();
    }

    /// <summary>
    /// Camera position helper function
    /// </summary>
    /// <param name="point">Target position</param>
    /// <returns>Position which camera should be at to maintain target at center of the screen</returns>
    private Vector3 GetLookPositionWithOffset(Vector3 point)
    {
        var forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();
        
        var deltaY = transform.position.y - point.y;
        var target = point;
        target.y = fixedCameraPositionY;
        target.z -= deltaY * Mathf.Tan((90 - transform.rotation.eulerAngles.x) * Mathf.Deg2Rad) * forward.z;
        target.x -= deltaY * Mathf.Tan((90 - transform.rotation.eulerAngles.x) * Mathf.Deg2Rad) * forward.x;

        return target;
    }

    private void OnLook(float input)
    {
        this.input = input;
    }
}
