using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PreGameMenuManager : MonoBehaviour
{
    public GameObject infiniteText;
    public GameObject legacyText;

    [SerializeField] private Button anyKeyButton;

    // Start is called before the first frame update
    void Start()
    {
        GlobalUIManager.isPreGame = true;

        anyKeyButton.onClick.AddListener(() => HandleStartGame());

        if (ScenesManager.instance.gameMode == GAME_MODE.INFINITE_MODE)
        {
            SetInfinitePregame();
        }
        if (ScenesManager.instance.gameMode == GAME_MODE.LEGACY_MODE)
        {
            SetLegacyPregame();
        }

        GlobalUIManager.instance.es.SetSelectedGameObject(anyKeyButton.gameObject);
    }

    public void SetInfinitePregame()
    {
        legacyText.SetActive(false);
        infiniteText.SetActive(true);
    }

    public void SetLegacyPregame()
    {
        infiniteText.SetActive(false);
        legacyText.SetActive(true);
    }

    public void HandleStartGame()
    {
        GlobalUIManager.isPreGame = false;

        if (ScenesManager.instance.gameMode == GAME_MODE.INFINITE_MODE)
        {
            GlobalUIManager.instance.SetHUDMenu();
            InfiniteGameController.instance.StartGame();
        }
        if (ScenesManager.instance.gameMode == GAME_MODE.LEGACY_MODE)
        {
            GlobalUIManager.instance.SetHUDMenu();
            LegacyGameController.instance.StartGame();

        }
    }

}
