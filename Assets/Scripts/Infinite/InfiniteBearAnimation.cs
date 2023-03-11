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

    private float warnCooldown;

    public float translationSpeed = 10f;
    public float delayBeforeDestroy = 0.05f;

    public float shadowMinScale = 0.25f;

    public float shadowMinAlpha = 100f;
    public float shadowMaxAlpha = 200f;

    public float bearMinAlpha = 170f;
    public float bearMaxAlpha = 255f;

    private bool isCoroutineRunning = false;
    private BoxCollider2D bearCollider;

    public GameObject impactVFX;


    private void Start()
    {
        bearCollider = GetComponent<BoxCollider2D>();
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
        AudioManager.instance.PlaySound(SOUND.BEAR_ROAR);
        while (remainingCooldown > 0f)
        {
            // Update countdown text
            countdownText.text = remainingCooldown.ToString("0.0");

            // Update shadow image scale and alpha
            float shadowXScale = Mathf.Lerp(shadowImage.transform.localScale.x, 0.75f, (warnCooldown - remainingCooldown) / warnCooldown);
            float shadowYScale = Mathf.Lerp(shadowImage.transform.localScale.y, 1f, (warnCooldown - remainingCooldown) / warnCooldown);
            shadowImage.transform.localScale = new Vector3(shadowXScale, shadowYScale, 1f);

            float shadowAlpha = Mathf.Lerp(shadowMinAlpha, shadowMaxAlpha, (warnCooldown - remainingCooldown) / warnCooldown);
            SetShadowAlpha(shadowAlpha);

            yield return new WaitForSeconds(0.1f);
            remainingCooldown -= 0.1f;
        }

        countdownText.text = "";


        // ============== SLOW MOTION BEFORE IMPACT ANIMATION ============== //
        InfiniteGameController.instance.SetRigidBodyExtrapolate(true);
        bearPaw.SetActive(true);

        SetSlowMotion(true);

        Vector3 startPosition = bearPaw.transform.localPosition;
        Vector3 endPosition = new Vector3(0f, 0f, 0f);

        Quaternion startRotation = bearPaw.transform.localRotation;
        Quaternion endRotation = Quaternion.Euler(0, 0, 0);

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
        AudioManager.instance.PlaySound(SOUND.BEAR_HIT);

        bearPaw.transform.localPosition = endPosition;
        bearPaw.transform.localRotation = endRotation;
        SetBearAlpha(bearMaxAlpha);

        // ============== IMPACT ANIMATION ============== //
        SetSlowMotion(false);

        bearCollider.enabled = true;
        InfiniteGameController.instance.SetRigidBodyExtrapolate(false);

        PlayImpactVFX();
        PlayImpactCameraShake();

        yield return new WaitForSeconds(0.25f);

        bearCollider.enabled = false;
        bearPaw.GetComponent<BoxCollider2D>().enabled = true;

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / 1f;
            yield return null;
        }

        bearPaw.GetComponent<BoxCollider2D>().enabled = false;


        // ============== POST IMPACT ANIMATION ============== //

        startPosition = bearPaw.transform.localPosition;
        endPosition = new Vector3(0f, 0f, -0.5f);

        t = 0f;

        while (t < 1f)
        {
            float easedProgress = EaseOutCirc(t);

            float bearAlpha = Mathf.Lerp(bearMaxAlpha, bearMinAlpha, easedProgress);
            SetBearAlpha(bearAlpha);

            float shadowAlpha = Mathf.Lerp(shadowMaxAlpha, shadowMinAlpha, easedProgress);
            SetShadowAlpha(shadowAlpha);

            float shadowXScale = Mathf.Lerp(shadowImage.transform.localScale.x, 0.6f, easedProgress);
            float shadowYScale = Mathf.Lerp(shadowImage.transform.localScale.y, 0.8f, easedProgress);
            shadowImage.transform.localScale = new Vector3(shadowXScale, shadowYScale, 1f);

            bearPaw.transform.localPosition = Vector3.Lerp(startPosition, endPosition, easedProgress);

            t += Time.deltaTime / delayBeforeDestroy;
            yield return null;
        }

        //yield return new WaitForSeconds(delayBeforeDestroy);

        Destroy(gameObject);

        isCoroutineRunning = false;
    }

    // ====================================== //
    // ====================================== //

    private void SetShadowAlpha(float shadowAlpha)
    {
        Color shadowColor = shadowImage.GetComponent<Image>().color;
        shadowColor.a = shadowAlpha / 255f;
        shadowImage.GetComponent<Image>().color = shadowColor;
    }

    private void SetBearAlpha(float bearAlpha)
    {
        Color bearColor = bearPaw.GetComponent<MeshRenderer>().material.color;
        bearColor.a = bearAlpha / 255f;
        bearPaw.GetComponent<MeshRenderer>().material.color = bearColor;
    }

    private void SetSlowMotion(bool isSlowMotion)
    {
        if (isSlowMotion)
        {
            float distanceFromFruit = Vector2.Distance(InfiniteGameController.instance.GetFruitLocalPosition(), transform.localPosition);
            float slowMotion = Mathf.Clamp(distanceFromFruit, 2f, 7f) * 0.15f;
            Time.timeScale = slowMotion;
        }
        else
        {
            Time.timeScale = 1f;
        }

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
