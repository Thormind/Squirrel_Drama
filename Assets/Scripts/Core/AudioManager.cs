using System;
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

public enum MUSIC
{
    MENU_1,
    MENU_2,
    INFINITE_1,
    INFINITE_2,
    LEGACY_1,
    LEGACY_2,
    RANDOM,
    NONE
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
    SQUIRREL_HAPPY,             // Implanted: false
    SQUIRREL_SAD,               // Implanted: false
    BEAR_ROAR,                  // Implanted: true
    BEAR_HIT,                   // Implanted: true
    WORM_BLINK,                 // Implanted: false
    ELEVATOR_MOVEMENT_INFINITE, // Implanted: false
    BUNNY_SFX,                  // Implanted: false
    OBSTACLE_SPAWN,             // Implanted: false Note: correct pitch
    SCORE_BONUS_1,
    SCORE_BONUS_2,
    POINT_SPAWN,
    LIFE_SPAWN,
    FRUIT_HOLE,
    FAIL,
    SQUIRREL_KISS,
    // Legacy mode
    FAIL_HOLE,
    GOOD_HOLE,
};

//CLASS AUDIOMANAGER

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    // ===== VARIABLES DECLARATIONS ===== //

    public AudioSource intro;
    public AudioSource menuAudioSource;
    public AudioSource infiniteAudioSource;
    public AudioSource legacyMusicAudioSource;
    public AudioSource gameOverMusic;
    public AudioSource wind;
    public AudioSource buzz;

    [SerializeField] public AudioListener cameraListener;
    public AudioListener gameListener;

    public GameObject soundplayer;
    public AudioItem[] AudioItemArray;
    public Dictionary<SOUND, SoundAudioClip> soundDictionary = new Dictionary<SOUND, SoundAudioClip>();
    public MusicItem[] MusicItemArray;
    public Dictionary<MUSIC, SoundAudioClip> musicDictionary = new Dictionary<MUSIC, SoundAudioClip>();


    public delegate void GameStateChangedEventHandler(GAME_STATE newGameState);

    [System.Serializable]
    public class AudioItem
    {
        public SOUND sound;
        public SoundAudioClip sound_audio_clip;
    }

    [System.Serializable]
    public class MusicItem
    {
        public MUSIC music;
        public SoundAudioClip music_audio_clip;
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
        ScenesManager.OnGameStateChanged += HandleGameStateChanged;

        AdjustMaster();
        AdjustMusic();
        AdjustSfx();
        FillSoundDictionary();
        FillMusicDictionary();
        intro.Play();
    }

    private void OnDestroy()
    {
        ScenesManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GAME_STATE newGameState)
    {
        switch (newGameState)
        {
            case GAME_STATE.PRE_GAME:
                Resume();
                StopCurrentMusic();
                if (ScenesManager.gameMode == GAME_MODE.INFINITE_MODE)
                {
                    PlayWind();
                }
                if (ScenesManager.gameMode == GAME_MODE.LEGACY_MODE)
                {
                    PlayBuzz();
                }
                break;
            case GAME_STATE.PAUSED:
                Pause();
                break;
            case GAME_STATE.PREPARING:
                PauseMusic();
                AdjustMusic();
                if (ScenesManager.gameMode == GAME_MODE.INFINITE_MODE)
                {
                    PlayInfiniteMusic();
                }
                if (ScenesManager.gameMode == GAME_MODE.LEGACY_MODE)
                {
                    PlayLegacyMusic();
                }
                break;
            case GAME_STATE.ACTIVE:
                Resume();
                if (ScenesManager.gameMode == GAME_MODE.INFINITE_MODE)
                {
                    PlayInfiniteMusic();
                }
                if (ScenesManager.gameMode == GAME_MODE.LEGACY_MODE)
                {
                    PlayLegacyMusic();
                }
                break;
            case GAME_STATE.INACTIVE:
                AdjustMusic();
                StopWind();
                StopBuzz();
                if (!intro.isPlaying)
                    PlayMenuMusic();
                break;
            case GAME_STATE.LOADING:
                PlaySound(SOUND.SWEEP);
                break;
            case GAME_STATE.GAME_OVER:
                if(ScenesManager.gameMode == GAME_MODE.INFINITE_MODE)
                {
                    PlayGameOverMusic();
                    AudioManager.instance.PlaySound(SOUND.FAIL);
                    PlayWind();
                }
                if (ScenesManager.gameMode == GAME_MODE.LEGACY_MODE)
                {
                    PlayGameOverMusic();
                    AudioManager.instance.PlaySound(SOUND.FAIL);
                    PlayBuzz();
                }
                break;
            case GAME_STATE.LEVEL_COMPLETED:
                break;
            case GAME_STATE.GAME_COMPLETED:
                break;
        }
    }


    // ===== METHODS ===== //
    public string GetMusicName(GAME_MODE gameMode)
    {
        MUSIC music = SaveManager.GetMusicSettings(gameMode);
        int musicIndex = (int)music;

        string musicName = $"{Enum.GetName(typeof(MUSIC), musicIndex)}".Replace('_', ' ');
        return musicName;
    }

    public string SwitchMusic(GAME_MODE gameMode, bool selectNext)
    {
        MUSIC music = SaveManager.GetMusicSettings(gameMode);
        int musicIndex = (int)music;

        if (selectNext)
        {
            musicIndex++;
            if (musicIndex >= Enum.GetValues(typeof(MUSIC)).Length)
            {
                musicIndex = 0;
            }
        }
        else
        {
            musicIndex--;
            if (musicIndex < 0)
            {
                musicIndex = Enum.GetValues(typeof(MUSIC)).Length - 1;
            }
        }

        SaveManager.UpdateMusicSettings(gameMode, (MUSIC)musicIndex);

        string musicName = $"{Enum.GetName(typeof(MUSIC), musicIndex)}".Replace('_', ' ');
        return musicName;
    }

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

    private void FillMusicDictionary()
    {
        foreach (MusicItem item in MusicItemArray)
        {
            musicDictionary.Add(item.music, item.music_audio_clip);
        }
    }

    public void AdjustMusic()
    {
        float volume = SaveManager.instance.GetAudioSettings(AUDIO_CHANNEL.MUSIC);
        float dB;
        if (volume != 0)
            dB = (20.0f * Mathf.Log10(volume));
        else
            dB = -144.0f;
        if (ScenesManager.gameState == GAME_STATE.PAUSED)
        {
            dB -= 15;
        }
        mixer.SetFloat("musicVol", dB);
    }

    public void PauseMusic()
    {
        float volume = SaveManager.instance.GetAudioSettings(AUDIO_CHANNEL.MUSIC);
        float dB;
        if (volume != 0)
            dB = (20.0f * Mathf.Log10(volume));
        else
            dB = -144.0f;
        if(ScenesManager.gameMode == GAME_MODE.LEGACY_MODE)
        {
            dB = -144.0f;
        }
        mixer.SetFloat("musicVol", dB - 10);
    }

    public void AdjustSfx()
    {
        float volume = SaveManager.instance.GetAudioSettings(AUDIO_CHANNEL.SFX);
        float dB;
        if (volume != 0)
            dB = (20.0f * Mathf.Log10(volume));
        else
            dB = -144.0f;
        mixer.SetFloat("sfxVol", dB);
    }
    public void AdjustMaster()
    {
        float volume = SaveManager.instance.GetAudioSettings(AUDIO_CHANNEL.MASTER);
        float dB;
        if (volume != 0)
            dB = (20.0f * Mathf.Log10(volume));
        else
            dB = -144.0f;
        mixer.SetFloat("masterVol", dB);
    }

    // call exemple: AudioManager.instance.PlaySound(SOUND.SQUIRREL_PANIC);
    public void PlaySoundAllowed(SOUND sound, ulong time)
    {
        AudioSource audioSource = soundplayer.GetComponent<AudioSource>();
        if (!audioSource.isPlaying)
        {
            audioSource.clip = soundDictionary[sound].clip;
            audioSource.volume = soundDictionary[sound].sound_vol;
            audioSource.Play(time);
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
                    source.Play(time);
                    return;
                }
            }
            AudioSource newSource = soundplayer.AddComponent<AudioSource>();
            newSource.outputAudioMixerGroup = sfx;
            newSource.loop = false;
            newSource.playOnAwake = false;
            newSource.clip = soundDictionary[sound].clip;
            newSource.volume = soundDictionary[sound].sound_vol;
            newSource.Play(time);
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
        AdjustMusic();

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
        AdjustMaster();
        AdjustSfx();
        //Debug.Log(ScenesManager.previousGameState);
        if (ScenesManager.previousGameState != GAME_STATE.PREPARING && ScenesManager.previousGameState != GAME_STATE.GAME_OVER)
        {
            AudioSource[] sources = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            AudioSource[] music_sources = gameObject.GetComponents<AudioSource>();
            foreach (AudioSource source in sources)
            {
                bool ismusic = false;

                foreach (AudioSource music_source in music_sources)
                {
                    if (source == music_source || source.clip == soundDictionary[SOUND.SWEEP].clip)
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
        
    }


    public void PlaySound(SOUND sound, ulong time = 0)
    {
        if (AudioManager.instance != null)
        {
            PlaySoundAllowed(sound, time);
        }
    }

    public void StopCurrentMusic()
    {
        intro.Stop();
        menuAudioSource.Stop();
        legacyMusicAudioSource.Stop();
        infiniteAudioSource.Stop();
        gameOverMusic.Stop();
    }

    public void PlayIntro()
    {
        StopCurrentMusic();
        intro.Play();
    }

    public void PlayMenuMusic()
    {
        if (!menuAudioSource.isPlaying)
        {
            StopCurrentMusic();
            MUSIC menuMusicChoice = MUSIC.MENU_2;

            if (menuMusicChoice == MUSIC.RANDOM)
            {
                menuAudioSource.clip = GetRandomMusicClip().clip;
                menuAudioSource.volume = GetRandomMusicClip().sound_vol;
            }
            if (menuMusicChoice == MUSIC.NONE)
            {
                menuAudioSource.clip = null;
            }
            else
            {
                menuAudioSource.clip = musicDictionary[menuMusicChoice].clip;
                menuAudioSource.volume = musicDictionary[menuMusicChoice].sound_vol;
            }

            menuAudioSource.Play();
        }
    }

    public void PlayLegacyMusic()
    {
        if (!legacyMusicAudioSource.isPlaying)
        {
            StopCurrentMusic();
            MUSIC legacyMusicChoice = SaveManager.GetMusicSettings(GAME_MODE.LEGACY_MODE);

            if (legacyMusicChoice == MUSIC.RANDOM)
            {
                legacyMusicAudioSource.clip = GetRandomMusicClip().clip;
                legacyMusicAudioSource.volume = GetRandomMusicClip().sound_vol;
            }
            if (legacyMusicChoice == MUSIC.NONE)
            {
                legacyMusicAudioSource.clip = null;
            }
            if (legacyMusicChoice != MUSIC.RANDOM && legacyMusicChoice != MUSIC.NONE)
            {
                legacyMusicAudioSource.clip = musicDictionary[legacyMusicChoice].clip;
                legacyMusicAudioSource.volume = musicDictionary[legacyMusicChoice].sound_vol;
            }

            legacyMusicAudioSource.Play();
        }
    }

    public void PlayInfiniteMusic()
    {
        if (!infiniteAudioSource.isPlaying)
        {
            StopCurrentMusic();
            MUSIC infiniteMusicChoice = SaveManager.GetMusicSettings(GAME_MODE.INFINITE_MODE);

            if (infiniteMusicChoice == MUSIC.RANDOM)
            {
                infiniteAudioSource.clip = GetRandomMusicClip().clip;
                infiniteAudioSource.volume = GetRandomMusicClip().sound_vol;
            }
            if (infiniteMusicChoice == MUSIC.NONE)
            {
                infiniteAudioSource.clip = null;
            }
            if (infiniteMusicChoice != MUSIC.RANDOM && infiniteMusicChoice != MUSIC.NONE)
            {
                infiniteAudioSource.clip = musicDictionary[infiniteMusicChoice].clip;
                infiniteAudioSource.volume = musicDictionary[infiniteMusicChoice].sound_vol;
            }

            infiniteAudioSource.Play();
        }
    }

    public void PlayGameOverMusic()
    {
        if (!gameOverMusic.isPlaying)
        {
            StopCurrentMusic();
            gameOverMusic.Play();
        }
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

    public void PlayBuzz()
    {
        if (!buzz.isPlaying)
        {
            buzz.Play();
        }
    }

    public void StopBuzz()
    {
        if (buzz.isPlaying)
        {
            buzz.Stop();
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

    public SoundAudioClip GetRandomMusicClip()
    {
        MusicItem randomMusicItem = MusicItemArray[UnityEngine.Random.Range(0, MusicItemArray.Length)];
        return randomMusicItem.music_audio_clip;
    }
}
