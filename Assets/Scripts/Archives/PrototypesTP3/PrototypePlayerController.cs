using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypePlayerController : MonoBehaviour
{
    public float speed = 5f; // The speed at which the player moves
    public float minX = -25f; // The minimum X value for the player
    public float maxX = 25f; // The maximum X value for the player
    public float minY = -15f; // The minimum Y value for the player
    public float maxY = 15f; // The maximum Y value for the player

    private Rigidbody2D rb; // Reference to the player's Rigidbody2D componen

    // Start is called before the first frame update
    void Start()
    {
        // Get the player's Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the input axes for horizontal and vertical movement
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // Create a movement vector based on the input axes
        Vector2 moveVector = new Vector2(moveX, moveY);

        // Apply the movement vector to the player's Rigidbody2D component
        rb.velocity = moveVector * speed;

        // Clamp the player's position to stay within the limits of the zone
        Vector2 clampedPosition = rb.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        rb.position = clampedPosition;
    }
}
