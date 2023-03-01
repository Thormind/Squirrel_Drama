using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InfiniteElevatorController : MonoBehaviour
{
    private float bottomMovementSpeed = 60f;
    private float startMovementSpeed = 45f;
    private float movementSpeed = 30f;
    private float resetMovementSpeed = 500f;

    public Rigidbody2D leftLifter;
    public Rigidbody2D rightLifter;
    public Vector2 leftLifterPosition;
    public Vector2 rightLifterPosition;

    public Transform fruitTransformRef;

    private Vector3 bottomPosition;
    private Vector3 startPosition;

    private Vector2 movementOffsetLeft = new Vector2();
    private Vector2 movementOffsetRight = new Vector2();
    private float leftUpInputValue;
    private float leftDownInputValue;
    private float rightUpInputValue;
    private float rightDownInputValue;

    public float maxDifference = 6f;

    public float maxHeight;
    public float minHeight;

    private bool inputEnabled = false;

    public float startPositionVerticalOffset = 6f;

    // Start is called before the first frame update
    void Start()
    {
        bottomPosition = transform.localPosition;
        startPosition = bottomPosition + new Vector3(0, startPositionVerticalOffset, 0);
        minHeight = startPosition.y;
        maxHeight = minHeight + 40.5f;
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

            leftLifterPosition = leftLifter.transform.localPosition;
            rightLifterPosition = rightLifter.transform.localPosition;

            Vector2 leftLifterParentPosition = leftLifter.transform.parent.localPosition;
            Vector2 rightLifterParentPosition = rightLifter.transform.parent.localPosition;

            Vector2 leftWorldPos = leftLifter.transform.TransformPoint(leftLifterParentPosition);
            Vector2 rightWorldPos = rightLifter.transform.TransformPoint(rightLifterParentPosition);

            Vector2 newLeftLifterPosition = leftWorldPos + movementOffsetLeft;
            Vector2 newRightLifterPosition = rightWorldPos + movementOffsetRight;

            if (Mathf.Abs(leftLifterPosition.y - (rightLifterPosition.y + movementOffsetRight.y)) <= maxDifference)
            {
                if (rightLifterPosition.y + movementOffsetRight.y >= minHeight
                    && rightLifterPosition.y + movementOffsetRight.y <= maxHeight)
                {
                    rightLifter.MovePosition(newRightLifterPosition);
                }
            }

            if (Mathf.Abs((leftLifterPosition.y + movementOffsetLeft.y) - rightLifterPosition.y) <= maxDifference)
            {
                if (leftLifterPosition.y + movementOffsetLeft.y >= minHeight
                     && leftLifterPosition.y + movementOffsetLeft.y <= maxHeight)
                {
                    leftLifter.MovePosition(newLeftLifterPosition);
                }
            }

            if (rightLifterPosition.y + movementOffsetRight.y >= maxHeight
                || leftLifterPosition.y + movementOffsetLeft.y >= maxHeight)
            {
                InfiniteGameController.instance.LevelCompleted();
            }
        }
    }

    IEnumerator MoveBarToStartPosition()
    {
        while (leftLifter.transform.localPosition.y < bottomPosition.y + startPositionVerticalOffset)
        {
            leftLifterPosition = leftLifter.transform.localPosition;
            rightLifterPosition = rightLifter.transform.localPosition;

            Vector2 leftLifterParentPosition = leftLifter.transform.parent.localPosition;
            Vector2 rightLifterParentPosition = rightLifter.transform.parent.localPosition;

            Vector2 leftWorldPos = leftLifter.transform.TransformPoint(leftLifterParentPosition);
            Vector2 rightWorldPos = rightLifter.transform.TransformPoint(rightLifterParentPosition);

            Vector2 newLifterPosition = leftWorldPos + Vector2.up * startMovementSpeed * Time.fixedDeltaTime;
            Vector2 newRightLifterPosition = rightWorldPos + Vector2.up * startMovementSpeed * Time.fixedDeltaTime;

            leftLifter.MovePosition(newLifterPosition);
            rightLifter.MovePosition(newRightLifterPosition);

            yield return new WaitForEndOfFrame();
        }
        inputEnabled = true;

        InfiniteGameController.instance.ReadyForNextHole();

        yield return null;
    }

    IEnumerator MoveBarToBottomPosition()
    {
        while (leftLifterPosition.y > bottomPosition.y || rightLifterPosition.y > bottomPosition.y)
        {

            leftLifterPosition = leftLifter.transform.localPosition;
            rightLifterPosition = rightLifter.transform.localPosition;

            Vector2 leftLifterParentPosition = leftLifter.transform.parent.localPosition;
            Vector2 rightLifterParentPosition = rightLifter.transform.parent.localPosition;

            Vector2 leftWorldPos = leftLifter.transform.TransformPoint(leftLifterParentPosition);
            Vector2 rightWorldPos = rightLifter.transform.TransformPoint(rightLifterParentPosition);

            Vector2 newLeftLifterPosition = leftWorldPos - Vector2.up * bottomMovementSpeed * Time.fixedDeltaTime;
            Vector2 newRightLifterPosition = rightWorldPos - Vector2.up * bottomMovementSpeed * Time.fixedDeltaTime;

            if (leftLifterPosition.y > bottomPosition.y)
            {
                leftLifter.MovePosition(newLeftLifterPosition);
            }
            if (rightLifterPosition.y > bottomPosition.y)
            {
                rightLifter.MovePosition(newRightLifterPosition);
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

    public void QuickBarResetFunction()
    {
        inputEnabled = false;
        StartCoroutine(QuickBarResetToBottom());
    }

    IEnumerator QuickBarResetToBottom()
    {
        while (leftLifterPosition.y > bottomPosition.y || rightLifterPosition.y > bottomPosition.y)
        {

            leftLifterPosition = leftLifter.transform.localPosition;
            rightLifterPosition = rightLifter.transform.localPosition;

            Vector2 leftLifterParentPosition = leftLifter.transform.parent.localPosition;
            Vector2 rightLifterParentPosition = rightLifter.transform.parent.localPosition;

            Vector2 leftWorldPos = leftLifter.transform.TransformPoint(leftLifterParentPosition);
            Vector2 rightWorldPos = rightLifter.transform.TransformPoint(rightLifterParentPosition);

            Vector2 newLeftLifterPosition = leftWorldPos - Vector2.up * resetMovementSpeed * Time.fixedDeltaTime;
            Vector2 newRightLifterPosition = rightWorldPos - Vector2.up * resetMovementSpeed * Time.fixedDeltaTime;

            if (leftLifterPosition.y > bottomPosition.y)
            {
                leftLifter.MovePosition(newLeftLifterPosition);
            }
            if (rightLifterPosition.y > bottomPosition.y)
            {
                rightLifter.MovePosition(newRightLifterPosition);
            }

            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(QuickBarResetToStart());

        yield return null;
    }

    IEnumerator QuickBarResetToStart()
    {
        while (leftLifter.transform.localPosition.y < bottomPosition.y + startPositionVerticalOffset)
        {
            leftLifterPosition = leftLifter.transform.localPosition;
            rightLifterPosition = rightLifter.transform.localPosition;

            Vector2 leftLifterParentPosition = leftLifter.transform.parent.localPosition;
            Vector2 rightLifterParentPosition = rightLifter.transform.parent.localPosition;

            Vector2 leftWorldPos = leftLifter.transform.TransformPoint(leftLifterParentPosition);
            Vector2 rightWorldPos = rightLifter.transform.TransformPoint(rightLifterParentPosition);

            Vector2 newLifterPosition = leftWorldPos + Vector2.up * resetMovementSpeed * Time.fixedDeltaTime;
            Vector2 newRightLifterPosition = rightWorldPos + Vector2.up * resetMovementSpeed * Time.fixedDeltaTime;

            leftLifter.MovePosition(newLifterPosition);
            rightLifter.MovePosition(newRightLifterPosition);

            yield return new WaitForEndOfFrame();
        }

        inputEnabled = true;

        if (InfiniteHolesController.instance != null)
        {
            InfiniteHolesController.instance.SpawnHoles();
        }
        if (InfiniteBeesController.instance != null)
        {
            InfiniteBeesController.instance.SpawnBees();
        }
        if (InfiniteWormsController.instance != null)
        {
            InfiniteWormsController.instance.SpawnWorms();
        }
        if (InfinitePointsController.instance != null)
        {
            InfinitePointsController.instance.SpawnPoints();
        }
        if (InfiniteFruitsController.instance != null)
        {
            InfiniteFruitsController.instance.SpawnFruits();
        }

        InfiniteGameController.instance.QuickResetFruit();

        yield return null;
    }


}
