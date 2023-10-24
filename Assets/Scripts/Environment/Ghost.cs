using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public enum GhostState
    {
        Normal,
        Scared,
        Recover,
        Die
    }

    [SerializeField] private GhostState currentState;
    
    private Animator anim;
    private Collider2D col;

    private float dieTime = 5f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        currentState = GhostState.Normal;
    }

    public void Scare()
    {
        currentState = GhostState.Scared;
        anim.SetTrigger("Scared");
    }

    public void Recover()
    {
        if (IsDead()) return;
        
        currentState = GhostState.Recover;
        anim.SetTrigger("Recover");
    }

    public bool IsDead()
    {
        return currentState == GhostState.Die;
    }

    public void Die()
    {
        currentState = GhostState.Die;
        anim.SetTrigger("Die");
        SoundManager.Instance.PlayDeadGhostMusic();
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        col.enabled = false;
        yield return new WaitForSeconds(dieTime);

        currentState = GhostState.Normal;
        anim.SetTrigger("Normal");
        col.enabled = true;
    }

    public void Normal()
    {
        if (IsDead()) return;
        
        currentState = GhostState.Normal;
        anim.SetTrigger("Normal");
    }

    public GhostState GetCurrentState()
    {
        return currentState;
    }

    private void Update()
    {
        
    }
    
}
