using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LegacyElevatorController : MonoBehaviour
{
    public float movementSpeed = 1f;
    public float maxDifference = 0.5f;

    public Transform bottom;
    public Transform start;
    public Transform max;

    private Vector2 movementOffset;

    public Rigidbody2D leftLifter;
    public Rigidbody2D rightLifter;
    public LegacyBall ballRef;
    public Transform ballTransformRef;

    private Vector3 startPosition;

    private float leftUpInputValue;
    private float leftDownInputValue;
    private float rightUpInputValue;
    private float rightDownInputValue;

    public float maxHeight;
    public float minHeight;

    private bool inputEnabled = false;

    public float startPositionVerticalOffset = 1f;

    public Vector3 leftShaftInitialRotation;
    public Vector3 rightShaftInitialRotation;

    public Transform leftShaftTransform;
    public Transform rightShaftTransform;

    public Material material;


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

            float leftRotationAmount = 0f;
            float rightRotationAmount = 0f;

            if (leftUpInputValue != 0)
            {
                leftRotationAmount = Mathf.Lerp(0, 20f, leftUpInputValue);
            }
            if (leftDownInputValue != 0)
            {
                leftRotationAmount = Mathf.Lerp(0, -20f, leftDownInputValue);
            }
            if (rightUpInputValue != 0)
            {
                rightRotationAmount = Mathf.Lerp(0, 20f, rightUpInputValue);
            }
            if (rightDownInputValue != 0)
            {
                rightRotationAmount = Mathf.Lerp(0, -20f, rightDownInputValue);
            }

            leftShaftTransform.localRotation = Quaternion.Euler(leftShaftInitialRotation.x + leftRotationAmount, leftShaftInitialRotation.y, 0f);
            rightShaftTransform.localRotation = Quaternion.Euler(rightShaftInitialRotation.x + rightRotationAmount, rightShaftInitialRotation.y, 0f);


            Vector2 input = new Vector2(leftUpInputValue - leftDownInputValue, rightUpInputValue - rightDownInputValue);
            movementOffset = input * Time.fixedDeltaTime * movementSpeed;

            Vector2 targetLeftLifterPosition = leftLifter.position + Vector2.up * movementOffset.x;
            Vector2 targetRightLifterPosition = rightLifter.position + Vector2.up * movementOffset.y;

            if (Mathf.Abs(targetLeftLifterPosition.y - (targetRightLifterPosition.y)) <= maxDifference)
            {
                float leftTargetY = Mathf.Clamp(targetLeftLifterPosition.y, start.position.y, max.position.y);
                float rightTargetY = Mathf.Clamp(targetRightLifterPosition.y, start.position.y, max.position.y);
                leftLifter.MovePosition(new Vector2(targetLeftLifterPosition.x, leftTargetY));
                rightLifter.MovePosition(new Vector2(targetRightLifterPosition.x, rightTargetY));
            }
        }
    }

    IEnumerator MoveBarToStartPosition()
    {
        while (leftLifter.position.y < start.position.y)
        {
            Vector2 newLifterPosition = leftLifter.position + Vector2.up * movementSpeed * Time.fixedDeltaTime;
            Vector2 newRightLifterPosition = rightLifter.position + Vector2.up * movementSpeed * Time.fixedDeltaTime;

            leftLifter.MovePosition(newLifterPosition);
            rightLifter.MovePosition(newRightLifterPosition);

            yield return new WaitForEndOfFrame();
        }

        inputEnabled = true;

        LegacyGameController.instance.ReadyForNextHole();

        yield return null;
    }

    IEnumerator MoveBarToBottomPosition()
    {
        while (leftLifter.position.y > bottom.position.y || rightLifter.position.y > bottom.position.y)
        {

            if (leftLifter.position.y > bottom.position.y)
            {
                leftLifter.MovePosition(leftLifter.position - Vector2.up * movementSpeed * Time.fixedDeltaTime);
            }
            if (rightLifter.position.y > bottom.position.y)
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

    public void EnableInput(bool enableInput)
    {
        inputEnabled = enableInput;
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

    private void OnLeftEndUp(InputValue leftUpValue)
    {
        //print("UP");
        leftUpInputValue = leftUpValue.Get<float>();
    }

    private void OnLeftEndDown(InputValue leftDownValue)
    {
        //print("DOWN");
        leftDownInputValue = leftDownValue.Get<float>();
    }

    private void OnRightEndUp(InputValue rightUpValue)
    {
        //print("UP");
        rightUpInputValue = rightUpValue.Get<float>();
    }

    private void OnRightEndDown(InputValue rightDownValue)
    {
        //print("DOWN");
        rightDownInputValue = rightDownValue.Get<float>();
    }

}
