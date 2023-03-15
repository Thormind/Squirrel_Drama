using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CreditsMenuManager : MonoBehaviour
{
    [SerializeField] private Button exitButton;

    [SerializeField] private Toggle controlsToggle;
    [SerializeField] private Toggle artsToggle;
    [SerializeField] private Toggle creditsToggle;

    [SerializeField] private GameObject keyboardControlsPanel;
    [SerializeField] private GameObject gamepadControlsPanel;
    [SerializeField] private GameObject artsPanel;
    [SerializeField] private GameObject creditsPanel;

    private IEnumerator currentCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        UIAnimation.OnAnimationUICalled += HandleAnimationUICalled;

        exitButton.onClick.AddListener(() => HandleAnyKey());

        controlsToggle.onValueChanged.AddListener(delegate { OnControlsToggleValueChanged(controlsToggle.isOn); });
        creditsToggle.onValueChanged.AddListener(delegate { OnCreditsToggleValueChanged(creditsToggle.isOn); });
        artsToggle.onValueChanged.AddListener(delegate { OnArtsToggleValueChanged(artsToggle.isOn); });
    }

    private void OnDestroy()
    {
        UIAnimation.OnAnimationUICalled -= HandleAnimationUICalled;
    }

    private void OnEnable()
    {
        GlobalUIManager.instance.specificMenu = MENU.MENU_MAIN;
        GlobalUIManager.instance.SetFirstSelected(exitButton.gameObject);
    }

    private void HandleAnimationUICalled()
    {
        controlsToggle.isOn = false;
        creditsToggle.isOn = false;
        artsToggle.isOn = false;
    }

    public void HandleAnyKey()
    {
        HandleAnimationUICalled();

        GlobalUIManager.instance.SetMainMenu();
    }

    public void OnControlsToggleValueChanged(bool value)
    {
        keyboardControlsPanel.GetComponent<UIPanelAnimation>().AnimateFunction(value);
        gamepadControlsPanel.GetComponent<UIPanelAnimation>().AnimateFunction(value);
    }

    public void OnCreditsToggleValueChanged(bool value)
    {
        creditsPanel.GetComponent<UIPanelAnimation>().AnimateFunction(value);
    }

    public void OnArtsToggleValueChanged(bool value)
    {
        artsPanel.GetComponent<UIPanelAnimation>().AnimateFunction(value);
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
