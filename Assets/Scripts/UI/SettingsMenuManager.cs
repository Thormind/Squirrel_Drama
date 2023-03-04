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

    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;

    [SerializeField] private GameObject firstSlected;

    // Start is called before the first frame update
    void Start()
    {
        GlobalUIManager.instance.es.SetSelectedGameObject(exitButton.gameObject);

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
        masterVolume = volume;
        SaveManager.instance.UpdateAudioSettings(AUDIO_CHANNEL.MASTER, volume);
    }

    public void HandleMusicVolumeInputData(float volume)
    {
        musicVolume = volume;
        SaveManager.instance.UpdateAudioSettings(AUDIO_CHANNEL.MUSIC, volume);
    }

    public void HandleSFXVolumeInputData(float volume)
    {
        sfxVolume = volume;
        SaveManager.instance.UpdateAudioSettings(AUDIO_CHANNEL.SFX, volume);
    }

    private void HandleResetButton()
    {
        mainPanel.SetActive(false);
        resetPanel.SetActive(true);
        GlobalUIManager.instance.es.SetSelectedGameObject(cancelResetButton.gameObject);
    }

    private void HandleCancelButton()
    {
        resetPanel.SetActive(false);
        mainPanel.SetActive(true);
        GlobalUIManager.instance.es.SetSelectedGameObject(exitButton.gameObject);
    }

    private void HandleConfirmResetButton()
    {
        GlobalUIManager.instance.es.SetSelectedGameObject(exitButton.gameObject);
        SaveManager.instance.ResetBestScores();
        resetPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

}
