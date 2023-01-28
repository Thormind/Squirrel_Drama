using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button replayButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button homeButton;

    // Start is called before the first frame update
    void Start()
    {
        resumeButton.onClick.AddListener(() => GlobalUIManager.instance.ResumeGame());
        //replayButton.onClick.AddListener(() => GlobalUIManager.instance.ReplayGame());
        optionButton.onClick.AddListener(() => GlobalUIManager.instance.SetSettingsMenu());
        homeButton.onClick.AddListener(() => GlobalUIManager.instance.ReturnToMainMenu());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
