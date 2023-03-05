using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PreGameMenuManager : MonoBehaviour
{
    public Keyboard keyboard;

    public GameObject infiniteText;
    public GameObject legacyText;

    [SerializeField] private Button anyKeyButton;

    // Start is called before the first frame update
    void Start()
    {
        keyboard = Keyboard.current;

        anyKeyButton.onClick.AddListener(() => HandleStartGame());

        if (ScenesManager.instance.gameMode == GAME_MODE.INFINITE_MODE)
        {
            AudioManager.instance.StopCurrentMusic();
            SetInfinitePregame();
        }
        if (ScenesManager.instance.gameMode == GAME_MODE.LEGACY_MODE)
        {
            AudioManager.instance.StopCurrentMusic();
            SetLegacyPregame();
        }

        GlobalUIManager.instance.es.SetSelectedGameObject(anyKeyButton.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (keyboard.anyKey.wasPressedThisFrame && !(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)))
        {
            HandleStartGame();
        }
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
        if (ScenesManager.instance.gameMode == GAME_MODE.INFINITE_MODE)
        {
            AudioManager.instance.PlayInfinite();
            GlobalUIManager.instance.SetHUDMenu();
            InfiniteGameController.instance.StartGame();
        }
        if (ScenesManager.instance.gameMode == GAME_MODE.LEGACY_MODE)
        {
            AudioManager.instance.PlayLegacy();
            GlobalUIManager.instance.SetHUDMenu();
            LegacyGameController.instance.StartGame();
            
        }
    }

}
