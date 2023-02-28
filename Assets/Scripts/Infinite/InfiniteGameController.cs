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

    public float timePerDecrement = 10.0f;
    public float bonusScoreIncrement = 1000f;
    public float bonusScoreDecrement = 100f;

    public float fruitScoreIncrement;

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
    }


    public void UpdateHUD()
    {
        HUDMenuManager.instance.UpdateInfiniteHUD();
    }

    private void RecalculateScore()
    {
        score += bonusScore;
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

        CameraManager.instance.SetUnfocus();

        InfiniteHolesController.instance.RemoveHoles();
        InfiniteBeesController.instance.RemoveBees();
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

        CameraManager.instance.SetUnfocus();

        InfiniteHolesController.instance.RemoveHoles();
        InfiniteBeesController.instance.RemoveBees();
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
        InfiniteFruitsController.instance.SpawnFruits();

        CameraManager.instance.SetFocus();


        UpdateHUD();

        elevatorControllerRef.movementSpeed = 30f;
        elevatorControllerRef.MoveBarToStartPositionFunction();
    }

    public void ReadyForNextHole()
    {

        CameraManager.instance.SetFocus();

        InvokeRepeating(nameof(DecreaseBonusScore), timePerDecrement, timePerDecrement);

        UpdateHUD();
    }

    public void HandleFruitInHole()
    {
        CancelInvoke(nameof(DecreaseBonusScore));

        CameraManager.instance.SetUnfocus();

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

        CameraManager.instance.SetUnfocus();

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
