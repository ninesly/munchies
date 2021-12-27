using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsController : MonoBehaviour
{
    const string MASTER_VOLUME_KEY = "master volume";
    const string DIFFICULTY_KEY = "difficulty";

    const float MIN_VOLUME = 0F;
    const float MAX_VOLUME = 1F;
    const float DEFAULT_VOLUME = 0.13F;

    const float MIN_SOUND_VOLUME = 0.5F;
    const float MAX_SOUND_VOLUME = 1F;
    const float DEFAULT_SOUND_VOLUME = 1F;

    public static void SetMasterVolume(float volume)
    {
        if (volume >= MIN_VOLUME && volume <= MAX_VOLUME)
        {
            Debug.Log("Master volume set to: " + volume);
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
        }
        else
        {
            Debug.LogError("Master volume is out of range");
        }
    }
    public static float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, DEFAULT_VOLUME);
    }

    public static void SetDifficulty(float difLevel)
    {
        if (difLevel >= MIN_SOUND_VOLUME && difLevel <= MAX_SOUND_VOLUME)
        {
            Debug.Log("Difficulty set to: " + difLevel);
            PlayerPrefs.SetFloat(DIFFICULTY_KEY, difLevel);
        }
        else
        {
            Debug.LogError("Difficulty is out of range");
        }
    }
    public static float GetSoundVolume()
    {
        return PlayerPrefs.GetFloat(DIFFICULTY_KEY, DEFAULT_SOUND_VOLUME);
    }
}
