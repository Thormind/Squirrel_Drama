using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteFruit : MonoBehaviour
{
    private float fruitFallingGravityScale;
    private float fruitGravityScale;
    private float MinCollisionDistance;

    private float enterTheHoleTime = 1f;
    private float fallingFromTreeTime = 0.1f;

    public bool collisionEnabled;

    public Rigidbody2D fruitRigidbody;

    private Vector3 startFruitScale;

    private void Awake()
    {
        fruitRigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startFruitScale = transform.localScale;

        collisionEnabled = true;
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.gameObject.tag == "Hole"
            && Vector2.Distance(collision.transform.localPosition, fruitRigidbody.transform.localPosition) <= MinCollisionDistance
            && collisionEnabled)
        {
            collision.isTrigger = false;

            InfiniteGameController.instance.HandleFruitInHole();
            StartCoroutine(MoveToHoleCoroutine(collision));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collisionEnabled)
        {
            if (collision.transform.gameObject.tag == "Bee")
            {
                AudioManager.instance.PlaySound(SOUND.FRUIT_TOUCHBEE);
                InfiniteGameController.instance.HandleFruitInBee();

                StartCoroutine(FallFromTreeCoroutine(collision.transform));
            }

            if (collision.transform.gameObject.tag == "Worm")
            {
                AudioManager.instance.PlaySound(SOUND.FRUIT_FALL);
                InfiniteGameController.instance.HandleFruitInWorm();
                StartCoroutine(FallFromTreeCoroutine(collision.transform));
            }

            if (collision.transform.gameObject.tag == "Bear")
            {
                AudioManager.instance.PlaySound(SOUND.FRUIT_SQUASH);
                InfiniteGameController.instance.HandleFruitInBear();

                StartCoroutine(CrushedCoroutine(collision.transform));
            }

            if (collision.transform.gameObject.tag == "Points")
            {
                collision.enabled = false;
                collision.transform.gameObject.GetComponent<InfinitePointsAnimation>().HandleFruitInPointsFunction();
                InfiniteGameController.instance.HandleFruitInPoints();
            }

            if (collision.transform.gameObject.tag == "Fruit")
            {
                collision.enabled = false;
                collision.transform.gameObject.GetComponent<InfiniteFruitAnimation>().HandleFruitInFruitFunction();
                InfiniteGameController.instance.HandleFruitInFruit();
            }
        }
    }

    IEnumerator MoveToHoleCoroutine(Collider2D holeTransform)
    {
        fruitRigidbody.simulated = false;
        AudioManager.instance.PlaySound(SOUND.FRUIT_INHOLE);
        float t = 0;
        Vector3 fruitPosition = transform.localPosition;
        Vector3 holePosition = new Vector3(holeTransform.transform.localPosition.x, holeTransform.transform.localPosition.y, holeTransform.transform.localPosition.z + 2f);

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

        holeTransform.isTrigger = true;

    }

    IEnumerator FallFromTreeCoroutine(Transform obstacleTransform)
    {
        fruitRigidbody.simulated = false;

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
        fruitRigidbody.simulated = false;

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
        GetComponent<CircleCollider2D>().enabled = true;

        transform.localPosition = elevatorPostion;
        fruitRigidbody.velocity = Vector2.zero;
        fruitRigidbody.angularVelocity = 0;
        transform.localScale = startFruitScale;

        fruitRigidbody.gravityScale = fruitGravityScale;
    }

    public void QuickResetFruitPosition(Vector3 position)
    {
        transform.localPosition = position;
    }

    public void SetFruitGravityScale(float gravityScale)
    {
        fruitGravityScale = gravityScale;
        fruitRigidbody.gravityScale = gravityScale;
    }

    public void SetFruitFallingGravityScale(float gravityScale)
    {
        fruitFallingGravityScale = gravityScale;
    }

    public void SetFruitMinCollisionDistance(float minCollDis)
    {
        MinCollisionDistance = minCollDis;
    }
}
