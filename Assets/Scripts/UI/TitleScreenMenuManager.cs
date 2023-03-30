using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TitleScreenMenuManager : MonoBehaviour
{
    [SerializeField] private Button anyKeyButton;
    [SerializeField] private GameObject anyKeyText;

    // Start is called before the first frame update
    void Start()
    {
        anyKeyButton.onClick.AddListener(() => HandleAnyKey());
    }

    private void OnEnable()
    {
        AudioManager.instance.PlayIntro();
        anyKeyText.GetComponent<flashingText>().StartFlashing();

        GlobalUIManager.instance.specificMenu = MENU.MENU_MAIN;
        GlobalUIManager.instance.es.SetSelectedGameObject(anyKeyButton.gameObject);
    }

    public void HandleAnyKey()
    {
        anyKeyText.GetComponent<flashingText>().StopFlashing();

        GlobalUIManager.instance.SetMainMenu();
    }
}
