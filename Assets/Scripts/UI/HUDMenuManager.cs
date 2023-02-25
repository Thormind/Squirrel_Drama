using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDMenuManager : MonoBehaviour
{
    public static HUDMenuManager instance;

    public GameObject infinitePanel;

    public GameObject legacyPanel;

    public TMP_Text legacyScoreText;
    public TMP_Text legacyBonusText;
    public TMP_Text legacyBestScoreText;
    public TMP_Text legacyBallLifeText;

    public void Awake()
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

    // Start is called before the first frame update
    void Start()
    {
        if (ScenesManager.instance.gameMode == 0)
        {
            SetInfiniteHUD();
        }
        if (ScenesManager.instance.gameMode == 1)
        {
            SetLegacyHUD();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            GlobalUIManager.instance.OnPauseResume();
        }
    }

    public void SetInfiniteHUD()
    {
        legacyPanel.SetActive(false);
        infinitePanel.SetActive(true);
        //InfiniteGameController.instance.UpdateHUD();
    }

    public void SetLegacyHUD()
    {
        infinitePanel.SetActive(false);
        legacyPanel.SetActive(true);
        LegacyGameController.instance.UpdateHUD();
    }

    public void UpdateLegacyHUD()
    {
        legacyScoreText.text = LegacyGameController.instance.score.ToString();
        legacyBonusText.text = LegacyGameController.instance.bonusScore.ToString();
        legacyBestScoreText.text = LegacyGameController.instance.bestScore.ToString();
        legacyBallLifeText.text = LegacyGameController.instance.currentBallNumber.ToString();
    }

    public void UpdateInfiniteHUD(float score, float bonusScore, float bestScore, float life, float level)
    {
        //infiniteScoreText.text = score.ToString();
        //infiniteBonusText.text = bonusScore.ToString();
        //infiniteBestScoreText.text = bestScore.ToString();
        //infiniteBallLifeText.text = life.ToString();
        //infiniteLevelText.text = level.ToString();
    }
}
