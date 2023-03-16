using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PreGameMenuManager : MonoBehaviour
{
    public GameObject infiniteText;
    public GameObject legacyText;

    public float flashInterval = 0.5f;

    [SerializeField] private Button anyKeyButton;

    // Start is called before the first frame update
    void Start()
    {
        GlobalUIManager.isPreGame = true;

        anyKeyButton.onClick.AddListener(() => HandleStartGame());

        if (ScenesManager.instance.gameMode == GAME_MODE.INFINITE_MODE)
        {
            AudioManager.instance.StopCurrentMusic();
            AudioManager.instance.Playwind();
        }
        if (ScenesManager.instance.gameMode == GAME_MODE.LEGACY_MODE)
        {
            AudioManager.instance.StopCurrentMusic();
        }

        StartFlash();

        GlobalUIManager.instance.es.SetSelectedGameObject(anyKeyButton.gameObject);
    }

    public void HandleStartGame()
    {
        GlobalUIManager.isPreGame = false;

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

    private void OnEnable()
    {

        GlobalUIManager.isPreGame = true;

        if (ScenesManager.instance.gameMode == GAME_MODE.INFINITE_MODE)
        {
            AudioManager.instance.StopCurrentMusic();
        }
        if (ScenesManager.instance.gameMode == GAME_MODE.LEGACY_MODE)
        {
            AudioManager.instance.StopCurrentMusic();
        }

        StartFlash();

        GlobalUIManager.instance.es.SetSelectedGameObject(anyKeyButton.gameObject);
    }

    private void Flash()
    {
        if (ScenesManager.instance.gameMode == GAME_MODE.INFINITE_MODE)
        {
            infiniteText.SetActive(!infiniteText.activeSelf);
        }
        if (ScenesManager.instance.gameMode == GAME_MODE.LEGACY_MODE)
        {
            legacyText.SetActive(!legacyText.activeSelf);
        }

    }

    public void StartFlash()
    {
        StopFlash();
        infiniteText.SetActive(false);
        legacyText.SetActive(false);
        InvokeRepeating($"Flash", 0, flashInterval);
    }

    public void StopFlash()
    {
        CancelInvoke("Flash");
    }

}
