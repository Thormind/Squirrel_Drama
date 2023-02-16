using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class AudioUIController : MonoBehaviour
{

    public TMP_Text MasterVolumeTxt;
    public Slider MasterVolumeSlider;

    public TMP_Text MusicVolumeTxt;
    public Slider MusicVolumeSlider;

    public TMP_Text SFXVolumeTxt;
    public Slider SFXVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        // Get the current event system
        EventSystem currentEventSystem = EventSystem.current;

        // Disable gamepad input for UI navigation
        currentEventSystem.sendNavigationEvents = false;

        MasterVolumeSlider.onValueChanged.AddListener(delegate { HandleMasterVolumeInputData(MasterVolumeSlider.value); });
        MusicVolumeSlider.onValueChanged.AddListener(delegate { HandleMusicVolumeInputData(MusicVolumeSlider.value); });
        SFXVolumeSlider.onValueChanged.AddListener(delegate { HandleSFXVolumeInputData(SFXVolumeSlider.value); });

        HandleMasterVolumeInputData(MasterVolumeSlider.value);
        HandleMusicVolumeInputData(MusicVolumeSlider.value);
        HandleSFXVolumeInputData(SFXVolumeSlider.value);
    }

    public void HandleMasterVolumeInputData(float volume)
    {
        MasterVolumeTxt.text = volume.ToString();
    }

    public void HandleMusicVolumeInputData(float volume)
    {
        MusicVolumeTxt.text = volume.ToString();
    }

    public void HandleSFXVolumeInputData(float volume)
    {
        SFXVolumeTxt.text = volume.ToString();
    }
}
