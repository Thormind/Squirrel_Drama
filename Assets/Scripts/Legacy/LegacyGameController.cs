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

        bestScore = SaveManager.instance.GetBestScore(GAME_MODE.LEGACY_MODE);
    }
    
    public void UpdateHUD()
    {
        HUDMenuManager.instance.UpdateLegacyHUD();
    }

    private void RecalculateScore()
    {
        score += bonusScore;
        UpdateHUD();
    }

    private void RecalculateBestScore()
    {
        SaveManager.instance.UpdateBestScore(GAME_MODE.LEGACY_MODE, score);
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

    public Vector3 GetBallPosition()
    {
        return ballRef.gameObject.transform.position;
    }

    public GameObject GetCurrentHole()
    {
        return LegacyHoleController.instance.holes[currentHoleIndex];
    }

    public void ResetBall()
    {
        ballRef.ResetBallPosition();
    }

    public void NextHole()
    {
        currentHoleIndex++;
        bonusScore = (currentHoleIndex + 1) * bonusScoreIncrement;
        UpdateHUD();
    }

    public void ResetGame()
    {
        currentBallNumber = numberOfBallsPerGame;
        score = 0;
        currentHoleIndex = 0;
        bonusScore = (currentHoleIndex + 1) * bonusScoreIncrement;

        gameCompletedState = false;
        gameOverState = true;

        LegacyMachineLight.SetActive(false);

        CameraManager.instance.SetUnfocus();

        LegacyHoleController.instance.RemoveHoles();

        elevatorControllerRef.MoveBarToBottomPositionFunction();

        CancelInvoke(nameof(DecreaseBonusScore));

        UpdateHUD();
    }

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        //ResetGame();

        gameOverState = false;

        LegacyHoleController.instance.SpawnHoles();

        CameraManager.instance.SetFocus();

        LegacyMachineLight.SetActive(true);

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

    public void ReadyForNextHole()
    {
        LegacyHoleController.instance.holeIndicatorList[currentHoleIndex].GetComponent<HoleIndicator>().StartPulsating();

        InvokeRepeating(nameof(DecreaseBonusScore), timePerDecrement, timePerDecrement);

        UpdateHUD();
    }

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

                GlobalUIManager.instance.SetScoreBoardMenu();
            }
            UpdateHUD();
        }

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

}
