using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private Dictionary<string, int> bestScores = new Dictionary<string, int>();
    private Dictionary<string, float> audioSettings = new Dictionary<string, float>();
    private static Dictionary<GAME_MODE, MUSIC> musicSettings = new Dictionary<GAME_MODE, MUSIC>();
    private TIME_OF_DAY timeOfDay;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        LoadBestScores();
        LoadAudioSettings();
        LoadTimeOfDay();
        LoadMusicSettings();
    }

    // Best Scores Data
    private void LoadBestScores()
    {
        foreach (string gameMode in Enum.GetNames(typeof(GAME_MODE)))
        {
            int bestScore = PlayerPrefs.GetInt(gameMode + "BestScore", 0);
            bestScores[gameMode] = bestScore;
        }
    }

    private void SaveBestScores()
    {
        foreach (KeyValuePair<string, int> entry in bestScores)
        {
            PlayerPrefs.SetInt(entry.Key + "BestScore", entry.Value);
        }
        PlayerPrefs.Save();
    }

    public void UpdateBestScore(GAME_MODE gameMode, int score)
    {
        string gameModeName = gameMode.ToString();
        if (score > bestScores[gameModeName])
        {
            bestScores[gameModeName] = score;
            SaveBestScores();
        }
    }

    public int GetBestScore(GAME_MODE gameMode)
    {
        string gameModeName = gameMode.ToString();
        return bestScores[gameModeName];
    }

    public void ResetBestScores()
    {
        foreach (string gameMode in Enum.GetNames(typeof(GAME_MODE)))
        {
            bestScores[gameMode] = 0;
            PlayerPrefs.DeleteKey(gameMode + "BestScore"); // delete only the required key
        }
        SaveBestScores();
    }

    // Audio Settings Data
    private void LoadAudioSettings()
    {
        foreach (string audioChannel in Enum.GetNames(typeof(AUDIO_CHANNEL)))
        {
            float volume = PlayerPrefs.GetFloat(audioChannel + "Volume", 0.8f);
            audioSettings[audioChannel] = volume;
        }
    }

    private void SaveAudioSettings()
    {
        foreach (KeyValuePair<string, float> entry in audioSettings)
        {
            PlayerPrefs.SetFloat(entry.Key + "Volume", entry.Value);
        }
        PlayerPrefs.Save();
    }

    public void UpdateAudioSettings(AUDIO_CHANNEL audioChannel, float volume)
    {
        string audioChannelName = audioChannel.ToString();
        audioSettings[audioChannelName] = volume;
        SaveAudioSettings();
    }

    public float GetAudioSettings(AUDIO_CHANNEL audioChannel)
    {
        string audioChannelName = audioChannel.ToString();
        return audioSettings[audioChannelName];
    }

    //MUSIC SETTINGS
    public static void SaveMusicSettings()
    {
        foreach (KeyValuePair<GAME_MODE, MUSIC> entry in musicSettings)
        {
            string key = GetMusicKey(entry.Key);
            PlayerPrefs.SetInt(key, (int)entry.Value);
        }
        PlayerPrefs.Save();
    }

    public static void LoadMusicSettings()
    {
        foreach (GAME_MODE mode in Enum.GetValues(typeof(GAME_MODE)))
        {
            string key = GetMusicKey(mode);

            int musicValue = PlayerPrefs.GetInt(key, -1);
            if (musicValue == -1)
            {
                musicValue =  (int) GetDefaultMusic(mode);
            }

            musicSettings[mode] = (MUSIC)musicValue;
        }
    }

    public static void UpdateMusicSettings(GAME_MODE gameMode, MUSIC music)
    {
        musicSettings[gameMode] = music;
        SaveMusicSettings();
    }

    public static MUSIC GetMusicSettings(GAME_MODE mode)
    {
        return musicSettings[mode];
    }

    private static MUSIC GetDefaultMusic(GAME_MODE mode)
    {
        switch (mode)
        {
            case GAME_MODE.NONE:
                return MUSIC.MENU_2;
            case GAME_MODE.INFINITE_MODE:
                return MUSIC.INFINITE_1;
            case GAME_MODE.LEGACY_MODE:
                return MUSIC.LEGACY_1;
            default:
                return MUSIC.RANDOM;
        }
    }

    private static string GetMusicKey(GAME_MODE mode)
    {
        return string.Format("Music_{0}", mode.ToString());
    }

    public static void SetDefaultMusicSettings()
    {
        string key = GetMusicKey(GAME_MODE.NONE);
        PlayerPrefs.SetInt(key, (int)MUSIC.MENU_2);

        key = GetMusicKey(GAME_MODE.INFINITE_MODE);
        PlayerPrefs.SetInt(key, (int)MUSIC.INFINITE_1);

        key = GetMusicKey(GAME_MODE.LEGACY_MODE);
        PlayerPrefs.SetInt(key, (int)MUSIC.LEGACY_1);
    }

    //TIME OF DAY SETTINGS
    public delegate void TimeOfDayChangedEventHandler(TIME_OF_DAY newTimeOfDay);
    public static event TimeOfDayChangedEventHandler OnTimeOfDayChanged;
    public TIME_OF_DAY TimeOfDay
    {
        get { return timeOfDay; }
        set
        {
            timeOfDay = value;
            OnTimeOfDayChanged?.Invoke(timeOfDay);
            SaveTimeOfDay();
        }
    }

    private void SaveTimeOfDay()
    {
        if (timeOfDay == TIME_OF_DAY.NOON)
        {
            PlayerPrefs.SetInt("TimeOfDay_", 0);
        }
        if (TimeOfDay == TIME_OF_DAY.NIGHT)
        {
            PlayerPrefs.SetInt("TimeOfDay_", 1);
        }
    }

    private void LoadTimeOfDay()
    {
        int tod = PlayerPrefs.GetInt("TimeOfDay_", 0);
        if (tod == 0)
        {
            timeOfDay = TIME_OF_DAY.NOON;
        }
        if (tod == 1)
        {
            timeOfDay = TIME_OF_DAY.NIGHT;
        }
        OnTimeOfDayChanged?.Invoke(timeOfDay);

    }
}
