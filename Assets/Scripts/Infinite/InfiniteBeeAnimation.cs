using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfiniteBeeAnimation : MonoBehaviour
{

    Animator animator;

    private Vector3 position;

    public float yMin = 0.0f;
    public float yMax = 10.0f;

    private float ySpeed = 0.6f;
    private int yDirection = 1;
    private float timeSinceLastChange = 0.0f;
    private float minChangeTime = 0.5f;
    private float maxChangeTime = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 35f;
        animator.SetBool("isBothWings", true);

        yMin += transform.position.y;
        yMax += transform.position.y;
    }

    void Update()
    {
        CalculateBeeOscillation();
    }

    private void CalculateBeeOscillation()
    {
        // Add some randomness to the bee's movement
        timeSinceLastChange -= Time.deltaTime;
        if (timeSinceLastChange <= 0.0f)
        {
            ySpeed = Random.Range(0.5f, 0.85f);
            yDirection = (Random.value < 0.5f) ? -1 : 1;
            timeSinceLastChange = Random.Range(minChangeTime, maxChangeTime);
        }

        Vector3 position = transform.position;
        position += new Vector3(0f, ySpeed * yDirection * Time.deltaTime, 0f);

        position.y = Mathf.Clamp(position.y, yMin, yMax);

        transform.position = position;

        // Reverse direction when the bee hits the top or bottom of its oscillation
        if (transform.position.y >= yMax || transform.position.y <= yMin)
        {
            yDirection *= -1;
        }
    }
}