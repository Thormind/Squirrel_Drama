using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField] private Button timeOfDayButton;
    [SerializeField] private TextMeshProUGUI timeOfDayText;
    [SerializeField] private Sprite noonImage;
    [SerializeField] private Sprite nightImage;

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
        exitButton.onClick.AddListener(() => GlobalUIManager.instance.SetLastMenu());

        masterVolumeSlider.value = SaveManager.instance.GetAudioSettings(AUDIO_CHANNEL.MASTER);
        musicVolumeSlider.value = SaveManager.instance.GetAudioSettings(AUDIO_CHANNEL.MUSIC);
        sfxVolumeSlider.value = SaveManager.instance.GetAudioSettings(AUDIO_CHANNEL.SFX);

        masterVolumeSlider.onValueChanged.AddListener(delegate { HandleMasterVolumeInputData(masterVolumeSlider.value); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { HandleMusicVolumeInputData(musicVolumeSlider.value); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { HandleSFXVolumeInputData(sfxVolumeSlider.value); });

        confirmResetButton.onClick.AddListener(() => HandleConfirmResetButton());
        cancelResetButton.onClick.AddListener(() => HandleCancelButton());
        resetButton.onClick.AddListener(() => HandleResetButton());

        timeOfDayButton.onClick.AddListener(() => HandleTimeOfDayButton());
    }

    private void OnEnable()
    {
        resetPanel.SetActive(false);
        mainPanel.SetActive(true);

        SetTimeOfDayButton();

        GlobalUIManager.instance.specificMenu = MENU.MENU_MAIN;
        GlobalUIManager.instance.SetFirstSelected(exitButton.gameObject);
    }

    public void HandleMasterVolumeInputData(float volume)
    {
        SaveManager.instance.UpdateAudioSettings(AUDIO_CHANNEL.MASTER, volume);
        AudioManager.instance.AdjustMaster();
    }

    public void HandleMusicVolumeInputData(float volume)
    {
        SaveManager.instance.UpdateAudioSettings(AUDIO_CHANNEL.MUSIC, volume);
        AudioManager.instance.AdjustMusic();

    }

    public void HandleSFXVolumeInputData(float volume)
    {
        SaveManager.instance.UpdateAudioSettings(AUDIO_CHANNEL.SFX, volume);
        AudioManager.instance.AdjustSfx();
    }

    private void HandleResetButton()
    {
        mainPanel.SetActive(false);
        resetPanel.SetActive(true);
        GlobalUIManager.instance.SetFirstSelected(cancelResetButton.gameObject, true);
    }

    private void HandleCancelButton()
    {
        resetPanel.SetActive(false);
        mainPanel.SetActive(true);
        GlobalUIManager.instance.SetFirstSelected(exitButton.gameObject, true);
    }

    private void HandleConfirmResetButton()
    {
        GlobalUIManager.instance.SetFirstSelected(exitButton.gameObject, true);
        SaveManager.instance.ResetBestScores();
        SaveManager.SetDefaultMusicSettings();
        resetPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    private void HandleTimeOfDayButton()
    {
        TIME_OF_DAY currentTOD = SaveManager.instance.TimeOfDay;

        if (currentTOD == TIME_OF_DAY.NOON)
        {
            SaveManager.instance.TimeOfDay = TIME_OF_DAY.NIGHT;
        }
        if (currentTOD == TIME_OF_DAY.NIGHT)
        {
            SaveManager.instance.TimeOfDay = TIME_OF_DAY.NOON;
        }

        SetTimeOfDayButton();
        CameraManager.instance.SetTimeOfDay(SaveManager.instance.TimeOfDay);
    }

    private void SetTimeOfDayButton()
    {
        Image timeOfDayImage = timeOfDayButton.GetComponent<Image>();

        if (SaveManager.instance.TimeOfDay == TIME_OF_DAY.NOON)
        {
            timeOfDayText.text = "NOON";
            timeOfDayImage.sprite = noonImage;
        }
        if (SaveManager.instance.TimeOfDay == TIME_OF_DAY.NIGHT)
        {
            timeOfDayText.text = "NIGHT";
            timeOfDayImage.sprite = nightImage;
        }
    }
}
