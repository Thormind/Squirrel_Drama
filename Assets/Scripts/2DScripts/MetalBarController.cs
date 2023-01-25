using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalBarController : MonoBehaviour
{
    public float movementSpeed = 1.75f;

    public Rigidbody2D leftLifter;
    public Rigidbody2D rightLifter;

    private Vector3 startPosition;

    private Vector2 movementOffsetLeft = new Vector2();
    private Vector2 movementOffsetRight = new Vector2();
    public float maxDifference = 1f;

    public float maxHeight;
    public float minHeight;


    private bool inputEnabled = false;

    public float startPositionVerticalOffset = 1f;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        minHeight = startPosition.y + startPositionVerticalOffset;
        maxHeight = minHeight + 10f;
    }

    private void FixedUpdate()
    {
        if (inputEnabled)
        {
            movementOffsetLeft = Vector3.zero;
            movementOffsetRight = Vector3.zero;


            if(Input.GetKey(KeyCode.W))
            {
                movementOffsetLeft += Time.fixedDeltaTime * movementSpeed * Vector2.up;
            }

            if (Input.GetKey(KeyCode.I))
            {
                movementOffsetRight += Time.fixedDeltaTime * movementSpeed * Vector2.up;
            }

            if (Input.GetKey(KeyCode.S))
            {
                movementOffsetLeft -= Time.fixedDeltaTime * movementSpeed * Vector2.up;
            }

            if (Input.GetKey(KeyCode.K))
            {
                movementOffsetRight -= Time.fixedDeltaTime * movementSpeed * Vector2.up;
            }

            if (Mathf.Abs(leftLifter.position.y - (rightLifter.position.y + movementOffsetRight.y)) <= maxDifference)
            {
                if(rightLifter.position.y + movementOffsetRight.y >= minHeight 
                    && rightLifter.position.y + movementOffsetRight.y <= maxHeight)
                {
                    rightLifter.MovePosition(rightLifter.position + movementOffsetRight);
                } 
            }

            if (Mathf.Abs((leftLifter.position.y + movementOffsetLeft.y) -rightLifter.position.y) <= maxDifference) 
            {
                if (leftLifter.position.y + movementOffsetLeft.y >= minHeight
                     && leftLifter.position.y + movementOffsetLeft.y <= maxHeight)
                {
                    leftLifter.MovePosition(leftLifter.position + movementOffsetLeft);
                }
            }
        }
    }

    IEnumerator MoveBarToStartPosition()
    {
        while (leftLifter.position.y < startPosition.y + startPositionVerticalOffset)
        {
            leftLifter.MovePosition(leftLifter.position + Vector2.up * movementSpeed * Time.fixedDeltaTime);
            rightLifter.MovePosition(rightLifter.position + Vector2.up * movementSpeed * Time.fixedDeltaTime);

            yield return new WaitForEndOfFrame();
        }
        inputEnabled = true;

        GameController.instance.ReadyForNextHole();

        yield return null;
    }

    IEnumerator MoveBarToBottomPosition()
    {
        while (leftLifter.position.y > startPosition.y || rightLifter.position.y > startPosition.y)
        {
            if(leftLifter.position.y > startPosition.y)
            {
                leftLifter.MovePosition(leftLifter.position - Vector2.up * movementSpeed * Time.fixedDeltaTime);
            }
            if(rightLifter.position.y > startPosition.y)
            {
                rightLifter.MovePosition(rightLifter.position - Vector2.up * movementSpeed * Time.fixedDeltaTime);
            }

            yield return new WaitForEndOfFrame();
        }

        GameController.instance.ResetBall();

        if(!GameController.instance.gameCompletedState && !GameController.instance.gameOverState)
        {
            StartCoroutine(MoveBarToStartPosition());
        }

        yield return null;
    }

    [ContextMenu("Move To Start Position")]
    public void MoveBarToStartPositionFunction()
    {
        StartCoroutine(MoveBarToStartPosition());
    }

    [ContextMenu("Move To Bottom Position")]
    public void MoveBarToBottomPositionFunction()
    {
        inputEnabled = false;
        StartCoroutine(MoveBarToBottomPosition());
    }
}
