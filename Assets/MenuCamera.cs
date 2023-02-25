using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera: MonoBehaviour
{
    public static MenuCamera instance;

    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private Quaternion initialRotation;
    private Quaternion targetRotation;

    private Vector3 menuPosition = new Vector3(0f, 4.4f, -5f);
    private Vector3 fromLaunchPosition = new Vector3(0f, 4.35f, -25f);
    private Vector3 fromLegacyPosition = new Vector3(0.03f, 4.35f, -0.03f);
    private Vector3 fromInfinitePosition = new Vector3(0f, 0f, -15f);

    private Quaternion menuRotation;
    private Quaternion fromLaunchRotation;
    private Quaternion fromLegacyRotation;
    private Quaternion fromInfiniteRotation;

    public float smoothSpeed = 2.0f;

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
        menuRotation = Quaternion.Euler(0, 10, 0);
        fromLaunchRotation = Quaternion.Euler(0, 0, 0);
        fromLegacyRotation = Quaternion.Euler(0, 0, 0);
        fromInfiniteRotation = Quaternion.Euler(0, 0, 0);

        if (ScenesManager.instance.gameMode == 0)
        {
            initialPosition = fromLaunchPosition;
            initialRotation = fromLaunchRotation;

        }
        if (ScenesManager.instance.gameMode == 1)
        {
            initialPosition = fromInfinitePosition;
            initialRotation = fromInfiniteRotation;

        }
        if (ScenesManager.instance.gameMode == 2)
        {
            initialPosition = fromLegacyPosition;
            initialRotation = fromLegacyRotation;
        }

        transform.position = initialPosition;
        transform.rotation = initialRotation;

        targetPosition = menuPosition;
        targetRotation = menuRotation;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * smoothSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
    }

    private void FromLaunchTransition()
    {
        transform.position = Vector3.Lerp(menuPosition, fromLaunchPosition, Time.fixedDeltaTime * smoothSpeed);
        transform.rotation = Quaternion.Lerp(menuRotation, fromLaunchRotation, Time.fixedDeltaTime * smoothSpeed);
    }

    private void FromInfiniteTransition()
    {
        transform.position = Vector3.Lerp(fromInfinitePosition, menuPosition, Time.fixedDeltaTime * smoothSpeed);
        transform.rotation = Quaternion.Lerp(fromInfiniteRotation, menuRotation, Time.fixedDeltaTime * smoothSpeed);
    }

    private void FromLegacyTransition()
    {
        transform.position = Vector3.Lerp(fromLegacyPosition, menuPosition, Time.fixedDeltaTime * smoothSpeed);
        transform.rotation = Quaternion.Lerp(fromLegacyRotation, menuRotation, Time.fixedDeltaTime * smoothSpeed);
    }
}
