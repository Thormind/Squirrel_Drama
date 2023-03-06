using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button replayButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button homeButton;

    [SerializeField] private Button confirmRetryButton;
    [SerializeField] private Button cancelRetryButton;
    [SerializeField] private Button confirmQuitButton;
    [SerializeField] private Button cancelQuitButton;

    public GameObject mainPanel;
    public GameObject retryPanel;
    public GameObject quitPanel;

    // Start is called before the first frame update
    void Start()
    {
        GlobalUIManager.instance.SetControllerFirstSelected(replayButton.gameObject);

        resumeButton.onClick.AddListener(() => GlobalUIManager.instance.ResumeGame());
        optionButton.onClick.AddListener(() => GlobalUIManager.instance.SetSettingsMenu());

        replayButton.onClick.AddListener(() => HandleRetryButton());
        confirmRetryButton.onClick.AddListener(() => HandleConfirmRetryButton());
        cancelRetryButton.onClick.AddListener(() => HandleCancelButton());

        homeButton.onClick.AddListener(() => HandleQuitButton());
        confirmQuitButton.onClick.AddListener(() => GlobalUIManager.instance.ReturnToMainMenu());
        cancelQuitButton.onClick.AddListener(() => HandleCancelButton());
    }

    private void HandleRetryButton()
    {
        mainPanel.SetActive(false);
        retryPanel.SetActive(true);
        GlobalUIManager.instance.SetControllerFirstSelected(cancelRetryButton.gameObject);
    }

    private void HandleQuitButton()
    {
        mainPanel.SetActive(false);
        quitPanel.SetActive(true);
        GlobalUIManager.instance.SetControllerFirstSelected(cancelQuitButton.gameObject);
    }

    private void HandleCancelButton()
    {
        retryPanel.SetActive(false);
        quitPanel.SetActive(false);
        mainPanel.SetActive(true);
        GlobalUIManager.instance.SetControllerFirstSelected(replayButton.gameObject);
    }

    private void HandleConfirmRetryButton()
    {
        if (ScenesManager.instance.gameMode == GAME_MODE.INFINITE_MODE)
        {
            InfiniteGameController.instance.ResetGame();
            GlobalUIManager.instance.ReplayGame();
        }
        if (ScenesManager.instance.gameMode == GAME_MODE.LEGACY_MODE)
        {
            LegacyGameController.instance.ResetGame();
            GlobalUIManager.instance.ReplayGame();
        }
    }
}
