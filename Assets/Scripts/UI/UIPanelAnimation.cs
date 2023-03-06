using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelAnimation : MonoBehaviour
{
    private float duration = 0.5f;

    public bool startVisible = false;
    public bool isInAnimation = false;

    private RectTransform rectTransform;
    private Vector3 targetScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        targetScale = startVisible ? Vector3.one : Vector3.zero;
    }

    public void AnimateFunction(bool isIn)
    {
        isInAnimation = isIn;

        StopAllCoroutines();
        if (isIn)
        {
            StartCoroutine(Animate(targetScale, Vector3.one));
        }
        else
        {
            StartCoroutine(Animate(targetScale, Vector3.zero));
        }
    }

    private IEnumerator Animate(Vector3 startScale, Vector3 endScale)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;
        float easedTime;

        while (Time.time < endTime)
        {
            float t = Mathf.Clamp01((Time.time - startTime) / duration);

            if (isInAnimation)
            {
                easedTime = easeOutBack(t);
            }
            else
            {
                easedTime = easeInBack(t);
            }

            rectTransform.localScale = Vector3.LerpUnclamped(startScale, endScale, easedTime);
            yield return null;
        }

        rectTransform.localScale = endScale;
        targetScale = endScale;
    }

  
    private void OnValidate()
    {
        if (rectTransform != null)
        {
            targetScale = startVisible ? Vector3.one : Vector3.zero;
            rectTransform.localScale = targetScale;
        }
    }
  

    public static float easeInElastic(float x)
    {
        const float c4 = (2 * Mathf.PI) / 3;

        if (x == 0)
            return 0;
        else if (x == 1)
            return 1;
        else
            return -(Mathf.Pow(2, 10 * x - 10)) * Mathf.Sin((x * 10 - 10.75f) * c4);
    }

    public static float easeOutElastic(float x)
    {
        const float c4 = (2 * Mathf.PI) / 3;

        if (x == 0)
            return 0;
        else if (x == 1)
            return 1;
        else
            return Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * c4) + 1;
    }

    public float easeInBack(float x)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;

        return c3 * x * x * x - c1 * x * x;
    }

    float easeOutBack(float x)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;

        return 1f + c3 * Mathf.Pow(x - 1f, 3f) + c1 * Mathf.Pow(x - 1f, 2f);
    }

}
