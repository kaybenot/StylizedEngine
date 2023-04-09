using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform followTransform = null;
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
    }

    public void TeleportCamera(Vector3 lookPoint)
    {
        var deltaY = transform.position.y - followTransform.position.y;
        
        var target = followTransform.position;
        target.y = y;
        target.z -= deltaY * Mathf.Tan((90 - transform.rotation.eulerAngles.x) * Mathf.Deg2Rad);

        transform.position = target;
    }

    private void MoveCameraToPlayer()
    {
        var deltaY = transform.position.y - followTransform.position.y;
        
        var target = followTransform.position;
        target.y = y;
        target.z -= deltaY * Mathf.Tan((90 - transform.rotation.eulerAngles.x) * Mathf.Deg2Rad);

        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
    }
}
