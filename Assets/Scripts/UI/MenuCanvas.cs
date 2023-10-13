using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvas : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip normalStateGhostMusic;

    public void Play()
    {
        audioSource.clip = backgroundMusic;
        audioSource.Play();
        
        Invoke("PlayGhostMusic", backgroundMusic.length);
    }

    public void PlayGhostMusic()
    {
        audioSource.clip = normalStateGhostMusic;
        audioSource.Play();
    }
}
