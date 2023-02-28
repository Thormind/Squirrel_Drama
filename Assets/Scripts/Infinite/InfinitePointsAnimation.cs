using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinitePointsAnimation : MonoBehaviour
{
    private float movementSpeed = 0.75f;
    private float reachTheFruitTime = 0.5f;

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

        Vector3 pointsPosition = transform.position;
        Vector3 fruitPosition = InfiniteGameController.instance.GetFruitPosition();

        print($"POINTS: {pointsPosition.normalized}");
        print($"FROIT: {fruitPosition.normalized}");
        print($"DISTANCE: {Vector3.Distance(pointsPosition.normalized, fruitPosition.normalized)}");

        while (Vector3.Distance(pointsPosition.normalized, fruitPosition.normalized) > 0.02f)
        {

            print($"POINTS: {pointsPosition.normalized}");
            print($"FROIT: {fruitPosition.normalized}");
            print($"DISTANCE: {Vector3.Distance(pointsPosition.normalized, fruitPosition.normalized)}");
            //print($"POINTS: {pointsPosition}");
            //print($"FROIT: {fruitPosition}");
            //print($"DISTANCE: {Vector3.Distance(pointsPosition, fruitPosition)}");

            transform.position = Vector3.Lerp(transform.position, fruitPosition, Time.deltaTime * movementSpeed);
            //transform.localScale = startPointScale * Mathf.Lerp(1, 0.75f, t);
            //t += Time.deltaTime / reachTheFruitTime;

            yield return new WaitForEndOfFrame();

            fruitPosition = InfiniteGameController.instance.GetFruitPosition();
        }

        transform.position = InfiniteGameController.instance.GetFruitPosition();

        Destroy(gameObject);
    }
}
