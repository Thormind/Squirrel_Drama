using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum MENU_ANIMATION_TYPE
{
    NO_TRANSITION,
    BOUNCING,
    QUADRATIC,
    ELASTIC,
};

public class UIAnimation : MonoBehaviour
{
    [SerializeField] public float animationDuration = 1f;
    [SerializeField] public MENU_ANIMATION_TYPE animationType;

    private float startTime;

    public float floatingHeight;

    public float easedProgress;
    public bool startVisible = false;
    public bool isVisible = false;
    public bool isGoingIn = true;

    [SerializeField] public Vector3 targetScale;
    [SerializeField] public Vector3 targetPosition;

    private RectTransform rectTransform;
    [SerializeField] private RectTransform panelRectTransform;

    private void Awake()
    {
        //targetScale = startVisible ? Vector3.one : Vector3.zero;
        targetScale = panelRectTransform.localScale;
        targetPosition = panelRectTransform.localPosition;

        isGoingIn = true;
        isVisible = false;
    }





    // =============================================== //
    // ========== MAIN ANIMATION CONTROLLER ========== //
    // =============================================== //

    public void play()
    {
        // Get the RectTransform component of the UI element
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;

        // Set the start time for the animation
        startTime = Time.realtimeSinceStartup;

        StopAllCoroutines();

        if (animationType == MENU_ANIMATION_TYPE.ELASTIC)
        {
            if (isGoingIn)
            {
                StartCoroutine(AnimateElastic(Vector3.zero, targetScale, isGoingIn));
            }
            else
            {
                StartCoroutine(AnimateElastic(targetScale, Vector3.zero, isGoingIn));
            }
        }
        if (animationType == MENU_ANIMATION_TYPE.BOUNCING)
        {
            if (isGoingIn)
            {
                StartCoroutine(AnimateBouncing(new Vector2(0, -Screen.height), targetPosition, isGoingIn));
            }
            else
            {
                StartCoroutine(AnimateBouncing(targetPosition, new Vector2(0, -Screen.height), isGoingIn));
            }
        }
        if (animationType == MENU_ANIMATION_TYPE.QUADRATIC)
        {
            if (isGoingIn)
            {
                StartCoroutine(AnimateQuadratic(new Vector2(Screen.width, targetPosition.y), targetPosition, isGoingIn));
            }
            else
            {
                StartCoroutine(AnimateQuadratic(targetPosition, new Vector2(Screen.width, targetPosition.y), isGoingIn));
            }
        }
        if (animationType == MENU_ANIMATION_TYPE.NO_TRANSITION)
        {
            if (isGoingIn)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        isGoingIn = !isGoingIn;

        isVisible = !isVisible;

    }








    // ==================================================== //
    // ========== ANIMATION COROUTINES FUNCTIONS ========== //
    // ==================================================== //


    private IEnumerator AnimateBouncing(Vector3 startPosition, Vector3 endPosition, bool p_isGoingIn)
    {
        float startTime = Time.realtimeSinceStartup;
        float endTime = startTime + animationDuration;
        float easedTime;

        while (Time.realtimeSinceStartup < endTime)
        {
            float t = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / animationDuration);

            if (p_isGoingIn)
            {
                easedTime = easeOutBack(t);
            }
            else
            {
                easedTime = easeInBack(t);
            }

            panelRectTransform.localPosition = Vector3.LerpUnclamped(startPosition, endPosition, easedTime);
            yield return null;
        }

        panelRectTransform.localPosition = endPosition;
        targetPosition = endPosition;

        StartCoroutine(AnimateFloating());

    }

    private IEnumerator AnimateElastic(Vector3 startScale, Vector3 endScale, bool p_isGoingIn)
    {
        float startTime = Time.realtimeSinceStartup;
        float endTime = startTime + animationDuration;
        float easedTime;

        while (Time.realtimeSinceStartup < endTime)
        {
            float t = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / animationDuration);

            if (p_isGoingIn)
            {
                easedTime = easeOutBack(t);
            }
            else
            {
                easedTime = easeInBack(t);
            }

            panelRectTransform.localScale = Vector3.LerpUnclamped(startScale, endScale, easedTime);
            yield return null;
        }

        panelRectTransform.localScale = endScale;
        targetScale = endScale;

    }

    private IEnumerator AnimateQuadratic(Vector3 startPostion, Vector3 endPosition, bool p_isGoingIn)
    {
        float startTime = Time.realtimeSinceStartup;
        float endTime = startTime + animationDuration;
        float easedTime;

        while (Time.realtimeSinceStartup < endTime)
        {
            float t = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / animationDuration);

            if (p_isGoingIn)
            {
                easedTime = easeOutBack(t);
            }
            else
            {
                easedTime = easeInBack(t);
            }

            panelRectTransform.localPosition = Vector3.LerpUnclamped(startPostion, endPosition, easedTime);
            yield return null;
        }

        panelRectTransform.localPosition = endPosition;
        targetPosition = endPosition;

    }

    private IEnumerator AnimateFloating()
    {
        float startY = panelRectTransform.anchoredPosition.y;
        float time = 0f;

        while (isVisible)
        {
            while (time < animationDuration)
            {
                float newY = startY + Mathf.Sin(time * Mathf.PI) * floatingHeight;
                panelRectTransform.anchoredPosition = new Vector2(panelRectTransform.anchoredPosition.x, newY);

                time += Time.deltaTime;
                yield return null;
            }

            time = 0f;
        }
    }






    // ====================================== //
    // ========== EASING FUNCTIONS ========== //
    // ====================================== //

    float QuadraticEasing(float t)
    {
        // Quadratic easing in and out
        if (t < 0.5f)
            return 2 * t * t;
        else
            return -1 + (4 - 2 * t) * t;
    }

    float BounceEasing(float t)
    {
        // Bounce easing
        if (t < (1 / 2.75))
        {
            return 7.5625f * t * t;
        }
        else if (t < (2 / 2.75))
        {
            return 7.5625f * (t -= (1.5f / 2.75f)) * t + 0.75f;
        }
        else if (t < (2.5 / 2.75))
        {
            return 7.5625f * (t -= (2.25f / 2.75f)) * t + 0.9375f;
        }
        else
        {
            return 7.5625f * (t -= (2.625f / 2.75f)) * t + 0.984375f;
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
