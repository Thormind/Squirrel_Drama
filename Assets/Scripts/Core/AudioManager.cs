using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AUDIO_CHANNEL
{
    MASTER,
    MUSIC,
    SFX
};


public class AudioManager : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        uiMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
