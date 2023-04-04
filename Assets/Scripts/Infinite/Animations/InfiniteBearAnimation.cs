using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfiniteBearAnimation : MonoBehaviour
{

    public TextMeshProUGUI countdownText;
    public GameObject bearPaw;
    public GameObject shadowImage;

    public GameObject warnIndicatorImage;
    public GameObject warnIndicatorVFX;
    public float flashInterval = 0.5f;

    private float slowMotionMultiplier = 0.085f;
    private float impactMinDistanceFromFruit = 2f;
    private float impactMaxDistanceFromFruit = 7f;

    private float warnCooldown;
    private float defaultFixedDeltaTime = 0.02f;

    public float translationSpeed = 10f;
    public float delayBeforeDestroy = 0.05f;
    private float delayStayMobile = 1f;
    private float delayInterval = 0.1f;

    private float shadowMinAlpha = 75f;
    private float shadowMaxAlpha = 200f;

    public float bearMinAlpha = 170f;
    public float bearMaxAlpha = 255f;

    private Vector3 impactPosition = Vector3.zero;
    private Quaternion impactRotation = Quaternion.Euler(0f, 0f, 0f);

    private Vector3 postImpactPosition = new Vector3(0f, 0f, -0.5f);
    private Quaternion postImpactRotation = Quaternion.Euler(15f, 0f, 0f);

    private bool isCoroutineRunning = false;
    private PolygonCollider2D bearCollider;

    public GameObject impactVFX;


    private void Start()
    {
        bearCollider = GetComponent<PolygonCollider2D>();
        bearCollider.enabled = false;

        bearPaw.SetActive(false);
    }

    public void HandleBearAnimationFunction(float warnTime, float impactRange)
    {
        warnCooldown = warnTime;
        transform.localScale = new Vector3(impactRange, impactRange, impactRange);

        StartCoroutine(BearPawAttack());
    }

    IEnumerator BearPawAttack()
    {
        if (isCoroutineRunning)
        {
            yield break;
        }

        isCoroutineRunning = true;


        // ============== WARN ANIMATION ============== //
        float remainingCooldown = warnCooldown;
        PlayRoarSFX();
        PlayWarnIndicatorVFX();

        while (remainingCooldown > 0f)
        {
            // Update countdown text
            countdownText.text = remainingCooldown.ToString("0.0");

            // Update shadow image scale and alpha
            float shadowXScale = Mathf.Lerp(shadowImage.transform.localScale.x, 1f, (warnCooldown - remainingCooldown) / warnCooldown);
            float shadowYScale = Mathf.Lerp(shadowImage.transform.localScale.y, 1f, (warnCooldown - remainingCooldown) / warnCooldown);
            shadowImage.transform.localScale = new Vector3(shadowXScale, shadowYScale, 1f);

            float shadowAlpha = Mathf.Lerp(shadowMinAlpha, shadowMaxAlpha, (warnCooldown - remainingCooldown) / warnCooldown);
            SetShadowAlpha(shadowAlpha);

            yield return new WaitForSeconds(delayInterval);
            remainingCooldown -= delayInterval;
        }

        countdownText.text = "";


        // ============== SLOW MOTION BEFORE IMPACT ANIMATION ============== //
        bearPaw.SetActive(true);

        if (ScenesManager.gameState == GAME_STATE.ACTIVE)
        {
            SetSlowMotion(true);
        }


        Vector3 startPosition = bearPaw.transform.localPosition;
        Vector3 endPosition = impactPosition;

        Quaternion startRotation = bearPaw.transform.localRotation;
        Quaternion endRotation = impactRotation;

        float distance = Vector3.Distance(startPosition, endPosition);
        float duration = distance / translationSpeed;

        float t = 0f;

        while (t < 1f && bearPaw.transform.localRotation.x <= endRotation.x)
        {
            float easedProgress = EaseInCirc(t);

            bearPaw.transform.localPosition = Vector3.Lerp(startPosition, endPosition, easedProgress);
            bearPaw.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, easedProgress);

            float bearAlpha = Mathf.Lerp(bearMinAlpha, bearMaxAlpha, easedProgress);
            SetBearAlpha(bearAlpha);


            t += Time.deltaTime / duration;
            yield return null;
        }

        bearPaw.transform.localPosition = endPosition;
        bearPaw.transform.localRotation = endRotation;
        SetBearAlpha(bearMaxAlpha);
        SetShadowAlpha(0);

        // ============== IMPACT ANIMATION ============== //
        bearCollider.enabled = true;
        bearCollider.isTrigger = true;

        SetSlowMotion(false);
        PlayImpactSFX();
        PlayImpactVFX();
        PlayImpactCameraShake();

        yield return new WaitForSeconds(delayInterval);

        bearCollider.isTrigger = false;

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / delayStayMobile;
            yield return null;
        }

        bearCollider.enabled = false;


        // ============== POST IMPACT ANIMATION ============== //

        startPosition = bearPaw.transform.localPosition;
        endPosition = postImpactPosition;
        startRotation = bearPaw.transform.localRotation;
        endRotation = postImpactRotation;

        t = 0f;

        while (t < 1f)
        {
            float easedProgress = EaseOutCirc(t);

            float bearAlpha = Mathf.Lerp(bearMaxAlpha, bearMinAlpha, easedProgress);
            SetBearAlpha(bearAlpha);

            bearPaw.transform.localPosition = Vector3.Lerp(startPosition, endPosition, easedProgress);
            bearPaw.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, easedProgress);

            t += Time.deltaTime / delayBeforeDestroy;
            yield return null;
        }

        Destroy(gameObject);

        isCoroutineRunning = false;
    }

    // ====================================== //
    // =============== METHODS ============== //
    // ====================================== //

    private void SetShadowAlpha(float shadowAlpha)
    {
        Color shadowColor = shadowImage.GetComponent<Image>().color;
        shadowColor.a = shadowAlpha / 255f;
        shadowImage.GetComponent<Image>().color = shadowColor;
    }

    private void SetBearAlpha(float bearAlpha)
    {
        Color bearColor = bearPaw.GetComponentInChildren<MeshRenderer>().material.color;
        bearColor.a = bearAlpha / 255f;
        bearPaw.GetComponentInChildren<MeshRenderer>().material.color = bearColor;
    }

    private void SetSlowMotion(bool isSlowMotion)
    {
        InfiniteGameController.instance.SetRigidBodyExtrapolate(isSlowMotion);

        if (isSlowMotion)
        {
            float distanceFromFruit = Vector2.Distance(InfiniteGameController.instance.GetFruitLocalPosition(), transform.localPosition);
            float slowMotion = Mathf.Clamp(distanceFromFruit, impactMinDistanceFromFruit, impactMaxDistanceFromFruit) * slowMotionMultiplier;
            Time.timeScale = slowMotion;
        }
        else
        {
            Time.timeScale = 1f;
        }

        Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
    }

    private void PlayRoarSFX()
    {
        AudioManager.instance.PlaySound(SOUND.BEAR_ROAR);
    }

    private void PlayImpactSFX()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }

    private void PlayImpactVFX()
    {
        impactVFX.SetActive(true);
        var mod = impactVFX.GetComponent<ParticleSystem>().main;
        mod.simulationSpeed = 2f;
        impactVFX.GetComponent<ParticleSystem>().Play();
    }

    private void PlayImpactCameraShake()
    {
        float distance = Vector2.Distance(InfiniteGameController.instance.GetFruitLocalPosition(), transform.localPosition);

        if (CameraManager.instance != null)
        {
            CameraManager.instance.ShakeCamera(distance);
        }
    }

    private void PlayWarnIndicatorVFX()
    {
        warnIndicatorVFX.SetActive(true);
        var mod = warnIndicatorVFX.GetComponent<ParticleSystem>().main;
        mod.simulationSpeed = 0.5f;
        warnIndicatorVFX.GetComponent<ParticleSystem>().Play();
    }


    // ====================================== //
    // ========== EASING FUNCTIONS ========== //
    // ====================================== //

    float EaseInCirc(float t)
    {
        return 1 - Mathf.Sqrt(1 - Mathf.Pow(t, 2));
    }

    float EaseOutCirc(float t)
    {
        return Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2));
    }
}
