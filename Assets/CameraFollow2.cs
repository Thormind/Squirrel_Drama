using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2 : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public float cameraVerticalOffset = 0.125f;

    void FixedUpdate()
    {
        Vector3 desiredPosition = new Vector3(transform.position.x, target.position.y + cameraVerticalOffset, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
