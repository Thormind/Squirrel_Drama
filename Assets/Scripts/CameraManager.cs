using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public Transform target;

    public GameObject legacyMachine;
    public Light exteriorLight;
    private float exteriorLightIntensity = 1;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float targetFOV;
    private float smoothSpeed;

    private Vector3 menuPosition = new Vector3(0f, 0f, -900f);
    private Vector3 launchPosition = new Vector3(0f, 200f, -7000f);

    private Vector3 legacyUnfocusedPosition = new Vector3(0f, -0.5f, -35f);
    private Vector3 legacyFocusedPosition = new Vector3(0f, 0f, -15f);

    private Vector3 infiniteUnfocusedPosition = new Vector3(0f, -260f, -2000f);
    private Vector3 infiniteFocusedPosition = new Vector3(0f, -550f, -250f);
    public float infiniteVerticalOffset = 20f;

    private Quaternion menuRotation;
    private Quaternion launchRotation;
    private Quaternion legacyRotation;
    private Quaternion infiniteRotation;

    private float menuFOV = 20f;
    private float unfocusedFOV = 30f;
    private float focusedFOV = 40f;

    private float focusUnfocusSpeed = 2.0f;
    private float modeTransitionSpeed = 2.5f;

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
        menuRotation = Quaternion.Euler(0, 10, 0);
        launchRotation = Quaternion.Euler(0, 0, 0);
        legacyRotation = Quaternion.Euler(0, 0, 0);
        infiniteRotation = Quaternion.Euler(0, 0, 0);

        transform.position = launchPosition;
        transform.rotation = launchRotation;

        ModeTransition();
    }

    void FixedUpdate()
    {
        if (isFocused)
        {
            if (ScenesManager.instance.gameMode == 1)
            {
                Vector3 desiredPosition = new Vector3(infiniteFocusedPosition.x, InfiniteGameController.instance.GetFruitPosition().y + infiniteVerticalOffset, infiniteFocusedPosition.z);
                targetPosition = desiredPosition;
            }
            if (ScenesManager.instance.gameMode == 2)
            {
                Vector3 targetDirection = new Vector3(0, LegacyGameController.instance.GetBallPosition().y, 0) - transform.position;
                targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up); 
            }
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * smoothSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, Time.fixedDeltaTime * smoothSpeed);
        exteriorLight.intensity = Mathf.Lerp(exteriorLight.intensity, exteriorLightIntensity, Time.fixedDeltaTime * smoothSpeed);
    }

    public void ModeTransition()
    {
        isFocused = false;
        smoothSpeed = modeTransitionSpeed;

        if (ScenesManager.instance.gameMode == 0)
        {
            exteriorLightIntensity = 1;
            legacyMachine.SetActive(true);

            targetPosition = menuPosition;
            targetRotation = menuRotation;
            targetFOV = menuFOV;
        }
        if (ScenesManager.instance.gameMode == 1)
        {
            targetPosition = infiniteUnfocusedPosition;
            targetRotation = infiniteRotation;
            targetFOV = unfocusedFOV;
        }
        if (ScenesManager.instance.gameMode == 2)
        {
            exteriorLightIntensity = 0;
            legacyMachine.SetActive(false);

            targetPosition = legacyUnfocusedPosition;
            targetRotation = legacyRotation;
            targetFOV = unfocusedFOV;
        }
    }

    public void FocusTransition()
    {
        smoothSpeed = focusUnfocusSpeed;

        if (ScenesManager.instance.gameMode == 1)
        {
            if (isFocused)
            {
                Vector3 desiredPosition = new Vector3(infiniteFocusedPosition.x, InfiniteGameController.instance.GetFruitPosition().y + infiniteVerticalOffset, infiniteFocusedPosition.y);
                targetPosition = infiniteFocusedPosition;
                targetFOV = focusedFOV;
            }
            else
            {
                targetPosition = infiniteUnfocusedPosition;
                targetRotation = infiniteRotation;
                targetFOV = unfocusedFOV;
            }
        }
        if (ScenesManager.instance.gameMode == 2)
        {
            if (isFocused)
            {
                targetPosition = legacyFocusedPosition;
                Vector3 targetDirection = LegacyGameController.instance.GetBallPosition() - transform.position;
                targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up); ;
                targetFOV = focusedFOV;
            }
            else
            {
                targetPosition = legacyUnfocusedPosition;
                targetRotation = legacyRotation;
                targetFOV = unfocusedFOV;
            }
        }
    }

    public void SetFocus()
    {
        isFocused = true;
        FocusTransition();
    }

    public void SetUnfocus()
    {
        isFocused = false;
        FocusTransition();
    }
}

