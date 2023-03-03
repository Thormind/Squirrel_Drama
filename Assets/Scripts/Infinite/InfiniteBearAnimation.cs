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
    public float shadowMaxAlpha = 255f;

    public float bearMinAlpha = 175f;
    public float bearMaxAlpha = 255f;

    private bool isCoroutineRunning = false;
    private BoxCollider2D bearCollider;


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

        float remainingCooldown = warnCooldown;

        while (remainingCooldown > 0f)
        {
            // Update countdown text
            countdownText.text = remainingCooldown.ToString("0.0");

            // Update shadow image scale and alpha
            float shadowXScale = Mathf.Lerp(shadowImage.transform.localScale.x, 0.75f, (warnCooldown - remainingCooldown) / warnCooldown);
            float shadowYScale = Mathf.Lerp(shadowImage.transform.localScale.y, 1f, (warnCooldown - remainingCooldown) / warnCooldown);
            shadowImage.transform.localScale = new Vector3(shadowXScale, shadowYScale, 1f);

            float shadowAlpha = Mathf.Lerp(shadowMinAlpha, shadowMaxAlpha, (warnCooldown - remainingCooldown) / warnCooldown);
            Color shadowColor = shadowImage.GetComponent<Image>().color;
            shadowColor.a = shadowAlpha / 255f;
            shadowImage.GetComponent<Image>().color = shadowColor;

            yield return new WaitForSeconds(0.1f);
            remainingCooldown -= 0.1f;
        }

        countdownText.text = "";

        bearPaw.SetActive(true);

        Vector3 startPosition = bearPaw.transform.localPosition;
        Vector3 endPosition = new Vector3(0f, 0f, 0f);

        Quaternion startRotation = bearPaw.transform.localRotation;
        Quaternion endRotation = Quaternion.Euler(0, 0, 0);

        float distance = Vector3.Distance(startPosition, endPosition);
        float duration = distance / translationSpeed;

        float t = 0f;

        while (t < 1f)
        {
            bearPaw.transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            bearPaw.transform.localRotation = Quaternion.Lerp(startRotation, endRotation, t);

            float bearAlpha = Mathf.Lerp(bearMinAlpha, bearMaxAlpha, t);
            Color bearColor = bearPaw.GetComponent<MeshRenderer>().material.color;
            bearColor.a = bearAlpha / 255f;
            bearPaw.GetComponent<MeshRenderer>().material.color = bearColor;

            t += Time.deltaTime / duration;
            yield return null;
        }

        bearCollider.enabled = true;
        //bearCollider.isTrigger = true;
        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / 0.01f;
            yield return null;
        }

        bearCollider.enabled = false;

        startPosition = bearPaw.transform.localPosition;
        endPosition = new Vector3(0f, 0f, -0.5f);

        //distance = Vector3.Distance(startPosition, endPosition);
        //duration = distance / translationSpeed;

        t = 0f;

        while (t < 1f)
        {
            bearPaw.transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);

            float bearAlpha = Mathf.Lerp(bearMaxAlpha, bearMinAlpha, t);
            Color bearColor = bearPaw.GetComponent<MeshRenderer>().material.color;
            bearColor.a = bearAlpha / 255f;
            bearPaw.GetComponent<MeshRenderer>().material.color = bearColor;

            float shadowAlpha = Mathf.Lerp(shadowMaxAlpha, shadowMinAlpha, t);
            Color shadowColor = shadowImage.GetComponent<Image>().color;
            shadowColor.a = shadowAlpha / 255f;
            shadowImage.GetComponent<Image>().color = shadowColor;

            float shadowXScale = Mathf.Lerp(shadowImage.transform.localScale.x, 0.6f, t);
            float shadowYScale = Mathf.Lerp(shadowImage.transform.localScale.y, 0.8f, t);
            shadowImage.transform.localScale = new Vector3(shadowXScale, shadowYScale, 1f);

            t += Time.deltaTime / delayBeforeDestroy;
            yield return null;
        }

        //yield return new WaitForSeconds(delayBeforeDestroy);

        Destroy(gameObject);

        isCoroutineRunning = false;
    }
}
