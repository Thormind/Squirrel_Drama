using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum MENU_ANIMATION_TYPE
{
    NO_TRANSITION,
    BOUNCING,
    QUADRATIC,
    ACTIVE
};

public class UIAnimation : MonoBehaviour
{
    public float animationDuration = 1f;
    public int animationInOut = 0;

    public MENU_ANIMATION_TYPE animationType;

    [SerializeField] public Vector2 startPosition;
    [SerializeField] public Vector2 endPosition;

    [SerializeField] public Vector3 startScale;
    [SerializeField] public Vector3 endScale;

    private RectTransform rectTransform;
    private float startTime;

    private bool isPlaying = false;

    public float easedProgress;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isPlaying)
        {
            playAnimationInternal();
        }
    }

    public void play()
    {
        // Get the RectTransform component of the UI element
        rectTransform = GetComponent<RectTransform>();

        // Set the start time for the animation
        startTime = Time.realtimeSinceStartup;

        isPlaying = true;
    }

    private void playAnimationInternal()
    {
        // Calculate the progress of the animation
        float progress = (Time.realtimeSinceStartup - startTime) / animationDuration;

        // Use the animation curve to ease the animation
        if (animationType == MENU_ANIMATION_TYPE.NO_TRANSITION)
        {
            playNoAnimationInternal();
        }
        if (animationType == MENU_ANIMATION_TYPE.BOUNCING)
        {
            easedProgress = BounceEasing(progress);
            playBouncingAnimationInternal();
        }
        if (animationType == MENU_ANIMATION_TYPE.QUADRATIC)
        {
            easedProgress = QuadraticEasing(progress);
            playQuadraticAnimationInternal();
        }
        if (animationType == MENU_ANIMATION_TYPE.ACTIVE)
        {
            playActiveAnimationInternal();
        }

        // If the animation has completed, stop it
        if (progress >= 1f)
        {
            isPlaying = false;
            //enabled = false;
            //Destroy(this);
        }
    }

    private void playNoAnimationInternal()
    {
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

        // Set the position of the UI element based on the eased progress
        if (animationInOut == 1)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, easedProgress);
        }
        else
        {
            rectTransform.anchoredPosition = Vector2.Lerp(endPosition, startPosition, easedProgress);
        }
    }

    private void playBouncingAnimationInternal()
    {
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

        // Set the position of the UI element based on the eased progress
        if (animationInOut == 1)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, easedProgress);
        }
        else
        {
            rectTransform.anchoredPosition = Vector2.Lerp(endPosition, startPosition, easedProgress);
        }
    }

    private void playQuadraticAnimationInternal()
    {
        startPosition = new Vector2(Screen.width/5, 0);  
        endPosition = Vector2.zero; 

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

        // Set the position of the UI element based on the eased progress
        if (animationInOut == 1)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, easedProgress);
        }
        else
        {
            rectTransform.anchoredPosition = Vector2.Lerp(endPosition, startPosition, easedProgress);
        }
    }

    private void playActiveAnimationInternal()
    {

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

        rectTransform.anchoredPosition = Vector2.zero;

        if (animationInOut == 1)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

    float QuadraticEasing(float t)
    {
        // Quadratic easing in and out
        if (t < 0.5f)
            return 2 * t * t;
        else
            return -1 + (4 - 2 * t) * t;
    }

    float SineEasing(float t)
    {
       return -Mathf.Cos(t * (Mathf.PI / 2)) + 1;
    }

    float ZoomingEasing(float t)
    {
        // Zooming in and out
        float value = Mathf.PingPong(t, 1.0f);
        return Mathf.SmoothStep(0.5f, 1.5f, value);
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

    float BackEasing(float t)
    {
        float t1 = 1.70158f;
        float t2 = 2.70158f;
        return t * t * (t2 * t - t1);

    }
}
