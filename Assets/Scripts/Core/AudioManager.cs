using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


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
    public static AudioManager instance;

    public AudioSource uiMusic;
    public AudioSource infiniteMusic;
    public AudioSource legacyMusic;

    public GameObject soundplayer;
    public SoundAudioClip[] soundAudioClipArray;
    public Dictionary<SOUND, AudioClip> soundDictionary = new Dictionary<SOUND, AudioClip>();

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

    [System.Serializable]
    public class SoundAudioClip
    {
        public SOUND sound;
        public AudioClip clip;
    }

    public UnityEngine.Audio.AudioMixerGroup sfx;
    public UnityEngine.Audio.AudioMixerGroup music;
    public AudioMixer mixer;

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
        AdjustMusic();
        AdjustSfx();
        FillSoundDictionary();
        uiMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown("space"))
        //{
        //    PlaySound(SOUND.MOUSEOVER);
        //}
    }

    public void AdjustMusic()
    {
        float volume = SaveManager.instance.GetAudioSettings(AUDIO_CHANNEL.MUSIC);
        if (volume > 0)
        {
            volume = 1 - (1 / (volume*volume));
        }
        else
        {
            volume = -80;
        }
        mixer.SetFloat("musicVol", volume);
    }
    public void AdjustSfx()
    {
        float volume = SaveManager.instance.GetAudioSettings(AUDIO_CHANNEL.SFX);
        if (volume > 0)
        {
            volume = 1 - (1 / (volume * volume));
        }
        else
        {
            volume = -80;
        }
        mixer.SetFloat("sfxVol", volume);
    }
    public void AdjustMaster()
    {
        float volume = SaveManager.instance.GetAudioSettings(AUDIO_CHANNEL.MASTER);
        if (volume > 0)
        {
            volume = 1 - (1 / (volume * volume));
        }
        else
        {
            volume = -80;
        }
        mixer.SetFloat("masterVol", volume);
    }

    // call exemple: AudioManager.instance.PlaySound(SOUND.SQUIRREL_PANIC);
    public void PlaySound(SOUND sound)
    {
        AudioSource audioSource = soundplayer.GetComponent<AudioSource>();
        if (!audioSource.isPlaying)
        {
            audioSource.clip = soundDictionary[sound];
            audioSource.Play();
        }

        else
        {
            AudioSource[] sources = soundplayer.GetComponents<AudioSource>();
            foreach (AudioSource source in sources)
            {
                if (!source.isPlaying)
                {
                    source.clip = soundDictionary[sound];   
                    source.Play();
                    return;
                }
            }
            AudioSource newSource = soundplayer.AddComponent<AudioSource>();
            newSource.outputAudioMixerGroup = sfx;
            newSource.loop = false;
            newSource.playOnAwake = false;
            newSource.clip = soundDictionary[sound];
            newSource.volume = audioSource.volume;
            newSource.Play();
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


}
