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

    public int score = 0;
    public int bonusScore = 1000;
    public int bestScore;

    public int currentBallNumber = 3;
    public int numberOfBallsPerGame = 3;

    int currentHoleIndex = 0;

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
    }

    public void UpdateHUD()
    {
        if (HUDMenuManager.instance != null)
        {
            HUDMenuManager.instance.UpdateLegacyHUD();
        }
    }




    // ========== SCORE FUNCTIONS ========== //

    private void RecalculateScore()
    {
        score += bonusScore;
        UpdateHUD();
    }

    private void RecalculateBestScore()
    {

        if (SaveManager.instance != null)
        {
            SaveManager.instance.UpdateBestScore(GAME_MODE.LEGACY_MODE, score);
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






    // ========== LEVEL TRANSITIONS ========== //

    public void ResetGame()
    {
        currentBallNumber = numberOfBallsPerGame;
        score = 0;
        currentHoleIndex = 0;
        bonusScore = (currentHoleIndex + 1) * bonusScoreIncrement;

        gameCompletedState = false;
        gameOverState = true;

        LegacyMachineLight.SetActive(false);

        LegacyHoleController.instance.RemoveHoles();

        PrepareForHole();

        elevatorControllerRef.MoveBarToBottomPositionFunction();

        UpdateHUD();
    }

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        gameOverState = false;

        LegacyHoleController.instance.SpawnHoles();

        LegacyMachineLight.SetActive(true);

        UpdateHUD();

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    public void PrepareForHole()
    {
        CancelInvoke(nameof(DecreaseBonusScore));

        if (CameraManager.instance != null)
        {
            CameraManager.instance.Transition(false);
        }

        UpdateHUD();
    }

    public void ReadyForNextHole()
    {
        if (CameraManager.instance != null)
        {
            CameraManager.instance.Transition(true);
        }

        LegacyHoleController.instance.holeIndicatorList[currentHoleIndex].GetComponent<HoleIndicator>().StartPulsating();

        InvokeRepeating(nameof(DecreaseBonusScore), timePerDecrement, timePerDecrement);

        UpdateHUD();
    }

    public void NextHole()
    {
        currentHoleIndex++;
        bonusScore = (currentHoleIndex + 1) * bonusScoreIncrement;
        UpdateHUD();
    }

    public void ResetBall()
    {
        ballRef.ResetBallPosition();
    }


    // ========== OBSTACLES HANDLES ========== //

    public void HandleBallInHole(bool rightHole)
    {
        CancelInvoke(nameof(DecreaseBonusScore));

        if (rightHole)
        {
            LegacyHoleController.instance.holeIndicatorList[currentHoleIndex].GetComponent<HoleIndicator>().EndPulsating();
            RecalculateScore();

            if (currentHoleIndex < LegacyHoleController.instance.holes.Count - 1)
            {
                NextHole();
            }
            else
            {
                gameCompletedState = true;
            }
        }
        else
        {
            currentBallNumber--;
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
            UpdateHUD();
        }

        elevatorControllerRef.MoveBarToBottomPositionFunction();
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
