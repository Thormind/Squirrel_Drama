using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteFruit : MonoBehaviour
{
    public float enterTheHoleTime = 0.2f;

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
        startFruitPosition = transform.position;
        startFruitScale = transform.localScale;
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

        if (collision.transform.gameObject.tag == "Fruit")
        {
            collision.transform.gameObject.GetComponent<InfiniteFruitAnimation>().HandleFruitInFruitFunction();
            InfiniteGameController.instance.HandleFruitInFruit();
        }
    }

    IEnumerator MoveToHoleCoroutine(Transform holeTransform)
    {
        float t = 0;
        Vector3 fruitPosition = transform.position;

        while (t <= 1)
        {
            transform.position = Vector3.Lerp(fruitPosition, holeTransform.position, t);
            transform.localScale = startFruitScale * Mathf.Lerp(1, 0.75f, t);

            t += Time.deltaTime / enterTheHoleTime;

            yield return new WaitForEndOfFrame();
        }

        transform.position = holeTransform.position;

        GetComponent<CircleCollider2D>().enabled = false;
        fruitRigidbody.simulated = true;

        fruitRigidbody.gravityScale = 50f;
        fruitRigidbody.velocity = Vector2.zero;
        fruitRigidbody.angularVelocity = 0;

    }

    IEnumerator FallFromTreeCoroutine(Transform holeTransform)
    {
        float t = 0;
        Vector3 fruitPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1f);

        while (t <= 1)
        {
            transform.position = Vector3.Lerp(fruitPosition, holeTransform.position, t);
            transform.localScale = startFruitScale * Mathf.Lerp(0.75f, 1f, t);

            t += Time.deltaTime / enterTheHoleTime;

            yield return new WaitForEndOfFrame();
        }

        transform.position = holeTransform.position;

        GetComponent<CircleCollider2D>().enabled = false;
        fruitRigidbody.simulated = true;

        fruitRigidbody.gravityScale = 50f;
        fruitRigidbody.velocity = Vector2.zero;
        fruitRigidbody.angularVelocity = 0;

    }

    public void ResetFruitPosition()
    {
        transform.position = startFruitPosition;
        GetComponent<CircleCollider2D>().enabled = true;
        fruitRigidbody.velocity = Vector2.zero;
        fruitRigidbody.angularVelocity = 0;
        transform.localScale = startFruitScale;

        fruitRigidbody.gravityScale = 108f;
    }
}
