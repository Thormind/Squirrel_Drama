using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CreditsMenuManager : MonoBehaviour
{
    [SerializeField] private Button anyKeyButton;
    [SerializeField] private GameObject anyKeyText;

    [SerializeField] private Toggle controlsToggle;
    [SerializeField] private Toggle artsToggle;
    [SerializeField] private Toggle creditsToggle;

    [SerializeField] private GameObject keyboardControlsPanel;
    [SerializeField] private GameObject gamepadControlsPanel;
    [SerializeField] private GameObject artsPanel;
    [SerializeField] private GameObject creditsPanel;

    [SerializeField] private GameObject flashText;

    private IEnumerator currentCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        anyKeyButton.onClick.AddListener(() => HandleAnyKey());

        controlsToggle.onValueChanged.AddListener(delegate { OnControlsToggleValueChanged(controlsToggle.isOn); });
        creditsToggle.onValueChanged.AddListener(delegate { OnCreditsToggleValueChanged(creditsToggle.isOn); });
        artsToggle.onValueChanged.AddListener(delegate { OnArtsToggleValueChanged(artsToggle.isOn); });

        GlobalUIManager.instance.es.SetSelectedGameObject(anyKeyButton.gameObject);
    }

    public void HandleAnyKey()
    {
        controlsToggle.isOn = false;
        creditsToggle.isOn = false;
        artsToggle.isOn = false;
        anyKeyText.GetComponent<flashingText>().StopFlash();
        anyKeyText.SetActive(false);
        GlobalUIManager.instance.SetMainMenu();
    }

    public void OnControlsToggleValueChanged(bool value)
    {
        keyboardControlsPanel.GetComponent<UIPanelAnimation>().AnimateFunction(value);
        gamepadControlsPanel.GetComponent<UIPanelAnimation>().AnimateFunction(value);
        GlobalUIManager.instance.es.SetSelectedGameObject(anyKeyButton.gameObject);
    }

    public void OnCreditsToggleValueChanged(bool value)
    {
        creditsPanel.GetComponent<UIPanelAnimation>().AnimateFunction(value);
        GlobalUIManager.instance.es.SetSelectedGameObject(anyKeyButton.gameObject);
    }

    public void OnArtsToggleValueChanged(bool value)
    {
        artsPanel.GetComponent<UIPanelAnimation>().AnimateFunction(value);
        GlobalUIManager.instance.es.SetSelectedGameObject(anyKeyButton.gameObject);
    }

    public void PlayAnimationCoroutine(IEnumerator coroutine)
    {
        if (currentCoroutine != null)
        {
            StartCoroutine(WaitForCoroutine(currentCoroutine, coroutine));
        }
        else
        {
            currentCoroutine = coroutine;
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator WaitForCoroutine(IEnumerator previousCoroutine, IEnumerator nextCoroutine)
    {
        yield return StartCoroutine(previousCoroutine);
        currentCoroutine = nextCoroutine;
        yield return StartCoroutine(nextCoroutine);
        currentCoroutine = null;
    }


}
