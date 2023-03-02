using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;

    void FixedUpdate()
    {

        if ((target.position.x >= 25 || target.position.x <= -25) || 
            (target.position.y >= 12 || target.position.y <= -12))
        {
            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, -40);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
        else
        {
            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, -25);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }

    }
}
