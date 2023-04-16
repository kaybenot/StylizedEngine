using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera), typeof(PlayerInput))]
public class CameraFollow : MonoBehaviour
{
    private Transform followTransform;
    private float input;
    private float fixedCameraPositionY;

    private void Start()
    {
        followTransform = FindObjectOfType<Player>().transform;
        fixedCameraPositionY = transform.position.y;
        
        if (followTransform != null)
            TeleportCamera(followTransform.position);
    }

    private void OnValidate()
    {
        // Teleport camera to player spawner position
        fixedCameraPositionY = transform.position.y;
        TeleportCamera(FindObjectOfType<PlayerSpawner>().transform.position);
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

    public void Look(InputAction.CallbackContext context)
    {
        if (context.performed)
            input = context.ReadValue<float>();
        if (context.canceled)
            input = 0f;
    }
}
