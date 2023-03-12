using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



// ===== STRUCTS & ENUMS ===== //

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
    SWEEP,                      // Implanted: true
    // Infinite mode
    FRUIT_FALL,                 // Implanted: True
    FRUIT_SQUASH,               // Implanted: True
    POINT_GRAB,                 // Implanted: true
    LIFE_SPIN,                  // Implanted: true
    LIFE_POP,                   // Implanted: true
    SQUIRREL_PANIC,             // Implanted: false
    SQUIRREL_CAUTION,           // Implanted: false
    BEAR_ROAR,                  // Implanted: true
    BEAR_HIT,                   // Implanted: true
    WORM_BLINK,                 // Implanted: false
    ELEVATOR_MOVEMENT_INFINITE, // Implanted: false
    BUNNY_SFX,                  // Implanted: false
    OBSTACLE_SPAWN,             // Implanted: false
    // Legacy mode
};

//CLASS AUDIOMANAGER

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // ===== VARIABLES DECLARATIONS ===== //

    public AudioSource uiMusic;
    public AudioSource infiniteMusic;
    public AudioSource legacyMusic;

    [SerializeField] public AudioListener cameraListener;
    public AudioListener gameListener;

    public GameObject soundplayer;
    public SoundAudioClip[] soundAudioClipArray;
    public Dictionary<SOUND, AudioClip> soundDictionary = new Dictionary<SOUND, AudioClip>();

    [System.Serializable]
    public class SoundAudioClip
    {
        public SOUND sound;
        public AudioClip clip;
    }

    public UnityEngine.Audio.AudioMixerGroup sfx;
    public UnityEngine.Audio.AudioMixerGroup music;
    public AudioMixer mixer;


    // ===== AWAKE, START, UPDATE ===== //

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


    // ===== METHODS ===== //
    public void SwitchAudioListener(GAME_MODE gameMode)
    {
        switch (gameMode)
        {
            case GAME_MODE.NONE:
                if (cameraListener != null)
                {
                    cameraListener.enabled = true;
                }
                if (gameListener != null)
                {
                    cameraListener.enabled = false;
                }
                break;
            case GAME_MODE.INFINITE_MODE:
                if (cameraListener != null)
                {
                    cameraListener.enabled = false;
                }
                if (gameListener != null)
                {
                    cameraListener.enabled = true;
                }
                break;
            case GAME_MODE.LEGACY_MODE:
                if (cameraListener != null)
                {
                    cameraListener.enabled = false;
                }
                if (gameListener != null)
                {
                    cameraListener.enabled = true;
                }
                break;
        }

    }
    private void FillSoundDictionary()
    {
        foreach (SoundAudioClip soundAudioClip in soundAudioClipArray)
        {
            soundDictionary.Add(soundAudioClip.sound, soundAudioClip.clip);
        }
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
    public void PlaySoundAllowed(SOUND sound)
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
    
    public void PlaySound(SOUND sound)
    {
        if(AudioManager.instance != null)
        {
            PlaySoundAllowed(sound);
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
