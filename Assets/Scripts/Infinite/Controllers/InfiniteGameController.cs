using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TMPro;

[System.Serializable]
public enum PREPERATION_STATE
{
    AFTER_START,
    AFTER_DEATH,
    AFTER_LEVEL_COMPLETED,
};

public class InfiniteGameController : MonoBehaviour
{
    public static InfiniteGameController instance = null;

    [SerializeField] private InfiniteElevatorParametersSO elevatorParameters;

    public InfiniteElevatorController elevatorControllerRef;
    public InfiniteFruit fruitRef;
    public InfiniteScoreMultiplier scoreMultiplierRef;
    public SquirrelAnimation squirrelAnimationRef;

    public GameObject fruitParent;
    public GameObject obstaclesParent;
    public GameObject obstacleInstanciateVFX;
    public GameObject fruitInstanciateVFX;

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

    public delegate void GameStateChangedEventHandler(GAME_STATE newGameState);

    private Coroutine currentPreparingCoroutine;

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

    private void Start()
    {
        ScenesManager.OnGameStateChanged += HandleGameStateChanged;

        currentLevel = 1;
        difficultyLevel = 1;

        bonusScoreIncrement = 1000;
        bonusScoreDecrement = 100;
        timePerDecrement = 5.0f;
        maxTimeBeforeDecrement = 30f;
        maxBonusScore = 1000;

        fruitScoreIncrement = 500;
        pointsScoreIncrement = 25;

        score = 0;
        bonusScore = 0;

        if (SaveManager.instance != null)
        {
            bestScore = SaveManager.instance.GetBestScore(GAME_MODE.INFINITE_MODE);
        }


        elevatorControllerRef.SetElevatorMovementSpeed(ElevatorMovementSpeed);
        elevatorControllerRef.SetElevatorStartMovementSpeed(ElevatorStartMovementSpeed);
        elevatorControllerRef.SetElevatorMaxDifference(ElevatorMaxDifference);

        fruitRef.SetFruitGravityScale(FruitGravityScale);
        fruitRef.SetFruitFallingGravityScale(FruitFallingGravityScale);
        fruitRef.SetFruitMinCollisionDistance(FruitMinCollisionDistance);

    }

    private void OnDestroy()
    {
        ScenesManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void FixedUpdate()
    {
        UpdateTimer();
        UpdateHUD(GAME_DATA.MAP);

        LevelCompletedCheck();
    }

    public void UpdateHUD(GAME_DATA gameData)
    {
        if (HUDMenuManager.instance != null && HUDMenuManager.instance.isActiveAndEnabled)
        {
            HUDMenuManager.instance.UpdateInfiniteHUD(gameData);
        }
    }

    public void ShowHUDBonusScoreIndicator(int bonusScore)
    {
        if (HUDMenuManager.instance != null && HUDMenuManager.instance.isActiveAndEnabled)
        {
            HUDMenuManager.instance.ShowInfiniteBonusScoreUpdateIndicator(bonusScore);
        }
    }

    public void ShowHUDLifeIndicator(bool extraLife)
    {
        if (HUDMenuManager.instance != null && HUDMenuManager.instance.isActiveAndEnabled)
        {
            HUDMenuManager.instance.ShowInfiniteLifeUpdateIndicator(extraLife);
        }
    }

    public IEnumerator ShowLevelIndicator()
    {
        while (HUDMenuManager.instance == null || !HUDMenuManager.instance.isActiveAndEnabled)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();

        HUDMenuManager.instance.ShowInfiniteLevelIndicator();
    }

    // ========== TIME FUNCTIONS ========== //

    public void StartTimer()
    {
        if (!timerRunning)
        {
            startTime = Time.time - elapsedTime;
            timerRunning = true;
        }
    }

    public void PauseTimer()
    {
        if (timerRunning)
        {
            elapsedTime = Time.time - startTime;
            timerRunning = false;
        }
    }
  
    public void ResetTimer()
    {
        elapsedTime = 0f;
        timerRunning = false;
    }

    public void UpdateTimer()
    {
        if (timerRunning)
        {
            gameTime = Time.timeSinceLevelLoad - startTime;

            if (HUDMenuManager.instance != null && HUDMenuManager.instance.isActiveAndEnabled)
            {
                HUDMenuManager.instance.UpdateInfiniteTimer(gameTime);
            }
        }
    }

    // ========== SCORE & LIFE FUNCTIONS ========== //

    private void RecalculateBestScore()
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.UpdateBestScore(GAME_MODE.INFINITE_MODE, score);
        }
    }


    private void RecalculateBonusScore()
    {
        int timerBonus = CalculateTimerBonusScore();

        ShowHUDBonusScoreIndicator(timerBonus);

        if (HUDMenuManager.instance != null && HUDMenuManager.instance.isActiveAndEnabled && AnimationManager.instance != null && timerBonus > 0)
        {
            AnimationManager.instance.PlayInGameAnimation( HUDMenuManager.instance.AnimateInfiniteBonusScore(bonusScore, bonusScore + timerBonus));
        }
        else if (HUDMenuManager.instance != null && HUDMenuManager.instance.isActiveAndEnabled && AnimationManager.instance != null && timerBonus <= 0)
        {
            AnimationManager.instance.PlayInGameAnimation(HUDMenuManager.instance.AnimateInfiniteTimerReset());
        }

        bonusScore += timerBonus;
    }

    public void RecalculateScore()
    {
        if (HUDMenuManager.instance != null && HUDMenuManager.instance.isActiveAndEnabled && bonusScore > 0)
        {
            AnimationManager.instance.PlayInGameAnimation(HUDMenuManager.instance.AnimateInfiniteScore(bonusScore, 0, score, score + bonusScore));
        }

        score += bonusScore;
        bonusScore = 0;
    }


    private void PointsBonusScoreIncrement()
    {
        int points = scoreMultiplierRef.ApplyMultiplierToScore(pointsScoreIncrement * difficultyLevel);
        bonusScore += points;
        ShowHUDBonusScoreIndicator(points);
        UpdateHUD(GAME_DATA.BONUS_SCORE);
    }

    private void FruitBonusScoreIncrement()
    {
        int points = scoreMultiplierRef.ApplyMultiplierToScore(fruitScoreIncrement * difficultyLevel);
        bonusScore += points;
        ShowHUDBonusScoreIndicator(points);
        UpdateHUD(GAME_DATA.BONUS_SCORE);
    }

    private void FruitNumberIncrement()
    {
        currentFruitNumber++;
        ShowHUDLifeIndicator(true);
        UpdateHUD(GAME_DATA.LIFE);
    }

    private void FruitNumberDecrement()
    {
        currentFruitNumber--;
        ShowHUDLifeIndicator(false);
        UpdateHUD(GAME_DATA.LIFE);
    }

    // Call this method to calculate the bonus score
    public int CalculateTimerBonusScore()
    {
        float totalTime = Time.time - startTime;

        if (totalTime <= maxTimeBeforeDecrement)
        {
            return scoreMultiplierRef.ApplyMultiplierToScore(maxBonusScore * currentLevel);
        }
        else
        {
            float timeAboveMaxTimeBeforeDecrement = totalTime - maxTimeBeforeDecrement;
            int bonusScore = maxBonusScore - Mathf.FloorToInt(timeAboveMaxTimeBeforeDecrement / timePerDecrement) * bonusScoreDecrement;
            bonusScore = Mathf.Max(bonusScore, 0); // ensure the bonus score is not negative
            return scoreMultiplierRef.ApplyMultiplierToScore(bonusScore * currentLevel);
        }
    }










    // ======================================= //
    // ========== LEVEL TRANSITIONS ========== //
    // ======================================= //

    public void ResetNReady()
    {
        StartCoroutine(elevatorControllerRef.MoveBarToStartPosition());
        StartCoroutine(fruitRef.AnimateFruitReset());
    }


    public void LevelCompleted()
    {
        EnableFruitCollision(false);

        StartSquirrelFruitSequence();

        RemoveObstacles();

        PauseTimer();

        ResetTimer();

        RecalculateBonusScore();

        RecalculateScore();

        if (HUDMenuManager.instance != null && HUDMenuManager.instance.isActiveAndEnabled && AnimationManager.instance != null)
        {
            AnimationManager.instance.PlayInGameAnimation(NextLevel());
        }
        
    }


    public void StartSquirrelFruitSequence()
    {
        if (HUDMenuManager.instance != null && HUDMenuManager.instance.isActiveAndEnabled && AnimationManager.instance != null)
        {
            AnimationManager.instance.PlayHappySquirrelFaceAnimation();
            AnimationManager.instance.PlayInGameAnimation(fruitRef.MoveToSquirrelCoroutine());
            AnimationManager.instance.PlaySquirrelTakeFruitAnimation();
            AnimationManager.instance.PlayInGameAnimation(fruitRef.MoveToSquirrelLoveCoroutine());
            AnimationManager.instance.PlaySquirrelLoveFruitAnimation();
            AnimationManager.instance.PlayInGameAnimation(fruitRef.AfterSquirrelLoveCoroutine());
        }
    }

    public IEnumerator NextLevel()
    {
        yield return null;

        currentLevel++;
        UpdateHUD(GAME_DATA.LEVEL);

        difficultyLevel++;
        if (difficultyLevel > 9)
        {
            difficultyLevel = 9;
        }

        // ==================== //

        fruitRef.ResetFruitPosition();

        StartCoroutine(ShowLevelIndicator());

        StartCoroutine(PrepareForLevel(PREPERATION_STATE.AFTER_LEVEL_COMPLETED));
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

        // ==================== //

        RemoveObstacles();

        ResetTimer();
        UpdateHUD(GAME_DATA.TIMER);


        SetSlowMotion(false);
        EnableFruitCollision(false);
        SetRigidBodyExtrapolate(false);

        if (CameraManager.instance != null)
        {
            CameraManager.instance.Transition(false);
        }

        if (currentPreparingCoroutine != null)
        {
            StopCoroutine(currentPreparingCoroutine);
            currentPreparingCoroutine = null;
        }

        StopAllCoroutines();

        fruitRef.ResetFruitPosition();
        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        ScenesManager.gameState = GAME_STATE.PREPARING;

        SpawnObstacles();

        StartCoroutine(ShowLevelIndicator());

        if (currentPreparingCoroutine != null)
        {
            StopCoroutine(currentPreparingCoroutine);
            currentPreparingCoroutine = null;
        }

        currentPreparingCoroutine = StartCoroutine(PreparingAfterDeathCallback());

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    public IEnumerator PreparingAfterDeathCallback()
    {
        while (AnimationManager.instance.ObstaclesAnimationIsPlaying() || elevatorControllerRef.HasNotReachedBottom())
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();

        if (ScenesManager.gameState != GAME_STATE.GAME_OVER && ScenesManager.gameState != GAME_STATE.PRE_GAME)
        {
            ScenesManager.gameState = GAME_STATE.ACTIVE;
        }
    }

    public IEnumerator PreparingAfterLevelCompletedCallback()
    {

        while (elevatorControllerRef.HasNotReachedBottom())
        {
            yield return new WaitForEndOfFrame();
        }

        if (ScenesManager.gameState != GAME_STATE.PRE_GAME)
        {
            SpawnObstacles();
        }

        yield return new WaitForSeconds(1f);

        while (AnimationManager.instance.ObstaclesAnimationIsPlaying())
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();


        if (ScenesManager.gameState != GAME_STATE.PRE_GAME)
        {
            ScenesManager.gameState = GAME_STATE.ACTIVE;
        }

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    public void ReadyForLevel()
    {
        if (CameraManager.instance != null)
        {
            CameraManager.instance.Transition(true);
        }

        EnableFruitCollision(true);
        SetRigidBodyExtrapolate(false);

        StartTimer();
    }

    private IEnumerator PrepareForLevel(PREPERATION_STATE prepState)
    {
        PauseTimer();

        if (ScenesManager.gameState != GAME_STATE.GAME_OVER)
        {
            ScenesManager.gameState = GAME_STATE.PREPARING;
        }

        EnableFruitCollision(false);
        SetRigidBodyExtrapolate(false);
        SetSlowMotion(false);

        if (CameraManager.instance != null)
        {
            CameraManager.instance.Transition(false);
        }

        if (currentPreparingCoroutine != null)
        {
            StopCoroutine(currentPreparingCoroutine);
            currentPreparingCoroutine = null;
        }

        switch (prepState)
        {
            case PREPERATION_STATE.AFTER_DEATH:
                currentPreparingCoroutine = StartCoroutine(PreparingAfterDeathCallback());
                break;
            case PREPERATION_STATE.AFTER_LEVEL_COMPLETED:
                currentPreparingCoroutine = StartCoroutine(PreparingAfterLevelCompletedCallback());
                break;
        }
        
        elevatorControllerRef.MoveBarToBottomPositionFunction();

        yield return null;
    }

    private IEnumerator Death()
    {
        AnimationManager.instance.PlaySadSquirrelFaceAnimation();

        PauseTimer();

        ScenesManager.gameState = GAME_STATE.DIED;

        EnableFruitCollision(false);
        SetRigidBodyExtrapolate(true);
        SetSlowMotion(true);

        float t = 0;

        while (t <= 1)
        {

            t += Time.deltaTime / 0.5f;

            yield return new WaitForEndOfFrame();
        }

        yield return null;

        StartCoroutine(PrepareForLevel(PREPERATION_STATE.AFTER_DEATH));

        GameOverCheck();
    }









    // ======================================= //
    // ========== GAME STATE CHECKS ========== //
    // ======================================= //

    private void HandleGameStateChanged(GAME_STATE newGameState)
    {
        switch (newGameState)
        {
            case GAME_STATE.PRE_GAME:
                ResetGame();
                break;
            case GAME_STATE.PREPARING:
                scoreMultiplierRef.ResetMultiplier();
                break;
            case GAME_STATE.ACTIVE:
                if (!elevatorControllerRef.HasNotReachedBottom())
                {
                    ResetNReady();
                }
                break;
            case GAME_STATE.GAME_OVER:
                RemoveObstacles();
                fruitRef.ResetFruitPosition();
                RecalculateBestScore();
                break;
            case GAME_STATE.LEVEL_COMPLETED:
                LevelCompleted();
                scoreMultiplierRef.StopMultiplier();
                break;
        }
    }


    public void GameOverCheck()
    {
        if (currentFruitNumber <= 0)
        {
            ScenesManager.gameState = GAME_STATE.GAME_OVER;
        }
    }


    public void LevelCompletedCheck()
    {
        if (elevatorControllerRef.HasReachedEnd() 
            && ScenesManager.gameState != GAME_STATE.LEVEL_COMPLETED 
            && ScenesManager.gameState != GAME_STATE.PREPARING)
        {
            ScenesManager.gameState = GAME_STATE.LEVEL_COMPLETED;
        }
    }











    // ======================================= //
    // ========== OBSTACLES HANDLES ========== //
    // ======================================= //

    public void HandleFruitInHole()
    {
        StartCoroutine(Death());

        FruitNumberDecrement();
    }

    public void HandleFruitInBee()
    {
        StartCoroutine(Death());

        FruitNumberDecrement();
    }

    public void HandleFruitInWorm()
    {
        StartCoroutine(Death());

        FruitNumberDecrement();
    }

    public void HandleFruitInBear()
    {
        StartCoroutine(Death());

        FruitNumberDecrement();
    }

    public void HandleFruitFalling()
    {
        StartCoroutine(Death());

        FruitNumberDecrement();
    }

    public void HandleFruitInPoints()
    {
        scoreMultiplierRef.CollectInfiniteFruitOrPoint();

        PointsBonusScoreIncrement();
    }

    public void HandleFruitInFruit()
    {
        scoreMultiplierRef.CollectInfiniteFruitOrPoint();

        if (currentFruitNumber < 3)
        {
            FruitNumberIncrement();
        }
        else if (currentFruitNumber >= 3)
        {
            FruitBonusScoreIncrement();
        }
    }

    public void SpawnObstacles()
    {
        if (AnimationManager.instance != null)
        {
            AnimationManager.instance.ClearObstaclesQueue();
        }
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
        if (AnimationManager.instance != null)
        {
            AnimationManager.instance.ClearObstaclesQueue();
        }
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

 
    public void ObstacleInstantiateAnimation(Vector3 position)
    {
        Instantiate(obstacleInstanciateVFX, position, Quaternion.identity, obstaclesParent.transform);
    }













    // ========================================== //
    // ========== COLLISIONS FUNCTIONS ========== //
    // ========================================== //

    public void SetRigidBodyExtrapolate(bool extrapolate)
    {
        Rigidbody2D[] rbs = FindObjectsOfType<Rigidbody2D>();

        foreach(Rigidbody2D rb in rbs)
        {
            if (extrapolate)
            {
                rb.interpolation = RigidbodyInterpolation2D.Extrapolate;
            }
            else
            {
                rb.interpolation = RigidbodyInterpolation2D.None;
            }
        }
    }

    private void SetSlowMotion(bool slowMo)
    {
        float defaultFixedDeltaTime = 0.02f;

        if (slowMo)
        {
            Time.timeScale = 0.2f;
            Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
        }
    }


    private void EnableFruitCollision(bool enableCollision)
    {
        fruitRef.GetComponent<InfiniteFruit>().collisionEnabled = enableCollision;
    }

    public void ResetFruitPosition()
    {
        fruitRef.ResetFruitPosition();
    }
















    // ============================= //
    // ========== GETTERS ========== //
    // ============================= //

    public Vector3 GetFruitPosition()
    {
        return fruitRef.gameObject.transform.position;
    }

    public Vector3 GetFruitLocalPosition()
    {
        return fruitRef.gameObject.transform.localPosition;
    }

    public Vector3 GetFruitPositionForIndicator()
    {
        return fruitRef.gameObject.transform.position;
    }

    public GameObject GetFruitParent()
    {
        return fruitParent;
    }

    public float GetFruitHeightForMap()
    {
        return Mathf.Max(0f, fruitRef.gameObject.transform.localPosition.y);
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

    public Vector3 GetElevatorPositionForFruitReset()
    {
        return elevatorControllerRef.gameObject.transform.position + Vector3.up * 2f;
    }

    public bool ElevatorHasNotReachedStartPosition()
    {
        return elevatorControllerRef.HasNotReachedStart();
    }

    public float ElevatorDistanceBetweenStartBottom()
    {
        return elevatorControllerRef.GetClampedDistanceBetweenBottomStart();
    }

    public float GetMultiplierTimeLeft()
    {
        return scoreMultiplierRef.GetConvertedTimeLeftBeforeEndOfStreak();
    }

    public float GetCurrentMultiplier()
    {
        return scoreMultiplierRef.GetCurrentMultiplier();
    }

    public float GetCurrentMultiplierStreak()
    {
        return scoreMultiplierRef.GetCurrentMultiplierStreak();
    }






    // ========================================== //
    // ========== PARAMETERS FUNCTIONS ========== //
    // ========================================== //


    //ElevatorMovementSpeed
    public float ElevatorMovementSpeed
    {
        get { return elevatorParameters.GetMovementSpeed(); }
        set
        {
            elevatorParameters.SetMovementSpeed(value);
        }
    }

    //ElevatorStartMovementSpeed
    public float ElevatorStartMovementSpeed
    {
        get { return elevatorParameters.GetStartMovementSpeed(); }
        set
        {
            elevatorParameters.SetStartMovementSpeed(value);
        }
    }

    //ElevatorMaxDifference
    public float ElevatorMaxDifference
    {
        get { return elevatorParameters.GetMaxDifference(); }
        set
        {
            elevatorParameters.SetMaxDifference(value);
        }
    }

    //FruitGravityScale
    public float FruitGravityScale
    {
        get { return elevatorParameters.GetFruitGravityScale(); }
        set
        {
            elevatorParameters.SetFruitGravityScale(value);
        }
    }

    //FruitFallingGravityScale
    public float FruitFallingGravityScale
    {
        get { return elevatorParameters.GetFruitFallingGravityScale(); }
        set
        {
            elevatorParameters.SetFruitFallingGravityScale(value);
        }
    }

    //FruitMinCollisionDistance
    public float FruitMinCollisionDistance
    {
        get { return elevatorParameters.GetFruitMinCollisionDistance(); }
        set
        {
            elevatorParameters.SetFruitMinCollisionDistance(value);
        }
    }
















    // =================================== //
    // ========== DEV FUNCTIONS ========== //
    // =================================== //

    public void QuickRemoveObstacles()
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


    public void QuickSpawnObstacles()
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
