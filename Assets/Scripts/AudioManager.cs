using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip audioClip_1;
    public AudioClip audioClip_2;
    public AudioClip audioClip_3;

    public AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayClip(string clipName)
    {
        if (clipName == "GoodTime" && audioClip_2 != null)
        {
            audioSource.PlayOneShot(audioClip_2, 3f);
        }
        else if (clipName == "BadTime" && audioClip_3 != null)
        {
            audioSource.PlayOneShot(audioClip_3, 3f);
        }
        if (clipName == "Music")
        {
            audioSource.clip = audioClip_1;
            audioSource.volume = 0.1f;
            audioSource.Play();
        }
    }
}
