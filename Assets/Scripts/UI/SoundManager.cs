using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public AudioSource BgAudioSource;
    public AudioSource audioSource;

    public bool sound = true;

    void Start()
    {
        // sound check
        if (PlayerPrefs.GetInt("SoundToggle") == 1)
            sound = true;
        else
            sound = false;

        if (!sound)
        {
            ToggleSound(false);
        }
    }

    public void ToggleSound(bool toggle)
    {
        if (toggle)
        {
            audioSource.volume = 1.0f;
            BgAudioSource.volume = 1.0f;
        }
        else
        {
            audioSource.volume = 0f;
            BgAudioSource.volume = 0f;
        }
    }

    public void PlaySoundFX(AudioClip clip, float volume)
    {
        if(sound)
            audioSource.PlayOneShot(clip, volume);
    }
}
