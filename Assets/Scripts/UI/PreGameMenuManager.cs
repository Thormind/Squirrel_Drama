using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PreGameMenuManager : MonoBehaviour
{
    public GameObject infiniteText;
    public GameObject legacyText;

    public GameObject flashingInfo;

    public float flashInterval = 0.5f;

    [SerializeField] private Button anyKeyButton;

    // Start is called before the first frame update
    void Start()
    {
        anyKeyButton.onClick.AddListener(() => HandleStartGame());
    }

    private void OnEnable()
    {
        AudioManager.instance.StopCurrentMusic();
        if (ScenesManager.gameMode == GAME_MODE.INFINITE_MODE)
        {
            AudioManager.instance.Playwind();
        }

        Flash(true);

        GlobalUIManager.instance.specificMenu = MENU.NONE;
        GlobalUIManager.instance.es.SetSelectedGameObject(anyKeyButton.gameObject);
    }


    public void HandleStartGame()
    {
        Flash(false);

        if (ScenesManager.gameMode == GAME_MODE.INFINITE_MODE)
        {
            AudioManager.instance.PlayInfinite();
            GlobalUIManager.instance.SetHUDMenu();
            InfiniteGameController.instance.StartGame();
        }
        if (ScenesManager.gameMode == GAME_MODE.LEGACY_MODE)
        {
            AudioManager.instance.PlayLegacy();
            GlobalUIManager.instance.SetHUDMenu();
            LegacyGameController.instance.StartGame();
            
        }
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
