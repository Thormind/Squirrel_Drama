using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyBall : MonoBehaviour
{
    public float enterTheHoleTime = 5f;

    Rigidbody2D ballRigidbody;

    private Vector3 startBallPosition;
    private Vector3 startBallScale;

    private void Awake()
    {
        ballRigidbody = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        startBallPosition = transform.position;
        startBallScale = transform.localScale;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.tag == "Hole" || collision.transform.gameObject.tag == "Obstacles")
        {
            if (collision.transform.parent.gameObject == LegacyGameController.instance.GetCurrentHole())
            {
                Debug.Log("RIGHT HOLE");
                LegacyGameController.instance.HandleBallInHole(true);
            }
            else
            {
                Debug.Log("WRONG HOLE");
                LegacyGameController.instance.HandleBallInHole(false);
            }
            ballRigidbody.simulated = false;
            StartCoroutine(MoveToHoleCoroutine(collision.transform));
        }
    }

    IEnumerator MoveToHoleCoroutine(Transform holeTransform)
    {
        float t = 0;
        Vector2 ballPosition = transform.position;
        Vector2 holePosition = holeTransform.position;

        while (t <= 1)
        {
            transform.position = Vector2.Lerp(ballPosition, holePosition, t);
            transform.localScale = startBallScale * Mathf.Lerp(1, 0.75f, t);

            t += Time.deltaTime / enterTheHoleTime;

            yield return new WaitForEndOfFrame();
        }

        transform.position = holeTransform.position;

        GetComponent<CircleCollider2D>().enabled = false;
        ballRigidbody.simulated = true;

        ballRigidbody.gravityScale = 2.5f;
        ballRigidbody.velocity = Vector2.zero;
        ballRigidbody.angularVelocity = 0;

    }

    public void ResetBallPosition()
    {
        transform.position = startBallPosition;
        GetComponent<CircleCollider2D>().enabled = true;
        ballRigidbody.velocity = Vector2.zero;
        ballRigidbody.angularVelocity = 0;
        transform.localScale = startBallScale;

        ballRigidbody.gravityScale = 4.57f;
    }
}
