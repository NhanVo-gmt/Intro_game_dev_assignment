using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    [SerializeField] private AudioClip moveSound;
    
    private Animator anim;
    private AudioSource audioSource;
    private Tweener tweener;

    private KeyCode lastInputKey = KeyCode.S;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        tweener = GetComponent<Tweener>();
    }

    private void Update()
    {
        GetInput();
        Move();
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            lastInputKey = KeyCode.A;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            lastInputKey = KeyCode.D;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            lastInputKey = KeyCode.W;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            lastInputKey = KeyCode.S;
        }
    }

    private void Move()
    {
        switch (lastInputKey)
        {
            case KeyCode.A:
                Move(Vector2.left);
                break;
            case KeyCode.D:
                Move(Vector2.right);
                break;
            case KeyCode.S:
                Move(Vector2.down);
                break;
            case KeyCode.W:
                Move(Vector2.up);
                break;
            default:
                break;
        }
    }

    private void Move(Vector2 moveTo)
    {
        if (tweener.AddTween(new Tween(transform, moveTo, Time.time, .5f)))
        {
            anim.SetFloat("Horizontal", moveTo.x);
            anim.SetFloat("Vertical", moveTo.y);
        }
        
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
