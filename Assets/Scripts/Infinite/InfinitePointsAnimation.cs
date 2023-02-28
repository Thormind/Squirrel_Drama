using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinitePointsAnimation : MonoBehaviour
{
    private float reachTheFruitTime = 2f;

    private Vector3 startPointPosition;
    private Vector3 startPointScale;

    // Start is called before the first frame update
    void Start()
    {
        startPointPosition = transform.localPosition;
        startPointScale = transform.localScale;
    }

    public void HandleFruitInPointsFunction()
    {
        StartCoroutine(HandleFruitInPoints());
    }

    IEnumerator HandleFruitInPoints()
    {
        float t = 0;

        Vector3 fruitPosition;

        while (t <= 1)
        {
            fruitPosition = InfiniteGameController.instance.GetFruitLocalPosition();

            transform.localPosition = Vector3.Lerp(transform.localPosition, fruitPosition, t);
            transform.localScale = startPointScale * Mathf.Lerp(1, 0.25f, t);
            t += Time.fixedDeltaTime / reachTheFruitTime;

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
