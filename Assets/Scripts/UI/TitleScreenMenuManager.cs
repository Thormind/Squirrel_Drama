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

        anyKeyText.GetComponent<flashingText>().StartFlash();

        GlobalUIManager.instance.es.SetSelectedGameObject(anyKeyButton.gameObject);
    }

    public void HandleAnyKey()
    {
        anyKeyText.GetComponent<flashingText>().StopFlash();
        anyKeyText.SetActive(false);
        GlobalUIManager.instance.SetMainMenu();
    }

    private void OnEnable()
    {
        anyKeyText.SetActive(true);
        anyKeyText.GetComponent<flashingText>().StartFlash();
        GlobalUIManager.instance.es.SetSelectedGameObject(anyKeyButton.gameObject);
    }
}
