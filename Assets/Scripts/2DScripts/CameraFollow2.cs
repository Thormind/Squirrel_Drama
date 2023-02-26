using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2 : MonoBehaviour
{
    public static CameraFollow2 instance;

    public Transform target;
    public Vector3 gameOverPosition;

    public float smoothSpeed = 0.125f;
    public float cameraVerticalOffset = 20f;

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
        InfiniteGameController.instance.StartGame();
        gameOverPosition = transform.position;
    }

    void FixedUpdate()
    {

        Vector3 desiredPosition = new Vector3(transform.position.x, target.position.y + cameraVerticalOffset, -250);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

    }
}
