using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public TMP_Text infiniteBestScore;
    public TMP_Text legacyBestScore;

    [SerializeField] private Button playInfiniteButton;
    [SerializeField] private Button playLegacyButton;

    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button playAnimationButton;

    [SerializeField] private Button returnButton;

    [SerializeField] private Button quitButton;
    [SerializeField] private Button quitConfirmButton;
    [SerializeField] private Button quitCancelButton;

    public GameObject mainPanel;
    public GameObject quitPanel;

    // Start is called before the first frame update
    void Start()
    {
        playInfiniteButton.onClick.AddListener(() => GlobalUIManager.instance.LoadGame(GAME_MODE.INFINITE_MODE));
        playLegacyButton.onClick.AddListener(() => GlobalUIManager.instance.LoadGame(GAME_MODE.LEGACY_MODE));

        settingsButton.onClick.AddListener(() => GlobalUIManager.instance.SetSettingsMenu());
        creditsButton.onClick.AddListener(() => GlobalUIManager.instance.SetCreditsMenu());
        //playAnimationButton.onClick.AddListener(() => GlobalUIManager.instance.SetAnimationMenu());

        returnButton.onClick.AddListener(() => GlobalUIManager.instance.SetTitleScreenMenu());

        quitButton.onClick.AddListener(() => HandleQuitButton());
        quitConfirmButton.onClick.AddListener(() => GlobalUIManager.instance.QuitApplication());
        quitCancelButton.onClick.AddListener(() => HandleCancelButton());
    }

    private void OnEnable()
    {
        quitPanel.SetActive(false);
        mainPanel.SetActive(true);

        infiniteBestScore.text = SaveManager.instance.GetBestScore(GAME_MODE.INFINITE_MODE).ToString();
        legacyBestScore.text = SaveManager.instance.GetBestScore(GAME_MODE.LEGACY_MODE).ToString();

        GlobalUIManager.instance.specificMenu = MENU.MENU_TITLE_SCREEN;
        GlobalUIManager.instance.SetFirstSelected(playInfiniteButton.gameObject);
    }

    private void HandleQuitButton()
    {
        mainPanel.SetActive(false);
        quitPanel.SetActive(true);
        GlobalUIManager.instance.SetFirstSelected(quitCancelButton.gameObject);
    }

    private void HandleCancelButton()
    {
        quitPanel.SetActive(false);
        mainPanel.SetActive(true);
        GlobalUIManager.instance.SetFirstSelected(quitButton.gameObject);
    }
}
