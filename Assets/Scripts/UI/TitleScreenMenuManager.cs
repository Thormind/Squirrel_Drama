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

    private InputAction previous;
    private InputAction next;
    private bool selectNext = true;

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
        GlobalUIManager.instance.SetSelected(anyKeyButton.gameObject, BUTTON.ANY_KEY);
        GlobalUIManager.selectedButton = BUTTON.ANY_KEY;

        previous = GlobalUIManager.instance.UIControls.UI.Previous;
        next = GlobalUIManager.instance.UIControls.UI.Next;

        previous.Enable();
        next.Enable();

        previous.performed += HandleMusicChangedToPrevious;
        next.performed += HandleMusicChangedToNext;
    }

    private void OnDisable()
    {
        previous.Disable();
        next.Disable();

        previous.performed -= HandleMusicChangedToPrevious;
        next.performed -= HandleMusicChangedToNext;
    }


    public void HandleAnyKey()
    {
        anyKeyText.GetComponent<flashingText>().StopFlashing();

        GlobalUIManager.instance.SetMainMenu();
    }

    public void HandleMusicChangedToPrevious(InputAction.CallbackContext context)
    {
        selectNext = false;
        musicButton.GetComponent<ButtonAnimation>().OnManualSubmit();
        musicButton.onClick.Invoke();
    }

    public void HandleMusicChangedToNext(InputAction.CallbackContext context)
    {
        selectNext = true;
        musicButton.GetComponent<ButtonAnimation>().OnManualSubmit();
        musicButton.onClick.Invoke();
    }

    public void HandleMusicChanged()
    {
        string musicName = AudioManager.instance.SwitchMusic(ScenesManager.gameMode, selectNext);
        musicText.text = musicName;
        GlobalUIManager.instance.SetSelected(anyKeyButton.gameObject, BUTTON.ANY_KEY);
        selectNext = true;
    }
}
