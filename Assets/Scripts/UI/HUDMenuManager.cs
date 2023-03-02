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

    public TMP_Text infiniteScoreText;
    public TMP_Text infiniteBonusText;
    public TMP_Text infiniteBestScoreText;
    public TMP_Text infiniteLevelText;
    public TMP_Text infiniteFruitLifeText;

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
        if (ScenesManager.instance.gameMode == GAME_MODE.INFINITE_MODE)
        {
            SetInfiniteHUD();
        }
        if (ScenesManager.instance.gameMode == GAME_MODE.LEGACY_MODE)
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
        InfiniteGameController.instance.UpdateHUD();
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
        legacyBestScoreText.text = SaveManager.instance.GetBestScore(GAME_MODE.LEGACY_MODE).ToString();
        legacyBallLifeText.text = LegacyGameController.instance.currentBallNumber.ToString();
    }

    public void UpdateInfiniteHUD()
    {
        infiniteScoreText.text = InfiniteGameController.instance.score.ToString();
        infiniteBonusText.text = InfiniteGameController.instance.bonusScore.ToString();
        infiniteBestScoreText.text = SaveManager.instance.GetBestScore(GAME_MODE.INFINITE_MODE).ToString();
        infiniteFruitLifeText.text = InfiniteGameController.instance.currentFruitNumber.ToString();
        infiniteLevelText.text = InfiniteGameController.instance.currentLevel.ToString();
    }
}
