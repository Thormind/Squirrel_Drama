using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System;

public class PreGameMenuManager : MonoBehaviour
{
    public GameObject infiniteText;
    public GameObject legacyText;

    public GameObject flashingInfo;

    public float flashInterval = 0.5f;

    [SerializeField] private Button anyKeyButton;

    [SerializeField] private Button musicButton;
    [SerializeField] private TMP_Text musicText;

    // Start is called before the first frame update
    void Start()
    {
        anyKeyButton.onClick.AddListener(() => HandleStartGame());
        musicButton.onClick.AddListener(() => HandleMusicChanged());
    }

    private void OnEnable()
    {
        Flash(true);

        musicText.text = AudioManager.instance.GetMusicName(ScenesManager.gameMode);

        GlobalUIManager.instance.specificMenu = MENU.NONE;
        GlobalUIManager.instance.es.SetSelectedGameObject(anyKeyButton.gameObject);
    }


    public void HandleStartGame()
    {
        Flash(false);

        if (ScenesManager.gameMode == GAME_MODE.INFINITE_MODE)
        {
            GlobalUIManager.instance.SetHUDMenu();
            InfiniteGameController.instance.StartGame();
        }
        if (ScenesManager.gameMode == GAME_MODE.LEGACY_MODE)
        {
            GlobalUIManager.instance.SetHUDMenu();
            LegacyGameController.instance.StartGame();
        }
    }

    public void HandleMusicChanged()
    {
        string musicName = AudioManager.instance.SwitchMusic(ScenesManager.gameMode);
        musicText.text = musicName;
    }

    private void Flash(bool isActive)
    {
        if (isActive)
        {
            if (ScenesManager.gameMode == GAME_MODE.INFINITE_MODE)
            {
                flashingInfo.GetComponent<flashingText>().StartFlashing(infiniteText);
            }
            if (ScenesManager.gameMode == GAME_MODE.LEGACY_MODE)
            {
                flashingInfo.GetComponent<flashingText>().StartFlashing(legacyText);
            }
        }
        else
        {
            if (ScenesManager.gameMode == GAME_MODE.INFINITE_MODE)
            {
                flashingInfo.GetComponent<flashingText>().StopFlashing(infiniteText);
            }
            if (ScenesManager.gameMode == GAME_MODE.LEGACY_MODE)
            {
                flashingInfo.GetComponent<flashingText>().StopFlashing(legacyText);
            }
        }
    }

}
