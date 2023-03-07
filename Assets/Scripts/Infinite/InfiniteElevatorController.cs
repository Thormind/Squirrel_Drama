using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InfiniteElevatorController : MonoBehaviour
{
    private float resetMovementSpeed = 100f;
    private float bottomMovementSpeed = 1f;
    private float startMovementSpeed = 1f;

    public float movementSpeed = 0.6f;
    private float maxDifference = 0.3f;

    public Transform bottom;
    public Transform start;
    public Transform end;

    public Rigidbody2D leftLifter;
    public Rigidbody2D rightLifter;

    private Vector2 movementOffset;

    private float leftUpInputValue;
    private float leftDownInputValue;
    private float rightUpInputValue;
    private float rightDownInputValue;

    private bool inputEnabled = false;

    private void FixedUpdate()
    {
        if (inputEnabled)
        {
            if (leftLifter.position.y >= end.position.y
                || rightLifter.position.y >= end.position.y)
            {
                InfiniteGameController.instance.LevelCompleted();
            }

            Vector2 input = new Vector2(leftUpInputValue - leftDownInputValue, rightUpInputValue - rightDownInputValue);
            movementOffset = input * Time.fixedDeltaTime * movementSpeed;

            Vector2 targetLeftLifterPosition = leftLifter.position + Vector2.up * movementOffset.x;
            Vector2 targetRightLifterPosition = rightLifter.position + Vector2.up * movementOffset.y;

            if (Mathf.Abs(targetLeftLifterPosition.y - (targetRightLifterPosition.y)) <= maxDifference)
            {
                float leftTargetY = Mathf.Clamp(targetLeftLifterPosition.y, start.position.y, end.position.y);
                float rightTargetY = Mathf.Clamp(targetRightLifterPosition.y, start.position.y, end.position.y);
                leftLifter.MovePosition(new Vector2(targetLeftLifterPosition.x, leftTargetY));
                rightLifter.MovePosition(new Vector2(targetRightLifterPosition.x, rightTargetY));
            }
        }
    }

    IEnumerator MoveBarToStartPosition()
    {
        while (leftLifter.position.y < start.position.y)
        {
            Vector2 newLifterPosition = leftLifter.position + Vector2.up * startMovementSpeed * Time.fixedDeltaTime;
            Vector2 newRightLifterPosition = rightLifter.position + Vector2.up * startMovementSpeed * Time.fixedDeltaTime;

            leftLifter.MovePosition(newLifterPosition);
            rightLifter.MovePosition(newRightLifterPosition);

            yield return new WaitForEndOfFrame();
        }

        inputEnabled = true;

        InfiniteGameController.instance.ReadyForLevel();

        yield return null;
    }

    IEnumerator MoveBarToBottomPosition()
    {
        bottomMovementSpeed = CalculateMoveBarToBottomSpeed();

        while (leftLifter.position.y > bottom.position.y || rightLifter.position.y > bottom.position.y)
        {

            if (leftLifter.position.y > bottom.position.y)
            {
                leftLifter.MovePosition(leftLifter.position - Vector2.up * bottomMovementSpeed * Time.fixedDeltaTime);
            }
            if (rightLifter.position.y > bottom.position.y)
            {
                rightLifter.MovePosition(rightLifter.position - Vector2.up * bottomMovementSpeed * Time.fixedDeltaTime);
            }

            yield return new WaitForEndOfFrame();
        }

        InfiniteGameController.instance.ResetFruit();


        if (InfiniteGameController.instance.levelCompletedState)
        {
            InfiniteGameController.instance.StartGame();
        }
        if (!InfiniteGameController.instance.gameOverState)
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

    public float CalculateMoveBarToBottomSpeed()
    {
        return Mathf.Abs(1f + transform.localPosition.y) * 0.5f;
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






    // ========== DEV FUNCTIONS ========== //

    public void QuickBarResetFunction()
    {
        inputEnabled = false;
        StartCoroutine(QuickBarResetToBottom());
    }

    IEnumerator QuickBarResetToBottom()
    {
        while (leftLifter.position.y > bottom.position.y || rightLifter.position.y > bottom.position.y)
        {

            if (leftLifter.position.y > bottom.position.y)
            {
                leftLifter.MovePosition(leftLifter.position - Vector2.up * resetMovementSpeed * Time.fixedDeltaTime);
            }
            if (rightLifter.position.y > bottom.position.y)
            {
                rightLifter.MovePosition(leftLifter.position - Vector2.up * resetMovementSpeed * Time.fixedDeltaTime);
            }

            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(QuickBarResetToStart());

        yield return null;
    }

    IEnumerator QuickBarResetToStart()
    {
        while (leftLifter.position.y < start.position.y)
        {
            leftLifter.MovePosition(leftLifter.position + Vector2.up * resetMovementSpeed * Time.fixedDeltaTime);
            rightLifter.MovePosition(rightLifter.position + Vector2.up * resetMovementSpeed * Time.fixedDeltaTime);

            yield return new WaitForEndOfFrame();
        }

        inputEnabled = true;

        InfiniteGameController.instance.SpawnObstacles();

        InfiniteGameController.instance.QuickResetFruit();

        yield return null;
    }
}
