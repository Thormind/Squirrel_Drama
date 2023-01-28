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

    public float masterVolume = 0.8f;
    public float musicVolume = 0.5f;
    public float sfxVolume = 1f;

    // Start is called before the first frame update
    void Start()
    {
        exitButton.onClick.AddListener(() => GlobalUIManager.instance.SetLastMenu());
        //resetButton.onClick.AddListener(() => SaveManager.ResetData());

        masterVolumeSlider.onValueChanged.AddListener(delegate { HandleMasterVolumeInputData(masterVolumeSlider.value); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { HandleMusicVolumeInputData(musicVolumeSlider.value); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { HandleSFXVolumeInputData(sfxVolumeSlider.value); });
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
}
