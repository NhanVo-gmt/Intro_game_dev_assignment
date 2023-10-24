using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PacStudentController : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioClip hitWallSound;
    
    [Header("Particle")]
    [SerializeField] private ParticleSystem hitWallParticle;
    [SerializeField] private ParticleSystem dieParticle;
    
    [Header("Component")]
    private Animator anim;
    private AudioSource audioSource;
    private BoxCollider2D col;
    private Tweener tweener;
    private SpriteRenderer sprite;

    private float tweenerDuration = 0.5f;

    private KeyCode lastInputKey = KeyCode.S;
    [SerializeField] private KeyCode currentInputKey = KeyCode.S;
    private float timeSinceLastSet = 0f;

    private Vector2 sizedBoxCheck = new Vector2(.9f, .9f);

    private bool hitWall = false;
    private bool isDie = false;
    private bool canMove = false;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        tweener = GetComponent<Tweener>();
        col = GetComponent<BoxCollider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        GameManager.Instance.OnPlayGame += (() => canMove = true);
    }

    private void Update()
    {
        if (isDie || !canMove) return;
        
        GetInput();
        Move();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pellet"))
        {
            other.gameObject.SetActive(false);
            GameManager.Instance.UpdateScore(10);
        }
        else if (other.CompareTag("Cherry"))
        {
            other.gameObject.SetActive(false);
            GameManager.Instance.UpdateScore(100);
        }
        else if (other.CompareTag("Ghost"))
        {
            Ghost ghost = other.GetComponent<Ghost>();
            switch (ghost.GetCurrentState())
            {
                case Ghost.GhostState.Normal:
                    Die();
                    break;
                case Ghost.GhostState.Scared:
                    ghost.Die();
                    GameManager.Instance.UpdateScore(300);
                    break;
                case Ghost.GhostState.Recover:
                    ghost.Die();
                    GameManager.Instance.UpdateScore(300);
                    break;
            }
        }
    }

    #region Move
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
        
        ChangeCurrentInput();
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
    
    IEnumerator HitWallCoroutine(Vector2 moveTo)
    {
        yield return new WaitForSeconds(tweenerDuration / 2);
        audioSource.PlayOneShot(hitWallSound);
        Instantiate(hitWallParticle,  (Vector2)transform.position + moveTo, Quaternion.identity);
    }

    public IEnumerator MoveCoroutine(Vector2 moveTo)
    {
        yield return new WaitUntil(() => tweener.AddTween(new Tween(transform, moveTo, Time.time, .5f)));
        anim.SetFloat("Horizontal", moveTo.x);
        anim.SetFloat("Vertical", moveTo.y);
        
        if (moveSound != null)
            audioSource.PlayOneShot(moveSound);
    }
    
    #endregion
    
    #region Die and respawn
    
    public void Die()
    {
        GameManager.Instance.Die();
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        dieParticle.Play();
        col.enabled = false;
        isDie = true;
        anim.SetTrigger("Die");
        tweener.StopTween();

        yield return new WaitForSeconds(.5f);

        sprite.enabled = false;
        transform.position = new Vector2(1, -1);
        
        yield return new WaitForSeconds(1f);
        
        Respawn();
    }

    void Respawn()
    {
        anim.SetTrigger("Respawn");
        
        currentInputKey = KeyCode.Alpha0;
        lastInputKey = KeyCode.Alpha0;
        
        isDie = false;
        col.enabled = true;
        sprite.enabled = true;
    }
    
    #endregion
    
    public void Teleport(int newX, int newY)
    {
        tweener.StopTween();
        transform.position = new Vector2(newX, newY);
    }
}
