using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private Dictionary<string, int> bestScores = new Dictionary<string, int>();
    private Dictionary<string, float> audioSettings = new Dictionary<string, float>();

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
}
