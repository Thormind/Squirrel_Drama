using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyBall : MonoBehaviour
{

    public float ballGravityScale = 2.285f;

    public float ballCollisionOffset = 0.05f;

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
        ballRigidbody.gravityScale = ballGravityScale;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Time.timeScale = 0.2f;
        //print($"{Vector2.Distance(collision.transform.localPosition, ballRigidbody.transform.localPosition)}");
        // && Vector2.Distance(collision.transform.localPosition, ballRigidbody.transform.localPosition) <= ballCollisionOffset

        if (collision.transform.gameObject.tag == "Hole")
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
        Vector3 ballPosition = transform.localPosition;
        Vector3 holePosition = new Vector3(holeTransform.localPosition.x, holeTransform.localPosition.y, holeTransform.localPosition.z + 1f);

        while (t <= 1)
        {
            transform.localPosition = Vector3.Lerp(ballPosition, holePosition, t);
            transform.localScale = startBallScale * Mathf.Lerp(1, 0.75f, t);

            t += Time.deltaTime / enterTheHoleTime;

            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = holePosition;

        GetComponent<CircleCollider2D>().enabled = false;
        ballRigidbody.simulated = true;

        ballRigidbody.gravityScale = ballGravityScale * 0.5f;
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

        ballRigidbody.gravityScale = ballGravityScale;
    }
}
