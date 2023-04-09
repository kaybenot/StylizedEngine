using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera), typeof(PlayerInput))]
public class CameraFollow : MonoBehaviour
{
    private Transform followTransform;
    private float input;
    private float y;

    private void Start()
    {
        followTransform = FindObjectOfType<Player>().transform;
        y = transform.position.y;
        
        if (followTransform != null)
            TeleportCamera(followTransform.position);
    }

    private void Update()
    {
        if (followTransform == null)
            return;

        MoveCameraToPlayer();
        RotateCamera();
    }

    public void TeleportCamera(Vector3 lookPoint)
    {
        transform.position = CameraPositionFromLookPoint(followTransform.position);
    }

    private void MoveCameraToPlayer()
    {
        transform.position = Vector3.Lerp(transform.position, CameraPositionFromLookPoint(followTransform.position), Time.deltaTime);
    }

    private void RotateCamera()
    {
        if (input == 0f)
            return;

        transform.RotateAround(followTransform.position, Vector3.up, input);
    }

    private Vector3 CameraPositionFromLookPoint(Vector3 point)
    {
        var forward = transform.forward;
        forward.y = 0f;
        forward.Normalize();
        
        var deltaY = transform.position.y - point.y;
        var target = point;
        target.y = y;
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
