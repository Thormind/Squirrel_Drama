using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreboardMenuManager : MonoBehaviour
{
    [SerializeField] private Button replayButton;
    [SerializeField] private Button optionButton;

    [SerializeField] private Button homeButton;
    [SerializeField] private Button confirmHomeButton;
    [SerializeField] private Button cancelHomeButton;

    public TMP_Text ScoreText;
    public TMP_Text BestScoreText;

    public GameObject mainPanel;
    public GameObject quitPanel;

    [SerializeField] private GameObject firstSlected;

    // Start is called before the first frame update
    void Start()
    {
        replayButton.onClick.AddListener(() => HandleReplayButton());
        optionButton.onClick.AddListener(() => GlobalUIManager.instance.SetSettingsMenu());

        homeButton.onClick.AddListener(() => HandleHomeButton());
        confirmHomeButton.onClick.AddListener(() => GlobalUIManager.instance.ReturnToMainMenu());
        cancelHomeButton.onClick.AddListener(() => HandleCancelButton());
    }

    private void OnEnable()
    {
        quitPanel.SetActive(false);
        mainPanel.SetActive(true);

        GlobalUIManager.instance.specificMenu = MENU.MENU_MAIN;
        GlobalUIManager.instance.SetFirstSelected(replayButton.gameObject);
        SetFinalScore();
    }

    private void HandleHomeButton()
    {
        mainPanel.SetActive(false);
        quitPanel.SetActive(true);
        GlobalUIManager.instance.SetFirstSelected(cancelHomeButton.gameObject, true);
    }

    private void HandleCancelButton()
    {
        quitPanel.SetActive(false);
        mainPanel.SetActive(true);
        GlobalUIManager.instance.SetFirstSelected(replayButton.gameObject, true);
    }

    private void HandleReplayButton()
    {
        GlobalUIManager.instance.ReplayGame();
    }

    public void SetFinalScore()
    {
        if (ScenesManager.gameMode == GAME_MODE.INFINITE_MODE)
        {
            ScoreText.text = InfiniteGameController.instance.score.ToString();
            BestScoreText.text = SaveManager.instance.GetBestScore(GAME_MODE.INFINITE_MODE).ToString();
        }
        if (ScenesManager.gameMode == GAME_MODE.LEGACY_MODE)
        {
            ScoreText.text = LegacyGameController.instance.score.ToString();
            BestScoreText.text = SaveManager.instance.GetBestScore(GAME_MODE.LEGACY_MODE).ToString();
        }
    }
}
