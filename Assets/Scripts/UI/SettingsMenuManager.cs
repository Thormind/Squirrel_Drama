using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField] private Button resetButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [SerializeField] private Button confirmResetButton;
    [SerializeField] private Button cancelResetButton;

    public GameObject mainPanel;
    public GameObject resetPanel;

    public float masterVolume = 0.8f;
    public float musicVolume = 0.5f;
    public float sfxVolume = 1f;

    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(() => GlobalUIManager.instance.SetLastMenu());

        masterVolumeSlider.onValueChanged.AddListener(delegate { HandleMasterVolumeInputData(masterVolumeSlider.value); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { HandleMusicVolumeInputData(musicVolumeSlider.value); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { HandleSFXVolumeInputData(sfxVolumeSlider.value); });

        resetButton.onClick.AddListener(() => HandleResetButton());
        
        confirmResetButton.onClick.AddListener(() => HandleConfirmResetButton());
        cancelResetButton.onClick.AddListener(() => HandleCancelButton());

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            GlobalUIManager.instance.OnPauseResume();
        }
    }

    public void HandleMasterVolumeInputData(float volume)
    {
        masterVolume = volume;
    }

    public void HandleMusicVolumeInputData(float volume)
    {
        musicVolume = volume;
    }

    public void HandleSFXVolumeInputData(float volume)
    {
        sfxVolume = volume;
    }

    private void HandleResetButton()
    {
        mainPanel.SetActive(false);
        resetPanel.SetActive(true);
    }

    private void HandleCancelButton()
    {
        resetPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    private void HandleConfirmResetButton()
    {
        //SaveManager.ResetData();
        resetPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

}
