using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TMPro;

public class LegacyGameController : MonoBehaviour
{
    public static LegacyGameController instance = null;

    public LegacyElevatorController elevatorControllerRef;
    public LegacyBall ballRef;
    public GameObject LegacyMachineLight;

    public int score;
    public int bonusScore;
    public int bestScore;

    public int currentBallNumber = 3;
    public int numberOfBallsPerGame = 3;

    int currentHoleIndex;

    public float timePerDecrement = 5.0f;
    public int bonusScoreIncrement = 1000;
    public int bonusScoreDecrement = 100;

    //public bool gameCompletedState = false;
    //public bool gameOverState = false;

    [SerializeField] private LegacyElevatorParametersSO elevatorParameters;

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
        if (SaveManager.instance != null)
        {
            bestScore = SaveManager.instance.GetBestScore(GAME_MODE.LEGACY_MODE);
        }

        currentHoleIndex = 0;
        score = 0;
        bonusScore = (currentHoleIndex + 1) * bonusScoreIncrement;

        elevatorControllerRef.SetElevatorMovementSpeed(ElevatorMovementSpeed);
        elevatorControllerRef.SetElevatorMaxDifference(ElevatorMaxDifference);

        ballRef.SetBallGravityScale(BallGravityScale);
        ballRef.SetBallMinCollisionDistance(BallMinCollisionDistance);

        LegacyHoleController.instance.SetHolesQuantity(HolesQuantity);
        LegacyHoleController.instance.SetHolesMinDistance(HolesMinDistance);
    }

    public void UpdateHUD(GAME_DATA gameData)
    {
        if (HUDMenuManager.instance != null && HUDMenuManager.instance.isActiveAndEnabled)
        {
            HUDMenuManager.instance.UpdateLegacyHUD(gameData);
        }
    }




    // ========== SCORE FUNCTIONS ========== //

    private void RecalculateScore()
    {
        if (HUDMenuManager.instance != null && HUDMenuManager.instance.isActiveAndEnabled)
        {
            AnimationManager.instance.PlayInGameAnimation(
                HUDMenuManager.instance.AnimateLegacyScore(bonusScore, 0, score, score + bonusScore),
                () => {
                    GameCompletedCheck();
                });
        }
        score += bonusScore;
        UpdateHUD(GAME_DATA.SCORE);
    }

    private void RecalculateBestScore()
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.UpdateBestScore(GAME_MODE.LEGACY_MODE, score);
        }
    }

    private void DecreaseBonusScore()
    {
        if (bonusScore >= bonusScoreDecrement)
        {
            bonusScore -= bonusScoreDecrement;
            UpdateHUD(GAME_DATA.BONUS_SCORE);
        }
        else
        {
            bonusScore = 0;
        }
    }






    // ========== LEVEL TRANSITIONS ========== //

    public void ResetGame()
    {
        currentBallNumber = numberOfBallsPerGame;
        UpdateHUD(GAME_DATA.LIFE); 

        score = 0;
        UpdateHUD(GAME_DATA.SCORE);

        currentHoleIndex = 0;
        bonusScore = (currentHoleIndex + 1) * bonusScoreIncrement;
        UpdateHUD(GAME_DATA.BONUS_SCORE);

        //gameCompletedState = false;
        //gameOverState = true;

        ScenesManager.gameState = GAME_STATE.GAME_OVER;

        LegacyMachineLight.SetActive(false);

        ballRef.HideBall();

        LegacyHoleController.instance.RemoveHoles();

        PrepareForHole();

        elevatorControllerRef.MoveBarToBottomPositionFunction();


    }

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        //gameOverState = false;
        //gameCompletedState = false;

        ScenesManager.gameState = GAME_STATE.ACTIVE;

        ballRef.SetBallMinCollisionDistance(BallMinCollisionDistance);
        LegacyHoleController.instance.SetHolesQuantity(HolesQuantity);
        LegacyHoleController.instance.SetHolesMinDistance(HolesMinDistance);

        LegacyHoleController.instance.SpawnHoles();

        LegacyMachineLight.SetActive(true);

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    public void PrepareForHole()
    {
        CancelInvoke(nameof(DecreaseBonusScore));

        if (CameraManager.instance != null)
        {
            CameraManager.instance.Transition(false);
        }
    }

    public void ReadyForNextHole()
    {
        if (CameraManager.instance != null)
        {
            CameraManager.instance.Transition(true);
        }

        LegacyHoleController.instance.holeIndicatorList[currentHoleIndex].GetComponent<LegacyHoleIndicator>().StartPulsating();

        InvokeRepeating(nameof(DecreaseBonusScore), timePerDecrement, timePerDecrement);
    }

    public void NextHole()
    {
        currentHoleIndex++;
        bonusScore = (currentHoleIndex + 1) * bonusScoreIncrement;

        UpdateHUD(GAME_DATA.BONUS_SCORE);

        ScenesManager.gameState = GAME_STATE.ACTIVE;

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    public void ResetBall()
    {
        Vector3 positon = new Vector3(0, elevatorControllerRef.transform.localPosition.y + 0.25f, 0);
        ballRef.ResetBallPosition(positon);
    }

    public void GameCompletedCheck()
    {
        if (currentHoleIndex < LegacyHoleController.instance.holeIndicatorList.Count - 1)
        {
            NextHole();
        }
        else
        {
            ScenesManager.gameState = GAME_STATE.GAME_COMPLETED;

            if (CameraManager.instance != null)
            {
                CameraManager.instance.Transition(false);
            }

            elevatorControllerRef.MoveBarToBottomPositionFunction();

            AnimationManager.instance.PlayInGameAnimation( GameCompletedAnimation(), () => { ResetGame(); });

            //elevatorControllerRef.MoveBarToBottomPositionFunction();
        }
    }

    private IEnumerator GameCompletedAnimation()
    {
        foreach(GameObject holeIndicator in LegacyHoleController.instance.holeIndicatorList)
        {
            holeIndicator.GetComponent<LegacyHoleIndicator>().StartFlashing();
        }

        yield return new WaitForSeconds(5f);

        foreach (GameObject holeIndicator in LegacyHoleController.instance.holeIndicatorList)
        {
            holeIndicator.GetComponent<LegacyHoleIndicator>().StopFlashing();
        }

        GlobalUIManager.instance.ReplayGame();

    }

    public void GameOverCheck()
    {
        if (currentBallNumber <= 0)
        {
            ScenesManager.gameState = GAME_STATE.GAME_OVER;

            RecalculateBestScore();

            LegacyHoleController.instance.holeIndicatorList[currentHoleIndex].GetComponent<LegacyHoleIndicator>().EndPulsating();

            if (GlobalUIManager.instance != null)
            {
                GlobalUIManager.instance.SetScoreBoardMenu();
            }
        }
    }



    // ========== OBSTACLES HANDLES ========== //

    public void HandleBallInHole(bool rightHole)
    {
        CancelInvoke(nameof(DecreaseBonusScore));

        elevatorControllerRef.EnableInput(false);

        if (rightHole)
        {
            ScenesManager.gameState = GAME_STATE.LEVEL_COMPLETED;
            LegacyHoleController.instance.holeIndicatorList[currentHoleIndex].GetComponent<LegacyHoleIndicator>().EndPulsating();
            RecalculateScore();
        }
        else
        {
            currentBallNumber--;
            UpdateHUD(GAME_DATA.LIFE);
            elevatorControllerRef.MoveBarToBottomPositionFunction();

            GameOverCheck();
        }

    }



    // ========== GETTERS ========== //

    public Vector3 GetBallPosition()
    {
        return ballRef.gameObject.transform.position;
    }

    public Vector3 GetFruitLocalPosition()
    {
        return ballRef.gameObject.transform.localPosition;
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

    public GameObject GetCurrentHole()
    {
        return LegacyHoleController.instance.holes[currentHoleIndex];
    }



    //ElevatorMovementSpeed
    public float ElevatorMovementSpeed
    {
        get { return elevatorParameters.GetMovementSpeed(); }
        set
        {
            elevatorParameters.SetMovementSpeed(value); 
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

    //BallGravityScale
    public float BallGravityScale
    {
        get { return elevatorParameters.GetBallGravityScale(); }
        set
        {
            elevatorParameters.SetBallGravityScale(value);
        }
    }

    //BallMinCollisionDistance
    public float BallMinCollisionDistance
    {
        get { return elevatorParameters.GetBallMinCollisionDistance(); }
        set
        {
            elevatorParameters.SetBallMinCollisionDistance(value);
        }
    }

    //Holes Quantity
    public int HolesQuantity
    {
        get { return elevatorParameters.GetHolesQuantity(); }
        set
        {
            elevatorParameters.SetHolesQuantity(value);
        }
    }

    //Holes MinDistance

    public float HolesMinDistance
    {
        get { return elevatorParameters.GetHolesMinDistance(); }
        set
        {
            elevatorParameters.SetHolesMinDistance(value);
        }
    }


}
