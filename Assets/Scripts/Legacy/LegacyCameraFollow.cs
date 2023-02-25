using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyCameraFollow : MonoBehaviour
{
    public static LegacyCameraFollow instance;

    public Transform target;
    private Vector3 unfocusedPosition = new Vector3(0f, -0.5f, -35f);
    private Vector3 focusedPosition = new Vector3(0f, 0f, -15f);
    private Quaternion startingRotation;
    private float unfocusedFOV = 30f;
    private float focusedFOV = 40f;

    public float introSpeed = 2.0f;
    public float smoothSpeed = 1.2f;
    public float cameraVerticalOffset = 0.125f;

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
        startingRotation = Quaternion.Euler(0, 0, 0);
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = isFocused ? focusedPosition : unfocusedPosition;
        float targetFOV = isFocused ? focusedFOV : unfocusedFOV;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * smoothSpeed);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, Time.fixedDeltaTime * smoothSpeed);

        if (isFocused)
        {
            Vector3 targetDirection = target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, startingRotation, Time.deltaTime * smoothSpeed);
        }
    }
}
