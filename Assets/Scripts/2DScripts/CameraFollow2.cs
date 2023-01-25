using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2 : MonoBehaviour
{
    public static CameraFollow2 instance;

    public Transform target;
    public Vector3 gameOverPosition;

    public float smoothSpeed = 0.125f;
    public float cameraVerticalOffset = 0.125f;

    public bool isGameActive = false;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        gameOverPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (isGameActive)
        {
            Vector3 desiredPosition = new Vector3(transform.position.x, target.position.y + cameraVerticalOffset, -15);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
        else
        {
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, gameOverPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
