using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PacStudentController : MonoBehaviour
{
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioClip hitWallSound;
    [SerializeField] private ParticleSystem hitWallParticle;
    
    private Animator anim;
    private AudioSource audioSource;
    private Tweener tweener;

    private float tweenerDuration = 0.5f;

    private KeyCode lastInputKey = KeyCode.S;
    private KeyCode currentInputKey = KeyCode.S;
    private float timeSinceLastSet = 0f;

    private Vector2 sizedBoxCheck = new Vector2(.95f, .95f);

    private bool hitWall = false;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
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

    private bool CanMove(Vector2 moveTo)
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll((Vector2)transform.position + moveTo, sizedBoxCheck, 0);
        foreach (Collider2D collider2D in collider2Ds)
        {
            if (collider2D.CompareTag("Wall"))
            {
                return false;
            }
        }

        return true;
    }

    private Vector2 GetMovementVector(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.A:
                return Vector2.left;
            case KeyCode.D:
                return Vector2.right;
            case KeyCode.S:
                return Vector2.down;
            case KeyCode.W:
                return Vector2.up;
            default:
                break;
        }

        return Vector2.zero;
    }

    private void Move()
    {
        Vector2 moveTo = GetMovementVector(currentInputKey);
        if (CanMove(moveTo))
        {
            if (tweener.AddTween(new Tween(transform, moveTo, Time.time, tweenerDuration)))
            {
                anim.SetFloat("Horizontal", moveTo.x);
                anim.SetFloat("Vertical", moveTo.y);
                audioSource.PlayOneShot(moveSound);
            }

            hitWall = false;
        }
        else if (!hitWall)
        {
            hitWall = true;
            StartCoroutine(HitWallCoroutine(moveTo));
        }
        
        if (CanMove(GetMovementVector(lastInputKey)))
        {
            currentInputKey = lastInputKey;
        }
    }

    IEnumerator HitWallCoroutine(Vector2 moveTo)
    {
        yield return new WaitForSeconds(tweenerDuration / 2);
        audioSource.PlayOneShot(hitWallSound);
        Instantiate(hitWallParticle,  (Vector2)transform.position + moveTo, Quaternion.identity);
    }

    void ChangeCurrentInput()
    {
        if (timeSinceLastSet <= 0)
        {
            if (lastInputKey != currentInputKey && CanMove(GetMovementVector(lastInputKey)))
            {
                currentInputKey = lastInputKey;
                timeSinceLastSet = tweenerDuration;
            }
        }

        timeSinceLastSet -= Time.deltaTime;
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
