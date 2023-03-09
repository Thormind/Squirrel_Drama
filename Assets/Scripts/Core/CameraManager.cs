using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public Transform target;

    public GameObject legacyMachine;

    public Material skyboxNoonMaterial;
    public Material skyboxNightMaterial;

    public Light dayLight;
    public Light nightLight;

    public Light loungeLight;
    public Light arcadeLight;

    private float dayLightIntensity = 4f;
    private float nightLightIntensity = 8f;
    private float loungeLightIntensity = 2000f;
    private float arcadeLightIntensity = 175f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float targetSize;
    private float smoothSpeed;

    // MENU
    private float menuFocusedSize = 75f;
    private Vector3 menuFocusedPosition = new Vector3(0f, 480f, -500f);
    private Quaternion menuFocusedRotation = Quaternion.Euler(20f, 10f, 0);

    private float menuUnfocusedSize = 150f;
    private Vector3 menuUnfocusedPosition = new Vector3(0f, 37.5f, -500f);
    private Quaternion menuUnfocusedRotation = Quaternion.Euler(-30, 0, 0);

    private float menuFocusedSize2 = 75f;
    private Vector3 menuFocusedPosition2 = new Vector3(0f, 350f, -500f);
    private Quaternion menuFocusedRotation2 = Quaternion.Euler(5f, -10, 0);

    private float menuUnfocusedSize2 = 120f;
    private Vector3 menuUnfocusedPosition2 = new Vector3(-25f, 75f, -500f);
    private Quaternion menuUnfocusedRotation2 = Quaternion.Euler(-25, 15, 0);

    private float menuCreditsSize = 25f;
    private Vector3 menuCreditsPosition = new Vector3(0f, 0f, -500f);
    private Quaternion menuCreditsRotation = Quaternion.Euler(-32, 0, 0);

    // LEGACY

    private float legacyFocusedSize = 2.5f;
    private Vector3 legacyFocusedPosition = new Vector3(2.5f, 326.25f, -500f);
    private Quaternion legacyFocusedRotation = Quaternion.Euler(3, 0, 0);

    private float legacyUnfocusedSize = 4.25f;
    private Vector3 legacyUnfocusedPosition = new Vector3(2.5f, 326.25f, -500f);
    private Quaternion legacyUnfocusedRotation = Quaternion.Euler(3, 0, 0);

    // INFINITE

    private float infiniteFocusedSize = 23.75f;
    private Vector3 infiniteFocusedPosition = new Vector3(0f, 175f, -500f);
    private Quaternion infiniteFocusedRotation = Quaternion.Euler(0, 0, 0);

    private float infiniteUnfocusedSize = 175f;
    private Vector3 infiniteUnfocusedPosition = new Vector3(0f, 230f, -500f);
    private Quaternion infiniteUnfocusedRotation = Quaternion.Euler(5, 0, 0);

    public float legacyVerticalOffset = 0.5f;
    public float infiniteVerticalOffset = 0.5f;

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
        transform.position = menuUnfocusedPosition;
        transform.rotation = menuUnfocusedRotation;

        Transition(false);
        SetTimeOfDay(SaveManager.instance.TimeOfDay);
    }

    void FixedUpdate()
    {    
        if (isFocused)
        {
            if (ScenesManager.instance.gameMode == GAME_MODE.INFINITE_MODE && InfiniteGameController.instance != null)
            {
                Vector3 targetPosition = InfiniteGameController.instance.GetElevatorPosition() + Vector3.up * infiniteVerticalOffset;
                Vector3 targetDirection = targetPosition - transform.position;
                targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            }
            if (ScenesManager.instance.gameMode == GAME_MODE.LEGACY_MODE && LegacyGameController.instance != null)
            {
                Vector3 targetPosition = LegacyGameController.instance.GetElevatorPosition() + Vector3.up * legacyVerticalOffset;
                Vector3 targetDirection = targetPosition - transform.position;
                targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            }
        }
 
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * smoothSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, Time.fixedDeltaTime * smoothSpeed);
        dayLight.intensity = Mathf.Lerp(dayLight.intensity, dayLightIntensity, Time.fixedDeltaTime * smoothSpeed);
        nightLight.intensity = Mathf.Lerp(nightLight.intensity, nightLightIntensity, Time.fixedDeltaTime * smoothSpeed);
        loungeLight.intensity = Mathf.Lerp(loungeLight.intensity, loungeLightIntensity, Time.fixedDeltaTime * smoothSpeed);
        arcadeLight.intensity = Mathf.Lerp(arcadeLight.intensity, arcadeLightIntensity, Time.fixedDeltaTime * smoothSpeed);

    }

    public void Transition(bool focused)
    {
        smoothSpeed = modeTransitionSpeed;

        isFocused = focused;

        if (ScenesManager.instance.gameMode == GAME_MODE.NONE)
        {
            if(GetComponent<AudioListener>() == null)
            {
                gameObject.AddComponent<AudioListener>();
            }
            CheckTimeOfDay();
            loungeLightIntensity = 2000f;
            arcadeLightIntensity = 0;
            legacyMachine.SetActive(true);

            if (SaveManager.instance.TimeOfDay == TIME_OF_DAY.NOON)
            {
                targetPosition = isFocused ? menuFocusedPosition : menuUnfocusedPosition;
                targetRotation = isFocused ? menuFocusedRotation : menuUnfocusedRotation;
                targetSize = isFocused ? menuFocusedSize : menuUnfocusedSize;

            }
            if (SaveManager.instance.TimeOfDay == TIME_OF_DAY.NIGHT)
            {
                targetPosition = isFocused ? menuFocusedPosition2 : menuUnfocusedPosition2;
                targetRotation = isFocused ? menuFocusedRotation2 : menuUnfocusedRotation2;
                targetSize = isFocused ? menuFocusedSize2 : menuUnfocusedSize2;
            }

        }
        if (ScenesManager.instance.gameMode == GAME_MODE.INFINITE_MODE)
        {
            Destroy(GetComponent<AudioListener>());
            CheckTimeOfDay();
            loungeLightIntensity = 2000f;
            arcadeLightIntensity = 0;
            legacyMachine.SetActive(true);

            if (isFocused)
            {
                targetSize = infiniteFocusedSize;
                targetPosition = infiniteFocusedPosition;

                if (InfiniteGameController.instance != null)
                {
                    Vector3 targetPosition = InfiniteGameController.instance.GetElevatorPosition() + Vector3.up * infiniteVerticalOffset;
                    Vector3 targetDirection = targetPosition - transform.position;
                    targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                }
            }
            else
            {
                targetPosition = infiniteUnfocusedPosition;
                targetRotation = infiniteUnfocusedRotation;
                targetSize = infiniteUnfocusedSize;
            }
        }
        if (ScenesManager.instance.gameMode == GAME_MODE.LEGACY_MODE)
        {
            CheckTimeOfDay();
            loungeLightIntensity = 0;
            arcadeLightIntensity = 175f;
            legacyMachine.SetActive(false);

            if (isFocused)
            {
                targetPosition = legacyFocusedPosition;
                targetSize = legacyFocusedSize;

                if (LegacyGameController.instance != null)
                {
                    Vector3 targetPosition = LegacyGameController.instance.GetElevatorPosition() + Vector3.up * legacyVerticalOffset;
                    Vector3 targetDirection = targetPosition - transform.position;
                    targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                }
            }
            else
            {
                targetPosition = legacyUnfocusedPosition;
                targetRotation = legacyUnfocusedRotation;
                targetSize = legacyUnfocusedSize;
            }
        }
    }

    public void TransitionToCredits()
    {
        if (ScenesManager.instance.gameMode == GAME_MODE.NONE)
        {
            CheckTimeOfDay();
            loungeLightIntensity = 2000f;
            arcadeLightIntensity = 0;
            legacyMachine.SetActive(true);

            targetPosition = menuCreditsPosition;
            targetRotation = menuCreditsRotation;
            targetSize = menuCreditsSize;
        }
    }

    public void CheckTimeOfDay()
    {
        if (SaveManager.instance.TimeOfDay == TIME_OF_DAY.NOON)
        {
            dayLightIntensity = 4f;
            nightLightIntensity = 0f;
        }
        if (SaveManager.instance.TimeOfDay == TIME_OF_DAY.NIGHT)
        {
            dayLightIntensity = 0f;
            nightLightIntensity = 8f;
        }
    }

    public void SetTimeOfDay(TIME_OF_DAY tod)
    {
        if (tod == TIME_OF_DAY.NOON)
        {
            if (ScenesManager.instance.gameMode == GAME_MODE.NONE)
            {
                targetPosition = isFocused ? menuFocusedPosition : menuUnfocusedPosition;
                targetRotation = isFocused ? menuFocusedRotation : menuUnfocusedRotation;
                targetSize = isFocused ? menuFocusedSize : menuUnfocusedSize;
            }
            


            dayLightIntensity = 4f;
            nightLightIntensity = 0f;

            Skybox skybox = Camera.main.GetComponent<Skybox>();
            skybox.material = skyboxNoonMaterial;
        }
        if (tod == TIME_OF_DAY.NIGHT)
        {
            if (ScenesManager.instance.gameMode == GAME_MODE.NONE)
            {
                targetPosition = isFocused ? menuFocusedPosition2 : menuUnfocusedPosition2;
                targetRotation = isFocused ? menuFocusedRotation2 : menuUnfocusedRotation2;
                targetSize = isFocused ? menuFocusedSize2 : menuUnfocusedSize2;
            }



            dayLightIntensity = 0f;
            nightLightIntensity = 8f;

            Skybox skybox = Camera.main.GetComponent<Skybox>();
            skybox.material = skyboxNightMaterial;
        }
    }

    public void ShakeCamera(float distanceToObstacle)
    {
        float shakeForce = Mathf.Clamp(7f - distanceToObstacle, 1f, 7f) * 1.25f; // Set a max force for the shake
        print($"Shake force: {shakeForce}");
        StartCoroutine(DoShakeCamera(shakeForce));
    }

    private IEnumerator DoShakeCamera(float shakeForce)
    {
        float elapsedTime = 0f;
        Vector3 originalPosition = transform.position;

        while (elapsedTime < 0.5f) // Shake for 0.3 seconds
        {
            float x = originalPosition.x + Random.Range(-shakeForce, shakeForce) * 0.05f;
            float y = originalPosition.y + Random.Range(-shakeForce, shakeForce) * 0.05f;
            float z = originalPosition.z + Random.Range(-shakeForce, shakeForce) * 0.05f;

            Vector3 newPos = new Vector3(x, y, z);

            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 25f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition; // Reset the camera position to its original position
    }
}

