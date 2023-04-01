using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndicatorAnimation : MonoBehaviour
{
    public TMP_Text indicatorText;

    public GameObject parentToFollow;

    public GameObject gameObjectToAnimate;

    public Vector3 position;


    // Update is called once per frame
    void Update()
    {
        
    }


    void FixedUpdate()
    {
        position.x = parentToFollow.transform.position.x;
        position.y = parentToFollow.transform.position.y;
        transform.position = position;
    }

    public void Show()
    {
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
