using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LegacyElevatorController : MonoBehaviour
{
    public float movementSpeed = 1.75f;

    public Rigidbody2D leftLifter;
    public Rigidbody2D rightLifter;
    public LegacyBall ballRef;

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

    public Vector3 leftShaftInitialRotation;
    public Vector3 rightShaftInitialRotation;

    public Transform leftShaftTransform;
    public Transform rightShaftTransform;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        minHeight = startPosition.y + startPositionVerticalOffset;
        maxHeight = minHeight + 10f;

        leftShaftInitialRotation = leftShaftTransform.rotation.eulerAngles;
        rightShaftInitialRotation = rightShaftTransform.rotation.eulerAngles;
    }

    private void FixedUpdate()
    {
        if (inputEnabled)
        {
            movementOffsetLeft = Vector3.zero;
            movementOffsetRight = Vector3.zero;

            float leftRotationAmount = 0f;
            float rightRotationAmount = 0f;

            if (leftUpInputValue != 0)
            {
                movementOffsetLeft += leftUpInputValue * Time.fixedDeltaTime * movementSpeed * Vector2.up;
                leftRotationAmount = Mathf.Lerp(0, 20f, leftUpInputValue);
            }
            if (leftDownInputValue != 0)
            {
                movementOffsetLeft += leftDownInputValue * Time.fixedDeltaTime * movementSpeed * Vector2.down;
                leftRotationAmount = Mathf.Lerp(0, -20f, leftDownInputValue);
            }
            if (rightUpInputValue != 0)
            {
                movementOffsetRight += rightUpInputValue * Time.fixedDeltaTime * movementSpeed * Vector2.up;
                rightRotationAmount = Mathf.Lerp(0, 20f, rightUpInputValue);
            }
            if (rightDownInputValue != 0)
            {
                movementOffsetRight += rightDownInputValue * Time.fixedDeltaTime * movementSpeed * Vector2.down;
                rightRotationAmount = Mathf.Lerp(0, -20f, rightDownInputValue);
            }

            leftShaftTransform.localRotation = Quaternion.Euler(leftShaftInitialRotation.x + leftRotationAmount, leftShaftInitialRotation.y, 0f);
            rightShaftTransform.localRotation = Quaternion.Euler(rightShaftInitialRotation.x + rightRotationAmount, rightShaftInitialRotation.y, 0f);

            if (Mathf.Abs(leftLifter.position.y - (rightLifter.position.y + movementOffsetRight.y)) <= maxDifference)
            {
                if (rightLifter.position.y + movementOffsetRight.y >= minHeight
                    && rightLifter.position.y + movementOffsetRight.y <= maxHeight)
                {
                    rightLifter.MovePosition(rightLifter.position + movementOffsetRight);
                }
            }

            if (Mathf.Abs((leftLifter.position.y + movementOffsetLeft.y) - rightLifter.position.y) <= maxDifference)
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
        LegacyCameraFollow.instance.isFocused = true;

        LegacyGameController.instance.ReadyForNextHole();

        yield return null;
    }

    IEnumerator MoveBarToBottomPosition()
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

        LegacyGameController.instance.ResetBall();

        if (!LegacyGameController.instance.gameCompletedState && !LegacyGameController.instance.gameOverState)
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
        LegacyCameraFollow.instance.isFocused = false;
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
        print("UP");
        leftUpInputValue = leftUpValue.Get<float>();
    }

    private void OnLeftEndDown(InputValue leftDownValue)
    {
        print("DOWN");
        leftDownInputValue = leftDownValue.Get<float>();
    }

    private void OnRightEndUp(InputValue rightUpValue)
    {
        print("UP");
        rightUpInputValue = rightUpValue.Get<float>();
    }

    private void OnRightEndDown(InputValue rightDownValue)
    {
        print("DOWN");
        rightDownInputValue = rightDownValue.Get<float>();
    }

}
