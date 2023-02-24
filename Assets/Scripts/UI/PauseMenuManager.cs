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
        resumeButton.onClick.AddListener(() => GlobalUIManager.instance.ResumeGame());
        optionButton.onClick.AddListener(() => GlobalUIManager.instance.SetSettingsMenu());

        replayButton.onClick.AddListener(() => HandleRetryButton());
        //confirmRetryButton.onClick.AddListener(() => GlobalUIManager.instance.ReplayGame());
        cancelRetryButton.onClick.AddListener(() => HandleCancelButton());

        homeButton.onClick.AddListener(() => HandleQuitButton());
        confirmQuitButton.onClick.AddListener(() => GlobalUIManager.instance.ReturnToMainMenu());
        cancelQuitButton.onClick.AddListener(() => HandleCancelButton());
    }

    private void HandleRetryButton()
    {
        mainPanel.SetActive(false);
        retryPanel.SetActive(true);
    }

    private void HandleQuitButton()
    {
        mainPanel.SetActive(false);
        quitPanel.SetActive(true);
    }

    private void HandleCancelButton()
    {
        retryPanel.SetActive(false);
        quitPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}
