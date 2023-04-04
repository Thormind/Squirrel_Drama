using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinitePointsAnimation : MonoBehaviour
{
    public GameObject pointModel;

    private float rotationSpeed = 25f;
    private float reachTheFruitTime = 3f;
    private float startPointScaleMultiplier = 1f;
    private float endPointScaleMultiplier = 0.25f;

    private Vector3 startPointPosition;
    private Vector3 startPointScale;

    // Start is called before the first frame update
    void Start()
    {
        startPointPosition = transform.localPosition;
        startPointScale = transform.localScale;
    }

    void Update()
    {
        pointModel.transform.Rotate(Vector3.up * Time.fixedDeltaTime * rotationSpeed, Space.World);
    }


    public void HandleFruitInPointsFunction()
    {
        StartCoroutine(HandleFruitInPoints());
    }

    IEnumerator HandleFruitInPoints()
    {
        float t = 0;

        Vector3 fruitPosition;
        bool soundPlayed = false;

        while (t <= 1)
        {
            if(!soundPlayed)
            {
                AudioManager.instance.PlaySound(SOUND.POINT_GRAB);
                soundPlayed = true;
            }

            fruitPosition = InfiniteGameController.instance.GetFruitLocalPosition();

            transform.localPosition = Vector3.Lerp(transform.localPosition, fruitPosition, t);
            transform.localScale = startPointScale * Mathf.Lerp(startPointScaleMultiplier, endPointScaleMultiplier, t);
            t += Time.fixedDeltaTime / reachTheFruitTime;

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
