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

    public int score;
    public int bonusScore;
    public int bestScore;

    public int difficultyLevel;
    public int currentLevel;
    public int currentFruitNumber = 3;

    public float timePerDecrement;
    public int bonusScoreIncrement;
    public int bonusScoreDecrement;

    public int fruitScoreIncrement;
    public int pointsScoreIncrement;

    public bool levelCompletedState;
    public bool gameOverState;

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

        gameOverState = false;
        levelCompletedState = false;

        currentLevel = 1;
        difficultyLevel = 1;

        bonusScoreIncrement = 1000;
        bonusScoreDecrement = 10;
        timePerDecrement = 2.0f;

        fruitScoreIncrement = 500;
        pointsScoreIncrement = 25;

        score = 0;
        bonusScore = currentLevel * bonusScoreIncrement;

        if (SaveManager.instance != null)
        {
            bestScore = SaveManager.instance.GetBestScore(GAME_MODE.INFINITE_MODE);
        }

    }

    public void UpdateHUD()
    {
        if (HUDMenuManager.instance != null)
        {
            HUDMenuManager.instance.UpdateInfiniteHUD();
        }
    }

    // ========== SCORE FUNCTIONS ========== //

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
        if (SaveManager.instance != null)
        {
            SaveManager.instance.UpdateBestScore(GAME_MODE.INFINITE_MODE, score);
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

    // ========== FRUITS FUNCTIONS ========== //

    private void FruitNumberIncrement()
    {
        if (currentFruitNumber < 3)
        {
            currentFruitNumber++;
        }

        UpdateHUD();
    }

    private void FruitNumberDecrement()
    {
        currentFruitNumber--;

        GameOverCheck();

        UpdateHUD();
    }

    // ========== GETTERS ========== //

    public Vector3 GetFruitPosition()
    {
        return fruitRef.gameObject.transform.position;
    }

    public Vector3 GetFruitLocalPosition()
    {
        return fruitRef.gameObject.transform.localPosition;
    }

    // ========== ANIMATIONS ========== //

    public void ResetFruit()
    {
        fruitRef.ResetFruitPosition();
    }

    public void LevelCompleted()
    {
        levelCompletedState = true;

        CancelInvoke(nameof(DecreaseBonusScore));

        //Play Level completed animation
        NextLevel();
    }

    // ========== LEVEL TRANSITIONS ========== //

    public void NextLevel()
    {
        RecalculateScore();

        currentLevel++;
        difficultyLevel++;
        
        if (difficultyLevel > 9)
        {
            difficultyLevel = 9;
        }

        bonusScore = currentLevel * bonusScoreIncrement;

        UpdateHUD();

        RemoveObstacles();

        PrepareForLevel();

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    public void ResetGame()
    {
        currentFruitNumber = 3;
        score = 0;
        currentLevel = 1;
        difficultyLevel = 1;
        bonusScore = currentLevel * bonusScoreIncrement;

        levelCompletedState = false;
        gameOverState = true;

        RemoveObstacles();

        PrepareForLevel();

        elevatorControllerRef.MoveBarToBottomPositionFunction();

        UpdateHUD();
    }

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        levelCompletedState = false;
        gameOverState = false;

        SpawnObstacles();

        UpdateHUD();

        elevatorControllerRef.MoveBarToStartPositionFunction();
    }

    public void ReadyForLevel()
    {
        if (CameraManager.instance != null)
        {
            CameraManager.instance.SetFocus();
        }

        InvokeRepeating(nameof(DecreaseBonusScore), timePerDecrement, timePerDecrement);

        UpdateHUD();
    }

    public void PrepareForLevel()
    {
        CancelInvoke(nameof(DecreaseBonusScore));

        if (CameraManager.instance != null)
        {
            CameraManager.instance.SetUnfocus();
        }

        UpdateHUD();
    }

    public void GameOverCheck()
    {
        if (currentFruitNumber <= 0)
        {
            gameOverState = true;

            RecalculateBestScore();

            if (GlobalUIManager.instance != null)
            {
                GlobalUIManager.instance.SetScoreBoardMenu();
            }
        }
    }

    // ========== OBSTACLES HANDLES ========== //

    public void HandleFruitInHole()
    {
        PrepareForLevel();

        FruitNumberDecrement();

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    public void HandleFruitInBee()
    {
        PrepareForLevel();

        FruitNumberDecrement();

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    public void HandleFruitInWorm()
    {
        PrepareForLevel();

        FruitNumberDecrement();

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    public void HandleFruitInBear()
    {
        PrepareForLevel();

        FruitNumberDecrement();

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    public void HandleFruitInPoints()
    {
        PointsScoreIncrement();
    }

    public void HandleFruitInFruit()
    {
        FruitScoreIncrement();

        FruitNumberIncrement();
    }

    public void SpawnObstacles()
    {
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
        if (InfiniteBearController.instance != null)
        {
            InfiniteBearController.instance.SpawnBears();
        }
        if (InfinitePointsController.instance != null)
        {
            InfinitePointsController.instance.SpawnPoints();
        }
        if (InfiniteFruitsController.instance != null)
        {
            InfiniteFruitsController.instance.SpawnFruits();
        }
    }

    public void RemoveObstacles()
    {
        if (InfiniteHolesController.instance != null)
        {
            InfiniteHolesController.instance.RemoveHoles();
        }
        if (InfiniteBeesController.instance != null)
        {
            InfiniteBeesController.instance.RemoveBees();
        }
        if (InfiniteWormsController.instance != null)
        {
            InfiniteWormsController.instance.RemoveWorms();
        }
        if (InfiniteBearController.instance != null)
        {
            InfiniteBearController.instance.RemoveBears();
        }
        if (InfinitePointsController.instance != null)
        {
            InfinitePointsController.instance.RemovePoints();
        }
        if (InfiniteFruitsController.instance != null)
        {
            InfiniteFruitsController.instance.RemoveFruits();
        }
    }


    // ========== DEV FUNCTIONS ========== //

    public void QuickResetFruit()
    {
        Vector3 positon = new Vector3(0, elevatorControllerRef.transform.localPosition.y + 0.25f, 0);
        fruitRef.QuickResetFruitPosition(positon);
    }

    public void SetLevel(int l)
    {
        currentLevel = l;

        if (l > 9)
        {
            difficultyLevel = 9;
        }
        else
        {
            difficultyLevel = l;
        }

        currentFruitNumber = 10000;

        RemoveObstacles();

        elevatorControllerRef.QuickBarResetFunction();
    }
}
