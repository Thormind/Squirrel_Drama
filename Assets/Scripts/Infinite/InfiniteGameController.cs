using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TMPro;

public class InfiniteGameController : MonoBehaviour
{
    public static InfiniteGameController instance = null;

    public InfiniteElevatorController elevatorControllerRef;
    public InfiniteFruit fruitRef;

    public float score = 0;
    public float bonusScore = 1000;
    public float bestScore = 0;

    public float level;
    public int currentFruitNumber = 3;

    public float timePerDecrement = 2.0f;
    public float bonusScoreIncrement = 1000f;
    public float bonusScoreDecrement = 10f;

    public float fruitScoreIncrement;
    public float pointsScoreIncrement;

    public bool levelCompletedState = false;
    public bool gameOverState = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        level = 1;
        fruitScoreIncrement = 500f;
        pointsScoreIncrement = 50f;
    }


    public void UpdateHUD()
    {
        if (HUDMenuManager.instance != null)
        {
            HUDMenuManager.instance.UpdateInfiniteHUD();
        }
    }

    private void RecalculateScore()
    {
        score += bonusScore;
        UpdateHUD();
    }

    private void PointsScoreIncrement()
    {
        score += pointsScoreIncrement;
        UpdateHUD();
    }

    private void FruitScoreIncrement()
    {
        score += fruitScoreIncrement;
        UpdateHUD();
    }

    private void RecalculateBestScore()
    {
        if (bestScore < score)
        {
            bestScore = score;
        }
        UpdateHUD();
    }

    private void DecreaseBonusScore()
    {
        if (bonusScore >= bonusScoreDecrement)
        {
            bonusScore -= bonusScoreDecrement;
            UpdateHUD();
        }
        else
        {
            bonusScore = 0;
            UpdateHUD();
        }
    }

    public Vector3 GetFruitPosition()
    {
        return fruitRef.gameObject.transform.position;
    }

    public void ResetFruit()
    {
        fruitRef.ResetFruitPosition();
    }

    public void LevelCompleted()
    {
        NextLevel();
    }

    public void NextLevel()
    {
        CancelInvoke(nameof(DecreaseBonusScore));

        RecalculateScore();

        level++;
        bonusScore = level * bonusScoreIncrement;

        levelCompletedState = true;

        InvokeRepeating(nameof(DecreaseBonusScore), timePerDecrement, timePerDecrement);

        UpdateHUD();

        if (CameraManager.instance != null)
        {
            CameraManager.instance.SetUnfocus();
        }

        InfiniteHolesController.instance.RemoveHoles();
        InfiniteBeesController.instance.RemoveBees();
        InfinitePointsController.instance.RemovePoints();
        InfiniteFruitsController.instance.RemoveFruits();

        elevatorControllerRef.movementSpeed = 200f;
        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    public void ResetGame()
    {
        currentFruitNumber = 3;
        score = 0;
        level = 1;
        bonusScore = level * bonusScoreIncrement;

        levelCompletedState = false;
        gameOverState = true;


        if (CameraManager.instance != null)
        {
            CameraManager.instance.SetUnfocus();
        }

        InfiniteHolesController.instance.RemoveHoles();
        InfiniteBeesController.instance.RemoveBees();
        InfinitePointsController.instance.RemovePoints();
        InfiniteFruitsController.instance.RemoveFruits();

        elevatorControllerRef.movementSpeed = 200f;
        elevatorControllerRef.MoveBarToBottomPositionFunction();

        CancelInvoke(nameof(DecreaseBonusScore));
        RecalculateBestScore();

        UpdateHUD();
    }

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        levelCompletedState = false;
        gameOverState = false;

        InfiniteHolesController.instance.SpawnHoles();
        InfiniteBeesController.instance.SpawnBees();
        InfinitePointsController.instance.SpawnPoints();
        InfiniteFruitsController.instance.SpawnFruits();


        if (CameraManager.instance != null)
        {
            CameraManager.instance.SetFocus();
        }


        UpdateHUD();

        elevatorControllerRef.movementSpeed = 30f;
        elevatorControllerRef.MoveBarToStartPositionFunction();
    }

    public void ReadyForNextHole()
    {

        if (CameraManager.instance != null)
        {
            CameraManager.instance.SetFocus();
        }

        InvokeRepeating(nameof(DecreaseBonusScore), timePerDecrement, timePerDecrement);

        UpdateHUD();
    }

    public void HandleFruitInHole()
    {
        CancelInvoke(nameof(DecreaseBonusScore));

        if (CameraManager.instance != null)
        {
            CameraManager.instance.SetUnfocus();
        }

        RecalculateScore();
        currentFruitNumber--; 

        if (currentFruitNumber <= 0)
        {
            gameOverState = true;

            RecalculateBestScore();

            GlobalUIManager.instance.SetScoreBoardMenu();
        }

        UpdateHUD();

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    public void HandleFruitInBee()
    {
        CancelInvoke(nameof(DecreaseBonusScore));

        if(CameraManager.instance != null)
        {
            CameraManager.instance.SetUnfocus();
        }

        RecalculateScore();
        currentFruitNumber--;

        if (currentFruitNumber <= 0)
        {
            gameOverState = true;

            RecalculateBestScore();

            GlobalUIManager.instance.SetScoreBoardMenu();
        }

        UpdateHUD();

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    public void HandleFruitInPoints()
    {
        PointsScoreIncrement();

        UpdateHUD();
    }

    public void HandleFruitInFruit()
    {
        FruitScoreIncrement();

        if (currentFruitNumber < 3)
        {
            currentFruitNumber++;
        }

        UpdateHUD();
    }

}
