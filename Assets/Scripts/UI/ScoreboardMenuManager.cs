using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardMenuManager : MonoBehaviour
{
    [SerializeField] private Button replayButton;
    [SerializeField] private Button optionButton;

    [SerializeField] private Button homeButton;
    [SerializeField] private Button confirmHomeButton;
    [SerializeField] private Button cancelHomeButton;

    public GameObject mainPanel;
    public GameObject quitPanel;

    // Start is called before the first frame update
    void Start()
    {
        //replayButton.onClick.AddListener(() => GlobalUIManager.instance.ReplayGame());
        optionButton.onClick.AddListener(() => GlobalUIManager.instance.SetSettingsMenu());

        homeButton.onClick.AddListener(() => HandleHomeButton());
        confirmHomeButton.onClick.AddListener(() => GlobalUIManager.instance.ReturnToMainMenu());
        cancelHomeButton.onClick.AddListener(() => HandleCancelButton());
    }

    private void HandleHomeButton()
    {
        mainPanel.SetActive(false);
        quitPanel.SetActive(true);
    }

    private void HandleCancelButton()
    {
        quitPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}
