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
    public bool isGoingIn = true;

    [SerializeField] public Vector3 targetScale;
    [SerializeField] public Vector3 targetPosition;

    private RectTransform rectTransform;
    [SerializeField] private RectTransform panelRectTransform;

    private void Awake()
    {
        targetScale = panelRectTransform.localScale;
        targetPosition = panelRectTransform.localPosition;

        isGoingIn = true;
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
                gameObject.SetActive(true);
                StartCoroutine(AnimateElastic(Vector3.zero, targetScale));
            }
            else
            {
                StartCoroutine(AnimateElastic(targetScale, Vector3.zero));
            }
        }
        if (animationType == MENU_ANIMATION_TYPE.BOUNCING)
        {
            if (isGoingIn)
            {
                gameObject.SetActive(true);
                StartCoroutine(AnimateBouncing(new Vector2(0, -Screen.height), targetPosition));
            }
            else
            {
                StartCoroutine(AnimateBouncing(targetPosition, new Vector2(0, -Screen.height)));
            }
        }
        if (animationType == MENU_ANIMATION_TYPE.QUADRATIC)
        {
            if (isGoingIn)
            {
                gameObject.SetActive(true);
                StartCoroutine(AnimateQuadratic(new Vector2(Screen.width, targetPosition.y), targetPosition));
            }
            else
            {
                StartCoroutine(AnimateQuadratic(targetPosition, new Vector2(Screen.width, targetPosition.y)));
            }
        }
        if (animationType == MENU_ANIMATION_TYPE.NO_TRANSITION)
        {

            gameObject.SetActive(isGoingIn);
            /*
            if (isGoingIn)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
            */
        }
    }








    // ==================================================== //
    // ========== ANIMATION COROUTINES FUNCTIONS ========== //
    // ==================================================== //


    private IEnumerator AnimateBouncing(Vector3 startPosition, Vector3 endPosition)
    {
        float startTime = Time.realtimeSinceStartup;
        float endTime = startTime + animationDuration;
        float easedTime;

        while (Time.realtimeSinceStartup < endTime)
        {
            float t = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / animationDuration);

            if (isGoingIn)
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

        if (!isGoingIn) 
        {  
            gameObject.SetActive(isGoingIn);  
        }
        /*
        else
        {
            //StartCoroutine(AnimateFloating());
        }
        */
    }

    private IEnumerator AnimateElastic(Vector3 startScale, Vector3 endScale)
    {
        float startTime = Time.realtimeSinceStartup;
        float endTime = startTime + animationDuration;
        float easedTime;

        while (Time.realtimeSinceStartup < endTime)
        {
            float t = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / animationDuration);

            if (isGoingIn)
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

        if (!isGoingIn) { gameObject.SetActive(isGoingIn); }

        panelRectTransform.localScale = endScale;

    }

    private IEnumerator AnimateQuadratic(Vector3 startPostion, Vector3 endPosition)
    {
        float startTime = Time.realtimeSinceStartup;
        float endTime = startTime + animationDuration;
        float easedTime;

        while (Time.realtimeSinceStartup < endTime)
        {
            float t = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / animationDuration);

            if (isGoingIn)
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

        if (!isGoingIn) { gameObject.SetActive(isGoingIn); }

        panelRectTransform.localPosition = endPosition;
    }

    private IEnumerator AnimateFloating()
    {
        float startY = panelRectTransform.anchoredPosition.y;
        float startX = panelRectTransform.anchoredPosition.x;

        Vector2 targetPos = new Vector2();

        while (isGoingIn)
        {
            float time = Time.realtimeSinceStartup;
            float animationD = startTime + Random.Range(0f, Mathf.PI * 2f);

            float newX = startX + Mathf.Sin(time * 2f + Random.Range(0f, Mathf.PI)) * floatingHeight;
            float newY = startY + Mathf.Sin(time * 2f + Random.Range(0f, Mathf.PI)) * floatingHeight;
            targetPos.x = newX;
            targetPos.y = newY;

            while (Time.realtimeSinceStartup < animationD)
            {
                float t = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / animationDuration);

                panelRectTransform.localPosition = Vector2.Lerp(panelRectTransform.localPosition, targetPos, t);

                yield return null;
            }

            yield return null;
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
