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

    private AudioSource audioSource;

    public bool sound = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // sound check
        if (PlayerPrefs.GetInt("SoundToggle") == 1)
            sound = true;
        else
            sound = false;
    }

    public void PlaySoundFX(AudioClip clip, float volume)
    {
        if(sound)
            audioSource.PlayOneShot(clip, volume);
    }
}
