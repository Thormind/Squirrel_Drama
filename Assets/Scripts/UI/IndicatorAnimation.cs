using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndicatorAnimation : MonoBehaviour
{
    public TMP_Text indicatorText;

    public GameObject parentToFollow;
    public GameObject gameObjectToFollow;
    public GameObject gameObjectToAnimate;

    public Vector3 position;

    private float hueSpeed = 1f;
    private float saturationSpeed = 2f;
    private float valueSpeed = 5f;
    private float minSaturation = 0.5f;
    private float minValue = 0.75f;
    private float hue;
    private float saturation;
    private float value;

    void Start()
    {
        hue = Random.Range(0f, 1f);
        value = Random.Range(0f, 1f);
    }

    void Update()
    {
        hue += hueSpeed * Time.deltaTime;
        if (hue >= 1f) hue -= 1f;
        //if (saturation >= 1f) saturation -= 1f;
        //if (value >= 1f) value -= 1f;

        //saturation = Mathf.PingPong(Time.time * saturationSpeed, 1f);
        value = Mathf.Max(minValue, Mathf.PingPong(Time.time * valueSpeed, 1f));

        Color color = Color.HSVToRGB(hue, 1f, value);
        indicatorText.color = color;
    }

    void FixedUpdate()
    {
        CalculateWorldToCanvasPoint();
    }

    public void CalculateWorldToCanvasPoint()
    {
        position.x = parentToFollow.transform.position.x;
        position.y = parentToFollow.transform.position.y;

        Camera cameraRef = Camera.main;
        gameObjectToFollow.transform.position = cameraRef.WorldToScreenPoint(position);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        Camera cameraRef = Camera.main;
        gameObjectToFollow.transform.position = cameraRef.WorldToScreenPoint(position);

        StartCoroutine(ShowAnimation());
    }

    IEnumerator ShowAnimation()
    {

        Vector3 startPosition = Vector3.zero;
        Vector3 endPosition = Vector3.up * 100f;

        float duration = 1f;
        float t = 0f;

        while (t < 1f)
        {
            float easedProgress = EaseOutCirc(t);

            gameObjectToAnimate.transform.localPosition = Vector3.Lerp(startPosition, endPosition, easedProgress);

            t += Time.deltaTime / duration;
            yield return null;
        }

        Destroy(gameObject);
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
