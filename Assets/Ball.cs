using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float enterTheHoleTime = 0.2f;

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
        if(collision.transform.gameObject.tag == "Hole")
        {
            if(collision.transform.parent.gameObject == GameController.instance.GetCurrentHole())
            {
                Debug.Log("RIGHT HOLE");
                GameController.instance.HandleBallInHole(true);
            }
            else
            {
                Debug.Log("WRONG HOLE");
                GameController.instance.HandleBallInHole(false);
            }
            ballRigidbody.simulated = false;
            StartCoroutine(MoveToHoleCoroutine(collision.transform));
        }
    }

    IEnumerator MoveToHoleCoroutine(Transform holeTransform)
    {
        float t = 0;
        Vector3 ballPosition = transform.position;

        while(t <= 1)
        {
            transform.position = Vector3.Lerp(ballPosition, holeTransform.position, t);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
