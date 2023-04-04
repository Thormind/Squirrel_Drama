using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    private Vector3 confirmTargetScale;
    private Vector3 selectTargetScale;
    private Vector3 originalScale;

    void Awake()
    {
        transform.localScale = Vector3.one;
        originalScale = transform.localScale;
        selectTargetScale = transform.localScale * 1.2f;
        confirmTargetScale = transform.localScale * 0.8f;
    }

    public void OnEnable()
    {
        StartCoroutine(AnimateButton(transform.localScale, originalScale));
    }

    public void OnDisable()
    {
        transform.localScale = Vector3.one;
    }

    // ===== SELECT ===== //

    public void OnSelect(BaseEventData eventData)
    {
        AudioManager.instance.PlaySound(SOUND.MOUSEOVER);
        StartCoroutine(AnimateButton(transform.localScale, selectTargetScale));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlaySound(SOUND.MOUSEOVER);
        StartCoroutine(AnimateButton(transform.localScale, selectTargetScale));
    }

    // ===== DESELECT ===== //

    public void OnPointerUp(PointerEventData eventData)
    {
        StartCoroutine(AnimateButton(transform.localScale, originalScale));
    }

    public void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine(AnimateButton(transform.localScale, originalScale));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(AnimateButton(transform.localScale, originalScale));
    }

    // ===== CONFIRM ===== //

    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.instance.PlaySound(SOUND.CLICK);
        StartCoroutine(AnimateButton(transform.localScale, confirmTargetScale));
    }


    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.instance.PlaySound(SOUND.CLICK);
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(AnimateSubmitButton(transform.localScale, confirmTargetScale));
        }

    }

    public void OnManualSubmit()
    {
        AudioManager.instance.PlaySound(SOUND.CLICK);
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(AnimateSubmitButton(transform.localScale, confirmTargetScale));
        }

    }


    // ===== ANIMATION ===== //

    private IEnumerator AnimateButton(Vector3 startScale, Vector3 endScale)
    {
        float animationDuration = 0.35f;
        float easedTime;

        float t = 0f;

        while (t < 1f)
        {
            easedTime = EaseOutQuint(t);

            transform.localScale = Vector3.LerpUnclamped(startScale, endScale, easedTime);

            t += Time.fixedDeltaTime / animationDuration;

            yield return null;
        }

        // Ensure the scale ends exactly at the target scale.
        transform.localScale = endScale;
    }

    private IEnumerator AnimateSubmitButton(Vector3 startScale, Vector3 endScale)
    {
        float animationDuration = 0.35f;
        float easedTime;

        float t = 0f;

        while (t < 1f)
        {
            easedTime = EaseOutQuint(t);

            transform.localScale = Vector3.LerpUnclamped(startScale, endScale, easedTime);

            t += Time.fixedDeltaTime / animationDuration;

            yield return null;
        }

        // Ensure the scale ends exactly at the target scale.
        transform.localScale = endScale;

        t = 0f;

        while (t < 1f)
        {
            easedTime = EaseOutQuint(t);

            transform.localScale = Vector3.LerpUnclamped(endScale, originalScale, easedTime);

            t += Time.fixedDeltaTime / animationDuration;

            yield return null;
        }

        transform.localScale = originalScale;
    }



    // ====================================== //
    // ========== EASING FUNCTIONS ========== //
    // ====================================== //

    float EaseOutQuint(float x)
    {
        return 1 - Mathf.Pow(1 - x, 5);
    }
    float EaseInQuint(float x)
    {
        return x * x * x * x * x;
    }
}
