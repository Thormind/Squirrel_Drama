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

    public bool gameCompletedState = false;
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

        if (SaveManager.instance != null)
        {
            bestScore = SaveManager.instance.GetBestScore(GAME_MODE.LEGACY_MODE);
        }


        currentHoleIndex = 0;
        score = 0;
        bonusScore = (currentHoleIndex + 1) * bonusScoreIncrement;
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

        gameCompletedState = false;
        gameOverState = true;

        LegacyMachineLight.SetActive(false);

        LegacyHoleController.instance.RemoveHoles();

        PrepareForHole();

        elevatorControllerRef.MoveBarToBottomPositionFunction();


    }

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        gameOverState = false;

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

        LegacyHoleController.instance.holeIndicatorList[currentHoleIndex].GetComponent<HoleIndicator>().StartPulsating();

        InvokeRepeating(nameof(DecreaseBonusScore), timePerDecrement, timePerDecrement);
    }

    public void NextHole()
    {
        currentHoleIndex++;
        bonusScore = (currentHoleIndex + 1) * bonusScoreIncrement;

        UpdateHUD(GAME_DATA.BONUS_SCORE);

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    public void ResetBall()
    {
        ballRef.ResetBallPosition();
    }

    public void GameCompletedCheck()
    {
        if (currentHoleIndex < LegacyHoleController.instance.holes.Count - 1)
        {
            NextHole();
        }
        else
        {
            gameCompletedState = true;
        }
    }



    // ========== OBSTACLES HANDLES ========== //

    public void HandleBallInHole(bool rightHole)
    {
        CancelInvoke(nameof(DecreaseBonusScore));

        if (rightHole)
        {
            LegacyHoleController.instance.holeIndicatorList[currentHoleIndex].GetComponent<HoleIndicator>().EndPulsating();
            RecalculateScore();
        }
        else
        {
            currentBallNumber--;
            UpdateHUD(GAME_DATA.LIFE);
            elevatorControllerRef.MoveBarToBottomPositionFunction();

            if (currentBallNumber <= 0)
            {

                gameOverState = true;

                RecalculateBestScore();

                LegacyHoleController.instance.holeIndicatorList[currentHoleIndex].GetComponent<HoleIndicator>().EndPulsating();

                if (GlobalUIManager.instance != null)
                {
                    GlobalUIManager.instance.SetScoreBoardMenu();
                }
            }
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
}
