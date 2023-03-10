using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyBall : MonoBehaviour
{

    public float ballGravityScale = 2.285f;

    public float MinCollisionDistance = 0.12f;

    public float enterTheHoleTime = 5f;

    Rigidbody2D ballRigidbody;

    private Vector3 startBallPosition;
    private Vector3 startBallScale;

    public bool collisionEnabled;

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

        collisionEnabled = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //print($"{ Vector2.Distance(collision.transform.localPosition, ballRigidbody.transform.localPosition)}");
        if (collision.transform.gameObject.tag == "Hole"
            && Vector2.Distance(collision.transform.localPosition, ballRigidbody.transform.localPosition) <= MinCollisionDistance
            && collisionEnabled)
        {
            collisionEnabled = false;
            if (collision.transform.gameObject == LegacyGameController.instance.GetCurrentHole())
            {
                //Debug.Log("RIGHT HOLE");
                LegacyGameController.instance.HandleBallInHole(true);
            }
            else
            {
                //Debug.Log("WRONG HOLE");
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

    public void HideBall()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        ballRigidbody.simulated = true;

        ballRigidbody.gravityScale = ballGravityScale * 0.5f;
        ballRigidbody.velocity = Vector2.zero;
        ballRigidbody.angularVelocity = 0;
    }


    public void ResetBallPosition(Vector3 elevatorPostion)
    {


        collisionEnabled = true;

        GetComponent<CircleCollider2D>().enabled = true;

        transform.localPosition = elevatorPostion;
        ballRigidbody.velocity = Vector2.zero;
        ballRigidbody.angularVelocity = 0;
        transform.localScale = startBallScale;

        ballRigidbody.gravityScale = ballGravityScale;
    }
}
