using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private AudioClip moveSound;
    
    private Animator anim;
    private AudioSource audioSource;
    private Tweener tweener;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        tweener = GetComponent<Tweener>();
    }

    private void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2.left, -1, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2.right, 1, 0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(Vector2.up, 0, 1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2.down, 0, -1);
        }
    }

    private void Move(Vector2 moveTo, int horizontal, int vertical)
    {
        tweener.AddTween(new Tween(transform, moveTo, Time.time, .5f));
        anim.SetFloat("Horizontal", horizontal);
        anim.SetFloat("Vertical", vertical);
        
        audioSource.PlayOneShot(moveSound);
    }
}
