using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    private Vector3 targetScale;
    private Vector3 originalScale;

    void Awake()
    {
        transform.localScale = Vector3.one;
        originalScale = transform.localScale;
        targetScale = transform.localScale * 1.2f;
    }

    public void OnEnable()
    {
        StartCoroutine(AnimateButton(transform.localScale, originalScale));
    }

    public void OnDisable()
    {
        transform.localScale = Vector3.one;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(AnimateButton(transform.localScale, targetScale));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(AnimateButton(transform.localScale, originalScale));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //StartCoroutine(AnimateButton(transform.localScale, targetScale));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //StartCoroutine(AnimateButton(transform.localScale, originalScale));
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(AnimateButton(transform.localScale, targetScale));
    }

    public void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine(AnimateButton(transform.localScale, originalScale));
    }

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
