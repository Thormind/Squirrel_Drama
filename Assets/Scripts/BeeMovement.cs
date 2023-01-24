using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeMovement : MonoBehaviour
{
    public float xSpeed = 1f;
    public float ySpeed = 0.25f;

    public float xMinBorder = -2.5f;
    public float xMaxBorder = 2.5f;
    public float xMinDistance;
    public float xMaxDistance;
    public float xMin = 0.45f;
    public float xMax = 0.75f;

    public float yMin = -0.1f;
    public float yMax = 0.1f;

    public float xDirection = 1f;
    public float yDirection = 1f;

    public Vector3 initialPosition;
    private Vector3 position;

    private void Start()
    {
        float randomXDirection = Mathf.Round(Random.Range(-1f, 1f));
        if (randomXDirection == 0)
        {
            randomXDirection = 1;
        }
        xDirection = randomXDirection;

        float randomYDirection = Mathf.Round(Random.Range(-1f, 1f));
        if (randomYDirection == 0)
        {
            randomYDirection = 1;
        }
        yDirection = randomYDirection;

        float xRandomDistance = Random.Range(xMin, xMax);
        xMinDistance = transform.position.x - xRandomDistance;
        xMaxDistance = transform.position.x + xRandomDistance;

        if (xMaxBorder <= xMaxDistance)
        {
            xMaxDistance = xMaxBorder;
        }
        if (xMinBorder >= xMinDistance)
        {
            xMinDistance = xMinBorder;
        }

        yMin += transform.position.y;
        yMax += transform.position.y;


    }

    void Update()
    {
        position = transform.position;
        position += new Vector3(xSpeed * xDirection * Time.deltaTime, ySpeed * yDirection * Time.deltaTime, 0f);

        position.x = Mathf.Clamp(position.x, xMinDistance, xMaxDistance);
        position.y = Mathf.Clamp(position.y, yMin, yMax);

        transform.position = position;

        if (transform.position.x >= xMaxDistance || transform.position.x <= xMinDistance)
        {
            xDirection *= -1;
        }
        if (transform.position.y >= yMax || transform.position.y <= yMin)
        {
            yDirection *= -1;
        }
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.tag == "Border")
        {
            Debug.Log("COLLIDE");
            xDirection *= -1;
        }
    }
   
}
