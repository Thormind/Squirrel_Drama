using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ElevatorController : MonoBehaviour
{
    public float movementSpeed = 1.75f;

    public Rigidbody2D leftLifter;
    public Rigidbody2D rightLifter;
    public Ball ballRef;

    private Vector3 startPosition;

    private Vector2 movementOffsetLeft = new Vector2();
    private Vector2 movementOffsetRight = new Vector2();
    private float leftUpInputValue;
    private float leftDownInputValue;
    private float rightUpInputValue;
    private float rightDownInputValue;

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

            if (leftUpInputValue != 0)
            {
                movementOffsetLeft += leftUpInputValue * Time.fixedDeltaTime * movementSpeed * Vector2.up;
            }
            if (leftDownInputValue != 0)
            {
                movementOffsetLeft += leftDownInputValue * Time.fixedDeltaTime * movementSpeed * Vector2.down;
            }
            if (rightUpInputValue != 0)
            {
                movementOffsetRight += rightUpInputValue * Time.fixedDeltaTime * movementSpeed * Vector2.up;
            }
            if (rightDownInputValue != 0)
            {
                movementOffsetRight += rightDownInputValue * Time.fixedDeltaTime * movementSpeed * Vector2.down;
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

    IEnumerator ResetStartPosition()
    {
        while (leftLifter.position.y < startPosition.y + startPositionVerticalOffset)
        {
            leftLifter.MovePosition(leftLifter.position + Vector2.up * movementSpeed * Time.fixedDeltaTime);
            rightLifter.MovePosition(rightLifter.position + Vector2.up * movementSpeed * Time.fixedDeltaTime);

            yield return new WaitForEndOfFrame();
        }
        inputEnabled = true;

        yield return null;
    }

    IEnumerator ResetBottomPosition()
    {
        while (leftLifter.position.y > startPosition.y || rightLifter.position.y > startPosition.y)
        {
            if (leftLifter.position.y > startPosition.y)
            {
                leftLifter.MovePosition(leftLifter.position - Vector2.up * movementSpeed * Time.fixedDeltaTime);
            }
            if (rightLifter.position.y > startPosition.y)
            {
                rightLifter.MovePosition(rightLifter.position - Vector2.up * movementSpeed * Time.fixedDeltaTime);
            }

            yield return new WaitForEndOfFrame();
        }

        ballRef.ResetBallPosition();

        StartCoroutine(ResetStartPosition());

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

    [ContextMenu("Reset Start Position")]
    public void ResetStartPositionFunction()
    {
        StartCoroutine(ResetStartPosition());
    }

    [ContextMenu("Reset Bottom Position")]
    public void ResetBottomPositionFunction()
    {
        inputEnabled = false;
        StartCoroutine(ResetBottomPosition());
    }

    private void OnLeftEndUp(InputValue leftUpValue)
    {
        leftUpInputValue = leftUpValue.Get<float>();
    }

    private void OnLeftEndDown(InputValue leftDownValue)
    {
        leftDownInputValue = leftDownValue.Get<float>();
    }

    private void OnRightEndUp(InputValue rightUpValue)
    {
        rightUpInputValue = rightUpValue.Get<float>();
    }

    private void OnRightEndDown(InputValue rightDownValue)
    {
        rightDownInputValue = rightDownValue.Get<float>();
    }

}
