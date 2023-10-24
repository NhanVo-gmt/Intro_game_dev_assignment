using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    [SerializeField] private AudioClip normalGhostStateClip;
    [SerializeField] private AudioClip scaredGhostStateClip;
    [SerializeField] private AudioClip deadGhostStateClip;
    
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
        GameManager.Instance.OnPlayGame += PlayNormalGhostMusic;
    }
    
    public void PlayNormalGhostMusic()
    {
        source.clip = normalGhostStateClip;
        source.Play();
    }

    public void PlayScaredGhostMusic()
    {
        if (source.clip == scaredGhostStateClip) return;
        
        source.clip = scaredGhostStateClip;
        source.Play();
    }
    
    public void PlayDeadGhostMusic()
    {
        if (source.clip == deadGhostStateClip) return;
        
        source.clip = deadGhostStateClip;
        source.Play();
    }
}
