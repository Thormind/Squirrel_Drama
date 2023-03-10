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

    public int bonusScoreIncrement;
    public int bonusScoreDecrement;
    private int maxBonusScore;

    public int fruitScoreIncrement;
    public int pointsScoreIncrement;

    public float gameTime;

    private float startTime;
    private float elapsedTime;
    public float timePerDecrement;
    public float maxTimeBeforeDecrement;
    private bool timerRunning = false;

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
        bonusScoreDecrement = 100;
        timePerDecrement = 5.0f;
        maxTimeBeforeDecrement = 20f;
        maxBonusScore = 1000;

        fruitScoreIncrement = 500;
        pointsScoreIncrement = 25;

        score = 0;

        bonusScore = 0;

        if (SaveManager.instance != null)
        {
            bestScore = SaveManager.instance.GetBestScore(GAME_MODE.INFINITE_MODE);
        }

    }

    private void Update()
    {
        UpdateTimer();
    }

    public void UpdateHUD(GAME_DATA gameData)
    {
        if (HUDMenuManager.instance != null)
        {
            HUDMenuManager.instance.UpdateInfiniteHUD(gameData);
        }
    }

    // ========== TIME FUNCTIONS ========== //

    // Call this method to start or resume the timer
    public void StartTimer()
    {
        if (!timerRunning)
        {
            startTime = Time.time - elapsedTime;
            timerRunning = true;
        }
    }

    // Call this method to pause the timer
    public void PauseTimer()
    {
        if (timerRunning)
        {
            elapsedTime = Time.time - startTime;
            timerRunning = false;
        }
    }
  
    // Call this method to reset the timer to 0
    public void ResetTimer()
    {
        elapsedTime = 0f;
        timerRunning = false;

        if (HUDMenuManager.instance != null)
        {
            HUDMenuManager.instance.ResetInfiniteTimer();
        }
    }

    // Call this method to reset the timer to 0
    public void UpdateTimer()
    {
        if (timerRunning)
        {
            gameTime = Time.timeSinceLevelLoad - startTime;

            if (HUDMenuManager.instance != null)
            {
                HUDMenuManager.instance.UpdateInfiniteTimer(gameTime);
            }
        }
    }


    // ========== SCORE FUNCTIONS ========== //

    private void RecalculateBonusScore()
    {
        int timerBonus = CalculateTimerBonusScore();

        if (HUDMenuManager.instance != null && AnimationManager.instance != null)
        {
            AnimationManager.instance.PlayAnimation(
                HUDMenuManager.instance.AnimateInfiniteBonusScore(bonusScore, bonusScore + timerBonus), 
                () => { RecalculateScore(); 
            });

        }

        bonusScore += timerBonus;
    }

    public void RecalculateScore()
    {
        if (HUDMenuManager.instance != null)
        {
            AnimationManager.instance.PlayAnimation(
                HUDMenuManager.instance.AnimateInfiniteScore(bonusScore, 0, score, score + bonusScore),
                () => { NextLevel();
            });
        }
        score += bonusScore;
        UpdateHUD(GAME_DATA.SCORE);
        bonusScore = 0;
        UpdateHUD(GAME_DATA.BONUS_SCORE);
    }


    private void PointsBonusScoreIncrement()
    {
        bonusScore += pointsScoreIncrement;
        UpdateHUD(GAME_DATA.BONUS_SCORE);
    }

    private void FruitBonusScoreIncrement()
    {
        bonusScore += fruitScoreIncrement;
        UpdateHUD(GAME_DATA.BONUS_SCORE);
    }

    private void RecalculateBestScore()
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.UpdateBestScore(GAME_MODE.INFINITE_MODE, score);
        }
    }

    // Call this method to calculate the bonus score
    public int CalculateTimerBonusScore()
    {
        float totalTime = Time.time - startTime;

        print($"Time: {totalTime}");

        if (totalTime <= 20f)
        {
            return maxBonusScore * currentLevel;
        }
        else
        {
            float timeAbove20 = totalTime - maxTimeBeforeDecrement;
            int bonusScore = maxBonusScore - Mathf.FloorToInt(timeAbove20 / timePerDecrement) * bonusScoreDecrement;
            bonusScore = Mathf.Max(bonusScore, 0); // ensure the bonus score is not negative
            return bonusScore * currentLevel;
        }
    }

    // ========== FRUITS FUNCTIONS ========== //

    private void FruitNumberIncrement()
    {
        currentFruitNumber++;
        UpdateHUD(GAME_DATA.LIFE);
    }

    private void FruitNumberDecrement()
    {
        currentFruitNumber--;
        UpdateHUD(GAME_DATA.LIFE);

        GameOverCheck();
    }

    private void EnableFruitCollision(bool enableCollision)
    {
        fruitRef.GetComponent<InfiniteFruit>().collisionEnabled = enableCollision;
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

    public Vector3 GetElevatorPosition()
    {
        return elevatorControllerRef.gameObject.transform.position;
    }

    public Vector3 GetElevatorLocalPosition()
    {
        return elevatorControllerRef.gameObject.transform.localPosition;
    }

    public float GetElevatorHeight()
    {
        return elevatorControllerRef.gameObject.transform.position.y;
    }

    public float GetElevatorLocalHeight()
    {
        return elevatorControllerRef.gameObject.transform.localPosition.y;
    }

    // ========== ANIMATIONS ========== //

    public void ResetFruit()
    {
        Vector3 positon = new Vector3(0, elevatorControllerRef.transform.localPosition.y + 0.5f, 0);
        fruitRef.ResetFruitPosition(positon);
    }

    public void LevelCompleted()
    {
        levelCompletedState = true;

        GlobalUIManager.instance.EnableInputs(false);

        RemoveObstacles();

        PauseTimer();

        ResetTimer();

        RecalculateBonusScore();
    }

    // ========== LEVEL TRANSITIONS ========== //

    public void NextLevel()
    {
        currentLevel++;
        UpdateHUD(GAME_DATA.LEVEL);

        difficultyLevel++;
        if (difficultyLevel > 9)
        {
            difficultyLevel = 9;
        }

        // ==================== //

        GlobalUIManager.instance.EnableInputs(true);

        PrepareForLevel();

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    public void ResetGame()
    {
        currentFruitNumber = 3;
        UpdateHUD(GAME_DATA.LIFE);

        score = 0;
        UpdateHUD(GAME_DATA.SCORE);

        bonusScore = 0;
        UpdateHUD(GAME_DATA.BONUS_SCORE);

        currentLevel = 1;
        UpdateHUD(GAME_DATA.LEVEL);

        difficultyLevel = 1;

        levelCompletedState = false;
        gameOverState = true;

        // ==================== //

        RemoveObstacles();

        ResetTimer();

        PrepareForLevel();

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        levelCompletedState = false;
        gameOverState = false;

        SpawnObstacles();

        //UpdateHUD();

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    public void ReadyForLevel()
    {
        if (CameraManager.instance != null)
        {
            CameraManager.instance.Transition(true);
        }

        EnableFruitCollision(true);

        StartTimer();

        //UpdateHUD();
    }

    public void PrepareForLevel()
    {
        PauseTimer();

        Time.timeScale = 1f;

        EnableFruitCollision(false);

        if (CameraManager.instance != null)
        {
            CameraManager.instance.Transition(false);
        }

        //UpdateHUD();
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
        PointsBonusScoreIncrement();
    }

    public void HandleFruitInFruit()
    {
        if (currentFruitNumber < 3)
        {
            FruitNumberIncrement();
        }
        if (currentFruitNumber >= 3)
        {
            FruitBonusScoreIncrement();
        }
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
