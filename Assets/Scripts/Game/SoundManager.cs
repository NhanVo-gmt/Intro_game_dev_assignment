using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    [SerializeField] private AudioClip normalGhostStateClip;
    [SerializeField] private AudioClip scaredGhostStateClip;
    
    private AudioSource source;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        ChangeToNormalGhostMusic();
    }

    public void ChangeToNormalGhostMusic()
    {
        source.clip = normalGhostStateClip;
        source.Play();
    }

    public void ChangeToScaredGhostMusic()
    {
        source.clip = scaredGhostStateClip;
        source.Play();
    }
}
