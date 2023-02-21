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

    // Start is called before the first frame update
    void Start()
    {
        playInfiniteButton.onClick.AddListener(() => GlobalUIManager.instance.StartGame(0));
        playLegacyButton.onClick.AddListener(() => GlobalUIManager.instance.StartGame(1));

        settingsButton.onClick.AddListener(() => GlobalUIManager.instance.SetSettingsMenu());
        creditsButton.onClick.AddListener(() => GlobalUIManager.instance.SetCreditsMenu());
        //playAnimationButton.onClick.AddListener(() => GlobalUIManager.instance.SetAnimationMenu());

        quitButton.onClick.AddListener(() => GlobalUIManager.instance.QuitApplication());
    }

}
