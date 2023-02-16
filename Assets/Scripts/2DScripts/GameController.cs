using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;

    public ElevatorController elevatorControllerRef;
    public Ball ballRef;

    float score = 0;
    float bonusScore = 100;
    float bestScore = 0;

    int currentBallNumber = 1;
    public int numberOfBallsPerGame = 3;

    int currentHoleIndex = 0;

    public float timePerDecrement = 10.0f;
    public float bonusScoreDecrement = 5.0f;

    public bool gameCompletedState = false;
    public bool gameOverState = false;

    public TMP_Text ballText;
    public TMP_Text ballText2;
    public TMP_Text scoreText;
    public TMP_Text scoreText2;
    public TMP_Text bestScoreText;
    public TMP_Text bestScoreText2;
    public TMP_Text bonusText;
    public TMP_Text bonusText2;
    public TMP_Text gameOverText;
    public GameObject StartButton;

    public VisualEffect leftLifterVFX;
    public VisualEffect rightLifterVFX;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        leftLifterVFX.Stop();
        rightLifterVFX.Stop();
    }
    private void UpdateUI()
    {
        ballText.text = currentBallNumber.ToString();
        scoreText.text = score.ToString();
        bonusText.text = bonusScore.ToString();
        bestScoreText.text = bestScore.ToString();
    }

    private void RecalculateScore()
    {
        score += bonusScore;
        UpdateUI();
    }

    private void RecalculateBestScore()
    {
        if (bestScore < score)
        {
            bestScore = score;
        }
        UpdateUI();
    }

    private void DecreaseBonusScore()
    {
        if (bonusScore >= bonusScoreDecrement)
        {
            bonusScore -= bonusScoreDecrement;
            UpdateUI();
        }
        else
        {
            bonusScore = 0;
            UpdateUI();
        }
    }

    public GameObject GetCurrentHole()
    {
        return HoleController.instance.holes[currentHoleIndex];
    }

    public void ResetBall()
    {
        ballRef.ResetBallPosition();
    }

    public void NextHole()
    {
        currentHoleIndex++;
        bonusScore = (currentHoleIndex + 1) * 100.0f;
        UpdateUI();
    }

    void ResetGame()
    {
        currentBallNumber = 1;
        score = 0;
        currentHoleIndex = 0;
        bonusScore = (currentHoleIndex + 1) * 100.0f;

        gameCompletedState = false;
        gameOverState = false;

        StartButton.SetActive(false);
        gameOverText.enabled = false;
        scoreText.enabled = true;
        scoreText2.enabled = true;
        bestScoreText.enabled = true;
        bestScoreText2.enabled = true;
        bonusText.enabled = true;
        bonusText2.enabled = true;
        ballText.enabled = true;
        ballText2.enabled = true;

        leftLifterVFX.Play();
        rightLifterVFX.Play();

        CameraFollow2.instance.isGameActive = true;

        HoleController.instance.SpawnHoles();
        BeesController.instance.SpawnBees();

        UpdateUI();
    }

    void StartGame()
    {
        ResetGame();

        elevatorControllerRef.MoveBarToStartPositionFunction();
    }

    public void ReadyForNextHole()
    {
        HoleController.instance.holeIndicatorList[currentHoleIndex].GetComponent<HoleIndicator>().StartPulsating();

        InvokeRepeating(nameof(DecreaseBonusScore), timePerDecrement, timePerDecrement);

        UpdateUI();
    }

    public void HandleBallInHole(bool rightHole)
    {
        CancelInvoke(nameof(DecreaseBonusScore));
        if(rightHole)
        {
            HoleController.instance.holeIndicatorList[currentHoleIndex].GetComponent<HoleIndicator>().EndPulsating();
            RecalculateScore();

            if(currentHoleIndex < HoleController.instance.holes.Count - 1)
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
            currentBallNumber++;
            if(currentBallNumber > numberOfBallsPerGame)
            {
                gameOverState = true;

                StartButton.SetActive(true);
                gameOverText.enabled = true;
                bestScoreText.enabled = true;
                bestScoreText2.enabled = true;
                ballText.enabled = false;
                ballText2.enabled = false;
                scoreText.enabled = false;
                scoreText2.enabled = false;
                bonusText.enabled = false;
                bonusText2.enabled = false;

                leftLifterVFX.Stop();
                rightLifterVFX.Stop();

                CameraFollow2.instance.isGameActive = false;

                RecalculateBestScore();

                HoleController.instance.holeIndicatorList[currentHoleIndex].GetComponent<HoleIndicator>().EndPulsating();
            }
            UpdateUI();
        }

        elevatorControllerRef.MoveBarToBottomPositionFunction();
    }

}
