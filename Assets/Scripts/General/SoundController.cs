using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public bool activateOnStart = true;
    [Header("Interface")]
    public AudioClip buttonSound;
    [Range(0f, 1f)] public float buttonVolume = 0.5f;

    [Header("InGame Events")]
    public AudioClip spawnSound;
    [Range(0f, 1f)] public float spawnVolume = 0.5f;
    public AudioClip deathSound;
    [Range(0f, 1f)] public float deathVolume = 0.5f;
    [Range(0f, 1f)] public float smunchieVolume = 0.5f;
    public AudioClip moatherSound;
    public AudioClip dictoSound;
    public AudioClip seeerSound;
    public AudioClip sacriSound;
    public AudioClip whipySound;

    [Header("OutGame Events")]
    public AudioClip startLevelSound;
    [Range(0f, 1f)] public float startLevelVolume = 0.6f;
    public AudioClip goalPointSound;
    [Range(0f, 1f)] public float goalPointVolume = 0.5f;
    public AudioClip winSound;
    [Range(0f, 1f)] public float winVolume = 0.5f;
    public AudioClip loseSound;
    [Range(0f, 1f)] public float loseVolume = 0.5f;

    bool oneStart = true;
    bool oneWin = true;
    bool oneLose = true;

    float soundVolumeMod = 1f;

    private void Awake()
    {
        if (activateOnStart) StartSound();      
    }

    private void Start()
    {
        soundVolumeMod = PlayerPrefsController.GetSoundVolume();
    }

    public void ButtonSound()
    {
        AudioSource.PlayClipAtPoint(buttonSound, Camera.main.transform.position, buttonVolume * soundVolumeMod);
    }
    public void MunchieSpawnSound()
    {
        var volume = spawnVolume * soundVolumeMod;
        Debug.Log("volume = " + volume);
        AudioSource.PlayClipAtPoint(spawnSound, Camera.main.transform.position, spawnVolume * soundVolumeMod);
    }
    public void SmunchieSpawnSound(Smunchie.SmunchieClass s_class)
    {

        var volume = smunchieVolume * soundVolumeMod;

        if (s_class == Smunchie.SmunchieClass.Moather)
        {
            AudioSource.PlayClipAtPoint(moatherSound, Camera.main.transform.position, volume);
        }
        else if (s_class == Smunchie.SmunchieClass.Dicto)
        {
            AudioSource.PlayClipAtPoint(dictoSound, Camera.main.transform.position, volume);
        }
        else if (s_class == Smunchie.SmunchieClass.Sacri)
        {
            AudioSource.PlayClipAtPoint(sacriSound, Camera.main.transform.position, volume);
        }
        else if (s_class == Smunchie.SmunchieClass.Seeer)
        {
            AudioSource.PlayClipAtPoint(seeerSound, Camera.main.transform.position, volume);
        }
        else if (s_class == Smunchie.SmunchieClass.Whipy)
        {
            AudioSource.PlayClipAtPoint(whipySound, Camera.main.transform.position, volume);
        }
    }
    public void DeathSound()
    {
        AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathVolume * soundVolumeMod);
    }
    public void GoalPointSound()
    {
        AudioSource.PlayClipAtPoint(goalPointSound, Camera.main.transform.position, goalPointVolume * soundVolumeMod);
    }
    public void WinSound()
    {
        if (!oneWin) return;
        AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position, winVolume * soundVolumeMod);
        oneWin = false;
    }
    public void LoseSound()
    {
        Debug.Log("lost before return;");
        if (!oneLose) return;
        Debug.Log("lost sound after return;");
        AudioSource.PlayClipAtPoint(loseSound, Camera.main.transform.position, loseVolume * soundVolumeMod);
        oneLose = false;
    }
    public void StartSound()
    {
        if (!oneStart) return;
        AudioSource.PlayClipAtPoint(startLevelSound, Camera.main.transform.position, startLevelVolume * soundVolumeMod);
        oneStart = false;
    }

    public void SetDefaultMusic()
    {
        var musicPlayer = FindObjectOfType<MusicPlayer>();
        if (!musicPlayer) return;
        musicPlayer.SetDefualtMusic();
    }
}
