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
    POINT_GRAB,                 // Implanted: true Note: correct volume
    LIFE_SPIN,                  // Implanted: true
    LIFE_POP,                   // Implanted: true
    SQUIRREL_PANIC,             // Implanted: false
    SQUIRREL_CAUTION,           // Implanted: false
    BEAR_ROAR,                  // Implanted: true
    BEAR_HIT,                   // Implanted: true
    WORM_BLINK,                 // Implanted: false
    ELEVATOR_MOVEMENT_INFINITE, // Implanted: false
    BUNNY_SFX,                  // Implanted: false
    OBSTACLE_SPAWN,             // Implanted: false Note: correct pitch
    SCORE_BONUS,
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
    public AudioSource wind;

    [SerializeField] public AudioListener cameraListener;
    public AudioListener gameListener;

    public GameObject soundplayer;
    public AudioItem[] AudioItemArray;
    public Dictionary<SOUND, SoundAudioClip> soundDictionary = new Dictionary<SOUND, SoundAudioClip>();

    [System.Serializable]
    public class AudioItem
    {
        public SOUND sound;
        public SoundAudioClip sound_audio_clip;
    }

    [System.Serializable]
    public class SoundAudioClip
    {
        public AudioClip clip;
        public float sound_vol;
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
        AdjustMaster();
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
                    gameListener.enabled = true;
                }
                break;
            case GAME_MODE.LEGACY_MODE:
                if (cameraListener != null)
                {
                    cameraListener.enabled = false;
                }
                if (gameListener != null)
                {
                    gameListener.enabled = true;
                }
                break;
        }

    }
    private void FillSoundDictionary()
    {
        foreach (AudioItem item in AudioItemArray)
        {
            soundDictionary.Add(item.sound, item.sound_audio_clip);
        }
    }

    public void AdjustMusic()
    {
        float volume = SaveManager.instance.GetAudioSettings(AUDIO_CHANNEL.MUSIC);
        if (volume > 0)
        {
            volume = 1 - (1 / (volume * volume));
        }
        else
        {
            volume = -80;
        }
        mixer.SetFloat("musicVol", volume);
    }

    public void PauseMusic()
    {
        float volume = SaveManager.instance.GetAudioSettings(AUDIO_CHANNEL.MUSIC);
        if (volume > 0)
        {
            volume = -10 - (1 / (volume * volume));
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
            audioSource.clip = soundDictionary[sound].clip;
            audioSource.volume = soundDictionary[sound].sound_vol;
            audioSource.Play();
        }

        else
        {
            AudioSource[] sources = soundplayer.GetComponents<AudioSource>();
            foreach (AudioSource source in sources)
            {
                if (!source.isPlaying)
                {
                    source.clip = soundDictionary[sound].clip;
                    source.volume = soundDictionary[sound].sound_vol;
                    source.Play();
                    return;
                }
            }
            AudioSource newSource = soundplayer.AddComponent<AudioSource>();
            newSource.outputAudioMixerGroup = sfx;
            newSource.loop = false;
            newSource.playOnAwake = false;
            newSource.clip = soundDictionary[sound].clip;
            newSource.volume = soundDictionary[sound].sound_vol;
            newSource.Play();
        }
    }

    public void HandleElevatorSFX(AudioSource elev_sound, Vector2 input)
    {
        float total_intensity = Mathf.Abs(input.x + input.y);

        if (elev_sound.volume < 0.05f + total_intensity / 10)
        {
            elev_sound.volume += 0.01f;
        }
        else
        {
            elev_sound.volume -= 0.02f;
        }
    }

    public void Pause()
    {
        PauseMusic();

        AudioSource[] sources = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        AudioSource[] music_sources = gameObject.GetComponents<AudioSource>();

        foreach (AudioSource source in sources)
        {
            bool ismusic = false;

            foreach(AudioSource music_source in music_sources)
            {
                if(source == music_source)
                {
                    ismusic = true;
                    break;
                }
            }
            if (source.isPlaying && !ismusic)
            {
                source.Pause();
            }
        }
    }

    public void Resume()
    {
        AdjustMusic();
        AudioSource[] sources = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        AudioSource[] music_sources = gameObject.GetComponents<AudioSource>();
        foreach (AudioSource source in sources)
        {
            bool ismusic = false;

            foreach (AudioSource music_source in music_sources)
            {
                if (source == music_source)
                {
                    ismusic = true;
                    break;
                }
            }

            if (source.time != 0 && !ismusic)
            {
                source.Play();
            }
        }
    }


    public void PlaySound(SOUND sound)
    {
        if (AudioManager.instance != null)
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

    public void PlayWind()
    {
        if (!wind.isPlaying)
        {
            wind.Play();
        }
    }

    public void StopWind()
    {
        if (wind.isPlaying)
        {
            wind.Stop();
        }
    }

    public void PlayElevatorSound(AudioSource source, Vector2 input)
    {
        if ((input.x == 0 && input.y == 0) && source.isPlaying)
        {
            source.Stop();
        }
        if ((input.x != 0 || input.y != 0) && !source.isPlaying)
        {
            source.Play();
        }
    }


}
