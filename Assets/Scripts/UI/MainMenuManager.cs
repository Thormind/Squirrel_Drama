using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public TMP_Text storyBestScore;
    public TMP_Text infiniteBestScore;
    public TMP_Text legacyBestScore;

    [SerializeField] private Button playStoryButton;
    [SerializeField] private Button playInfiniteButton;
    [SerializeField] private Button playLegacyButton;

    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button playAnimationButton;

    [SerializeField] private Button quitButton;
    [SerializeField] private Button quitConfirmButton;
    [SerializeField] private Button quitCancelButton;

    public GameObject mainPanel;
    public GameObject quitPanel;

    // Start is called before the first frame update
    void Start()
    {
        playInfiniteButton.onClick.AddListener(() => GlobalUIManager.instance.LoadGame(0));
        playLegacyButton.onClick.AddListener(() => GlobalUIManager.instance.LoadGame(1));

        settingsButton.onClick.AddListener(() => GlobalUIManager.instance.SetSettingsMenu());
        creditsButton.onClick.AddListener(() => GlobalUIManager.instance.SetCreditsMenu());
        //playAnimationButton.onClick.AddListener(() => GlobalUIManager.instance.SetAnimationMenu());

        quitButton.onClick.AddListener(() => HandleQuitButton());
        quitConfirmButton.onClick.AddListener(() => GlobalUIManager.instance.QuitApplication());
        quitCancelButton.onClick.AddListener(() => HandleCancelButton());
    }

    private void HandleQuitButton()
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
