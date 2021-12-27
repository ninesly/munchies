using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip defaultMusic;

    AudioSource audioSource;
    public static MusicPlayer instance;


    private void Awake()
    {
        Singleton();
    }

    private void Singleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefsController.GetMasterVolume();
    }


    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void SetDefualtMusic()
    {
        audioSource.clip = defaultMusic;
        audioSource.Play();
        audioSource.mute = false;
        audioSource.loop = true;
    }
}
