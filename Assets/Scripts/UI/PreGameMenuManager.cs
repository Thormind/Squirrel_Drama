using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PreGameMenuManager : MonoBehaviour
{

    public GameObject infiniteText;
    public GameObject legacyText;

    // Start is called before the first frame update
    void Start()
    {
        if (ScenesManager.instance.gameMode == 1)
        {
            SetInfinitePregame();
        }
        if (ScenesManager.instance.gameMode == 2)
        {
            SetLegacyPregame();
        }
    }

    // Update is called once per frame
    void Update()
    {
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

    private void OnConfirm()
    {
        if (ScenesManager.instance.gameMode == 1)
        {
            GlobalUIManager.instance.SetHUDMenu();
            //InfiniteGameController.instance.StartGame();
        }
        if (ScenesManager.instance.gameMode == 2)
        {
            GlobalUIManager.instance.SetHUDMenu();
            LegacyGameController.instance.StartGame();

        }
    }

}
