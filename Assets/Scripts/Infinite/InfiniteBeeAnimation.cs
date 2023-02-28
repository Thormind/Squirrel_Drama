using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfiniteBeeAnimation : MonoBehaviour
{

    Animator animator;

    private Vector3 position;

    private float yMin = -0.25f;
    private float yMax = 0.25f;

    private float oscillationSpeed = 2;
    private float oscillationSpeedMin = 0.75f;
    private float oscillationSpeedMax = 1.25f;

    private float wingsSpeed = 35f;
    private float yDirection = 1;
    private float timeSinceLastChange = 0.0f;
    private float minChangeTime = 0.5f;
    private float maxChangeTime = 1.5f;


    /// <summary>
    ///
    private float xSpeed = 0.85f;
    private float yRotationSpeed = 10f;

    private float xMinBorder = -2.5f;
    private float xMaxBorder = 2.5f;
    private float xMinDistance;
    private float xMaxDistance;
    private float xMin = 1f;
    private float xMax = 2f;

    private float xDirection = 1f;

    private Vector3 rightRotation;
    private Vector3 leftRotation;
    /// </summary>

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = wingsSpeed;
        animator.SetBool("isBothWings", true);

        rightRotation = Vector3.zero;
        leftRotation = new Vector3(0f, 180f, 0);

        //Y
        yMin = transform.localPosition.y + yMin;
        yMax = transform.localPosition.y + yMax;

        float randomYDirection = Mathf.Round(Random.Range(-1f, 1f));
        if (randomYDirection == 0)
        {
            randomYDirection = 1;
        }
        yDirection = randomYDirection;


        //X
        float randomXDirection = Mathf.Round(Random.Range(-1f, 1f));
        if (randomXDirection == 0)
        {
            randomXDirection = 1;
        }
        xDirection = randomXDirection;

        xMinBorder = InfiniteBeesController.instance.beesXMin;
        xMaxBorder = InfiniteBeesController.instance.beesXMax;

        float xRandomDistance = Random.Range(xMin, xMax);
        xMinDistance = transform.localPosition.x - xRandomDistance;
        xMaxDistance = transform.localPosition.x + xRandomDistance;

        if (xMaxBorder <= xMaxDistance)
        {
            xMaxDistance = xMaxBorder;
        }
        if (xMinBorder >= xMinDistance)
        {
            xMinDistance = xMinBorder;
        }
    }

    void FixedUpdate()
    {
        CalculateBeeOscillation();

        position = transform.localPosition;
        position += new Vector3(xSpeed * xDirection * Time.fixedDeltaTime, oscillationSpeed * yDirection * Time.fixedDeltaTime, 0f);
        position.y = Mathf.Clamp(position.y, yMin, yMax);
        position.x = Mathf.Clamp(position.x, xMinDistance, xMaxDistance);
        transform.localPosition = position;

        // Reverse direction when the bee hits the top or bottom of its oscillation
        if (transform.localPosition.y >= yMax || transform.localPosition.y <= yMin)
        {
            yDirection *= -1;
        }

        if (transform.localPosition.x >= xMaxDistance || transform.localPosition.x <= xMinDistance)
        {
            xDirection *= -1;

        }

        if (xDirection == 1)
        {
            Quaternion targetRotation = Quaternion.Euler(rightRotation);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * yRotationSpeed);
        }
        else
        {
            Quaternion targetRotation = Quaternion.Euler(leftRotation);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * yRotationSpeed);
        }
    }

    private void CalculateBeeOscillation()
    {
        // Add some randomness to the bee's movement
        timeSinceLastChange -= Time.deltaTime;
        if (timeSinceLastChange <= 0.0f)
        {
            oscillationSpeed = Random.Range(oscillationSpeedMin, oscillationSpeedMax);
            yDirection = (Random.value < 0.5f) ? -1 : 1;
            timeSinceLastChange = Random.Range(minChangeTime, maxChangeTime);
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