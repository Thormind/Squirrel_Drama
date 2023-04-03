using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelIndicatorAnimation : MonoBehaviour
{
    public TMP_Text indicatorText;

    public GameObject gameObjectToAnimate;

    private float textMinSpacing = 0f;
    private float textMaxSpacing = 500f;
    private float textMinAlpha = 0f;
    private float textMaxAlpha = 255f;
    private float textScaleMultiplier = 1.25f;

    // Start is called before the first frame update
    void Start()
    {
        SetTextColor(textMinAlpha);
        SetTextWordSpacing(textMaxSpacing);
    }

    public void Show()
    {
        StartCoroutine(ShowAnimation());
    }

    IEnumerator ShowAnimation()
    {
        Vector3 startScale = Vector3.one;
        Vector3 endScale = Vector3.one * textScaleMultiplier;

        float duration = 2f;
        float t = 0f;

        while (t < 1f)
        {
            float easedProgress = EaseOutCirc(t);

            float textAlpha = Mathf.Lerp(textMinAlpha, textMaxAlpha, easedProgress);
            SetTextColor(textAlpha);

            float textSpacing = Mathf.Lerp(textMaxSpacing, textMinSpacing, easedProgress);
            SetTextWordSpacing(textSpacing);

            gameObjectToAnimate.transform.localScale = Vector3.Lerp(startScale, endScale, easedProgress);

            t += Time.deltaTime / duration;
            yield return null;
        }

        SetTextColor(textMaxAlpha);
        SetTextWordSpacing(textMinSpacing);

        duration = 1f;
        t = 0f;

        while (t < 1f)
        {
            float easedProgress = EaseInCirc(t);

            float textAlpha = Mathf.Lerp(textMaxAlpha, textMinAlpha, easedProgress);
            SetTextColor(textAlpha);

            t += Time.deltaTime / duration;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void SetTextColor(float alpha)
    {
        Color textColor = indicatorText.color;
        textColor.a = alpha / 255f;
        indicatorText.color = textColor;
    }

    private void SetTextWordSpacing(float spacing)
    {
        indicatorText.wordSpacing = spacing;
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
