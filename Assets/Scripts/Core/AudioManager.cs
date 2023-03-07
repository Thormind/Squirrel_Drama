using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AUDIO_CHANNEL
{
    MASTER,
    MUSIC,
    SFX
};

public enum SOUND
{
    // Menus
    MOUSEOVER,                  // Implanted: false
    CLICK,                      // Implanted: false
    SWEEP,                      // Implanted: false
    // Infinite mode
    FRUIT_FALL,                 // Implanted: false
    FRUIT_SQUASH,               // Implanted: false
    FRUIT_INHOLE,               // Implanted: false
    FRUIT_MOVEMENT,             // Implanted: false
    FRUIT_TOUCHBEE,             // Implanted: false
    POINT_GRAB,                 // Implanted: false
    LIFE_GRAB,                  // Implanted: false
    SQUIRREL_PANIC,             // Implanted: false
    SQUIRREL_CAUTION,           // Implanted: false
    BEAR_ACTION,                // Implanted: false
    WORM_BLINK,                 // Implanted: false
    WORM_MOVEMENT,              // Implanted: false
    ELEVATOR_MOVEMENT_INFINITE, // Implanted: false
    BUNNY_SFX                   // Implanted: false
    // Legacy mode
};



public class AudioManager : MonoBehaviour
{

    public GameObject soundplayer;
    public SoundAudioClip[] soundAudioClipArray;
    public Dictionary<SOUND, AudioClip> soundDictionary = new Dictionary<SOUND, AudioClip>();

    [System.Serializable]
    public class SoundAudioClip
    {
        public SOUND sound;
        public AudioClip clip;
    }

    private void FillSoundDictionary()
    {
        foreach (SoundAudioClip soundAudioClip in soundAudioClipArray)
        {
            soundDictionary.Add(soundAudioClip.sound, soundAudioClip.clip);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        FillSoundDictionary();
        uiMusic.Play();
    }

    // call exemple: AudioManager.instance.PlaySound(SOUND.SQUIRREL_PANIC);
    public void PlaySound(SOUND sound)
    {
        soundplayer.GetComponent<AudioSource>().clip = soundDictionary[sound];
        soundplayer.GetComponent<AudioSource>().Play();
    }

    public static AudioManager instance;

    public AudioSource uiMusic;
    public AudioSource infiniteMusic;
    public AudioSource legacyMusic;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }
    public void StopCurrentMusic()
    {
        uiMusic.Stop();
        legacyMusic.Stop();
        infiniteMusic.Stop();
    }
    public void PlayUiMusic()
    {
        StopCurrentMusic();
        uiMusic.Play();
    }
    public void PlayLegacy()
    {
        StopCurrentMusic();
        legacyMusic.Play();
    }

    public void PlayInfinite()
    {
        StopCurrentMusic();
        infiniteMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown("space"))
        //{
        //    PlaySound(SOUND.SWEEP);
        //}
    }
}
