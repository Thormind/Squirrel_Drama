using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteFruit : MonoBehaviour
{
    public float fruitFallingGravityScale = 0.75f;
    public float fruitGravityScale = 1f;

    private float enterTheHoleTime = 1f;
    private float fallingFromTreeTime = 0.1f;

    Rigidbody2D fruitRigidbody;

    private Vector3 startFruitPosition;
    private Vector3 startFruitScale;

    private void Awake()
    {
        fruitRigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startFruitPosition = transform.localPosition;
        startFruitScale = transform.localScale;
        fruitRigidbody.gravityScale = fruitGravityScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.tag == "Hole")
        {
            InfiniteGameController.instance.HandleFruitInHole();

            fruitRigidbody.simulated = false;
            StartCoroutine(MoveToHoleCoroutine(collision.transform));
        }

        if (collision.transform.gameObject.tag == "Bee")
        {
            InfiniteGameController.instance.HandleFruitInBee();

            fruitRigidbody.simulated = false;
            StartCoroutine(FallFromTreeCoroutine(collision.transform));
        }

        if (collision.transform.gameObject.tag == "Worm")
        {
            InfiniteGameController.instance.HandleFruitInWorm();

            fruitRigidbody.simulated = false;
            StartCoroutine(FallFromTreeCoroutine(collision.transform));
        }

        if (collision.transform.gameObject.tag == "Bear")
        {
            InfiniteGameController.instance.HandleFruitInBear();

            fruitRigidbody.simulated = false;
            StartCoroutine(CrushedCoroutine(collision.transform));
        }

        if (collision.transform.gameObject.tag == "Points")
        {
            collision.transform.gameObject.GetComponent<InfinitePointsAnimation>().HandleFruitInPointsFunction();
            InfiniteGameController.instance.HandleFruitInPoints();
        }

        if (collision.transform.gameObject.tag == "Fruit")
        {
            collision.transform.gameObject.GetComponent<InfiniteFruitAnimation>().HandleFruitInFruitFunction();
            InfiniteGameController.instance.HandleFruitInFruit();
        }
    }

    IEnumerator MoveToHoleCoroutine(Transform holeTransform)
    {
        float t = 0;
        Vector3 fruitPosition = transform.localPosition;
        Vector3 holePosition = new Vector3(holeTransform.localPosition.x, holeTransform.localPosition.y, holeTransform.localPosition.z + 2f);

        while (t <= 1)
        {
            transform.localPosition = Vector3.Lerp(fruitPosition, holePosition, t);
            transform.localScale = startFruitScale * Mathf.Lerp(1, 0.75f, t);

            t += Time.deltaTime / enterTheHoleTime;

            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = holePosition;

        GetComponent<CircleCollider2D>().enabled = false;
        fruitRigidbody.simulated = true;

        fruitRigidbody.gravityScale = fruitFallingGravityScale;
        fruitRigidbody.velocity = Vector2.zero;
        fruitRigidbody.angularVelocity = 0;

    }

    IEnumerator FallFromTreeCoroutine(Transform obstacleTransform)
    {
        float t = 0;
        Vector3 fruitTargetPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 1f);
        Vector3 fruitPosition = transform.localPosition;

        while (t <= 1)
        {
            transform.localPosition = Vector3.Lerp(fruitPosition, fruitTargetPosition, t);

            t += Time.deltaTime / fallingFromTreeTime;

            yield return new WaitForEndOfFrame();
        }

        GetComponent<CircleCollider2D>().enabled = false;
        fruitRigidbody.simulated = true;

        fruitRigidbody.gravityScale = fruitFallingGravityScale;
        fruitRigidbody.velocity = Vector2.zero;
        fruitRigidbody.angularVelocity = 0;

    }

    IEnumerator CrushedCoroutine(Transform bearTransform)
    {
        float t = 0;
        Vector3 fruitTargetPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 5f);
        Vector3 fruitPosition = transform.localPosition;

        while (t <= 1)
        {
            transform.localPosition = Vector3.Lerp(fruitPosition, fruitTargetPosition, t);

            t += Time.deltaTime / fallingFromTreeTime;

            yield return new WaitForEndOfFrame();
        }

        GetComponent<CircleCollider2D>().enabled = false;
        fruitRigidbody.simulated = true;

        fruitRigidbody.gravityScale = fruitFallingGravityScale;
        fruitRigidbody.velocity = Vector2.zero;
        fruitRigidbody.angularVelocity = 0;

    }

    public void ResetFruitPosition(Vector3 elevatorPostion)
    {
        transform.localPosition = elevatorPostion;
        GetComponent<CircleCollider2D>().enabled = true;
        fruitRigidbody.velocity = Vector2.zero;
        fruitRigidbody.angularVelocity = 0;
        transform.localScale = startFruitScale;

        fruitRigidbody.gravityScale = fruitGravityScale;
    }

    public void QuickResetFruitPosition(Vector3 position)
    {
        transform.localPosition = position;
    }
}
