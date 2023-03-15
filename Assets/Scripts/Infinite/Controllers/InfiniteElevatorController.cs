using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InfiniteElevatorController : MonoBehaviour
{
    private float resetMovementSpeed = 100f;
    private float bottomMovementSpeed;

    private float startMovementSpeed;
    private float movementSpeed;
    private float maxDifference;

    public Transform bottom;
    public Transform start;
    public Transform end;

    public Rigidbody2D leftLifter;
    public Rigidbody2D rightLifter;
    public ParticleSystem leftLifterVFX;
    public ParticleSystem rightLifterVFX;

    public Rigidbody2D elevatorRigidBody;

    private Vector2 movementOffset;

    private float leftUpInputValue;
    private float leftDownInputValue;
    private float rightUpInputValue;
    private float rightDownInputValue;

    private bool inputEnabled = false;

    private void Awake()
    {
        elevatorRigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (inputEnabled)
        {
            if (leftLifter.position.y >= end.position.y
                && rightLifter.position.y >= end.position.y)
            {
                inputEnabled = false;
                InfiniteGameController.instance.LevelCompleted();
            }

            Vector2 input = new Vector2(leftUpInputValue - leftDownInputValue, rightUpInputValue - rightDownInputValue);
            movementOffset = input * Time.fixedDeltaTime * movementSpeed;

            ParticleSystem.MainModule leftVFX = leftLifterVFX.main;
            leftVFX.startSize = Mathf.Lerp(3f, 6f, Mathf.Abs(input.x));
            ParticleSystem.MainModule rightVFX = rightLifterVFX.main;
            rightVFX.startSize = Mathf.Lerp(3f, 6f, Mathf.Abs(input.y));

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

        bottomMovementSpeed = CalculateBottomMovementSpeed();

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


        if (ScenesManager.gameState == GAME_STATE.LEVEL_COMPLETED)
        {
            InfiniteGameController.instance.StartGame();
        }
        if (ScenesManager.gameState == GAME_STATE.ACTIVE)
        {
            if (AnimationManager.instance != null)
            {
                AnimationManager.instance.PlayObstaclesAnimation(MoveBarToStartPosition());
            }
            //StartCoroutine(MoveBarToStartPosition());
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
        StopAllCoroutines();
       
        StartCoroutine(MoveBarToBottomPosition());
    }

    public float CalculateBottomMovementSpeed()
    {
        // Ensure height is within the range of 0 to 40
        float height = Mathf.Clamp(leftLifter.transform.localPosition.y, 0f, 40f);

        // Calculate the slope of the line
        float m = (75f - 25f) / 40f;

        // Calculate the y-intercept of the line
        float b = 25f;

        // Calculate the speed using the linear equation y = mx + b
        float speed = m * height + b;

        return speed;
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


    public void SetElevatorMovementSpeed(float mSpeed)
    {
        movementSpeed = mSpeed;
    }

    public void SetElevatorStartMovementSpeed(float startSpeed)
    {
        startMovementSpeed = startSpeed;
    }

    public void SetElevatorMaxDifference(float maxDiff)
    {
        maxDifference = maxDiff;
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
