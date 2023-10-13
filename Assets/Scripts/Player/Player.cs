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
        // GetInput();
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2.left);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2.right);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(Vector2.up);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2.down);
        }
    }

    private void Move(Vector2 moveTo)
    {
        
        tweener.AddTween(new Tween(transform, moveTo, Time.time, .5f));
        anim.SetFloat("Horizontal", moveTo.x);
        anim.SetFloat("Vertical", moveTo.y);
        
        audioSource.PlayOneShot(moveSound);
    }

    public IEnumerator MoveCoroutine(Vector2 moveTo)
    {
        yield return new WaitUntil(() => tweener.AddTween(new Tween(transform, moveTo, Time.time, .5f)));
        anim.SetFloat("Horizontal", moveTo.x);
        anim.SetFloat("Vertical", moveTo.y);
        
        if (moveSound != null)
            audioSource.PlayOneShot(moveSound);
    }
}
