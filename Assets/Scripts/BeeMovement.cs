using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeMovement : MonoBehaviour
{
    public float speed = 1f;
    public float xMin = -10f;
    public float xMax = 10f;
    public float yMin = -0.5f;
    public float yMax = 0.5f;

    private float direction = 1f;
    private Vector3 position;

    void Update()
    {
        position = transform.position;
        position += new Vector3(speed * direction * Time.deltaTime, 0f, 0f);
        position.x = Mathf.Clamp(position.x, xMin, xMax);
        transform.position = position;

        if (transform.position.x >= xMax || transform.position.x <= xMin)
        {
            direction *= -1;
        }
    }
}
