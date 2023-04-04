using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2 : MonoBehaviour
{
    public static CameraFollow2 instance;

    public Transform target;
    public Vector3 gameOverPosition;

    public float smoothSpeed = 0.125f;
    public float cameraVerticalOffset = 50f;

    public bool isFocused = false;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isFocused = !isFocused;
        }
    }

    void FixedUpdate()
    {
        if (isFocused)
        {
            Vector3 desiredPosition = new Vector3(transform.position.x, target.position.y + cameraVerticalOffset, -400f);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
        else
        {
            Vector3 desiredPosition = new Vector3(transform.position.x, -300, -1500f);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }

    }
}
