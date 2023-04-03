using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class TitleScreenMenuManager : MonoBehaviour
{
    [SerializeField] private Button anyKeyButton;
    [SerializeField] private GameObject anyKeyText;

    [SerializeField] private Button musicButton;
    [SerializeField] private TMP_Text musicText;

    // Start is called before the first frame update
    void Start()
    {
        anyKeyButton.onClick.AddListener(() => HandleAnyKey());
        musicButton.onClick.AddListener(() => HandleMusicChanged());
    }

    private void OnEnable()
    {
        AudioManager.instance.PlayIntro();
        anyKeyText.GetComponent<flashingText>().StartFlashing();

        musicText.text = AudioManager.instance.GetMusicName(ScenesManager.gameMode);

        GlobalUIManager.instance.specificMenu = MENU.MENU_MAIN;
        GlobalUIManager.instance.es.SetSelectedGameObject(anyKeyButton.gameObject);
    }

    public void HandleAnyKey()
    {
        anyKeyText.GetComponent<flashingText>().StopFlashing();

        GlobalUIManager.instance.SetMainMenu();
    }

    public void HandleMusicChanged()
    {
        string musicName = AudioManager.instance.SwitchMusic(ScenesManager.gameMode);
        musicText.text = musicName;
    }
}
