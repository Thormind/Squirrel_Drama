using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField] private Button noonButton;
    [SerializeField] private Button nightButton;

    [SerializeField] private Button resetButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [SerializeField] private Button confirmResetButton;
    [SerializeField] private Button cancelResetButton;

    public GameObject mainPanel;
    public GameObject resetPanel;


    // Start is called before the first frame update
    void Start()
    {
        GlobalUIManager.instance.SetControllerFirstSelected(exitButton.gameObject);

        exitButton.onClick.AddListener(() => GlobalUIManager.instance.SetLastMenu());

        masterVolumeSlider.onValueChanged.AddListener(delegate { HandleMasterVolumeInputData(masterVolumeSlider.value); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { HandleMusicVolumeInputData(musicVolumeSlider.value); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { HandleSFXVolumeInputData(sfxVolumeSlider.value); });

        resetButton.onClick.AddListener(() => HandleResetButton());
        
        confirmResetButton.onClick.AddListener(() => HandleConfirmResetButton());
        cancelResetButton.onClick.AddListener(() => HandleCancelButton());

        masterVolumeSlider.value = SaveManager.instance.GetAudioSettings(AUDIO_CHANNEL.MASTER);
        musicVolumeSlider.value = SaveManager.instance.GetAudioSettings(AUDIO_CHANNEL.MUSIC);
        sfxVolumeSlider.value = SaveManager.instance.GetAudioSettings(AUDIO_CHANNEL.SFX);

        noonButton.onClick.AddListener(() => HandleTimeOfDayButton());
        nightButton.onClick.AddListener(() => HandleTimeOfDayButton());

        if (SaveManager.instance.TimeOfDay == TIME_OF_DAY.NOON)
        {
            noonButton.gameObject.SetActive(true);
            nightButton.gameObject.SetActive(false);
        }
        if (SaveManager.instance.TimeOfDay == TIME_OF_DAY.NIGHT)
        {
            nightButton.gameObject.SetActive(true);
            noonButton.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            GlobalUIManager.instance.OnPauseResume();
        }
        */
    }

    public void HandleMasterVolumeInputData(float volume)
    {
        SaveManager.instance.UpdateAudioSettings(AUDIO_CHANNEL.MASTER, volume);
    }

    public void HandleMusicVolumeInputData(float volume)
    {
        SaveManager.instance.UpdateAudioSettings(AUDIO_CHANNEL.MUSIC, volume);
    }

    public void HandleSFXVolumeInputData(float volume)
    {
        SaveManager.instance.UpdateAudioSettings(AUDIO_CHANNEL.SFX, volume);

        // Pour Get la value du channel updater partout dans le code
        //SaveManager.instance.GetAudioSettings(AUDIO_CHANNEL.SFX);
    }

    private void HandleResetButton()
    {
        mainPanel.SetActive(false);
        resetPanel.SetActive(true);
        GlobalUIManager.instance.SetControllerFirstSelected(cancelResetButton.gameObject);
    }

    private void HandleCancelButton()
    {
        resetPanel.SetActive(false);
        mainPanel.SetActive(true);
        GlobalUIManager.instance.SetControllerFirstSelected(exitButton.gameObject);
    }

    private void HandleConfirmResetButton()
    {
        GlobalUIManager.instance.SetControllerFirstSelected(exitButton.gameObject);
        SaveManager.instance.ResetBestScores();
        resetPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    private void HandleTimeOfDayButton()
    {
        TIME_OF_DAY currentTOD = SaveManager.instance.TimeOfDay;

        if (currentTOD == TIME_OF_DAY.NOON)
        {
            noonButton.gameObject.SetActive(false);
            nightButton.gameObject.SetActive(true);
            SaveManager.instance.TimeOfDay = TIME_OF_DAY.NIGHT;
        }
        if (currentTOD == TIME_OF_DAY.NIGHT)
        {
            nightButton.gameObject.SetActive(false);
            noonButton.gameObject.SetActive(true);
            SaveManager.instance.TimeOfDay = TIME_OF_DAY.NOON;
        }

        CameraManager.instance.SetTimeOfDay(SaveManager.instance.TimeOfDay);
    }
}
