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
        Recover
    }

    [SerializeField] private GhostState currentState;
    
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        currentState = GhostState.Normal;
    }

    public void Scare()
    {
        currentState = GhostState.Scared;
        anim.SetTrigger("Scared");
    }

    public void Recover()
    {
        currentState = GhostState.Recover;
        anim.SetTrigger("Recover");
    }

    public void Normal()
    {
        currentState = GhostState.Normal;
        anim.SetTrigger("Normal");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PacStudentController>(out PacStudentController pacStudentController))
        {
            switch (currentState)
            {
                case GhostState.Normal:
                    pacStudentController.Die();
                    break;
                case GhostState.Scared:
                    break;
                case GhostState.Recover:
                    break;
            }
        }
    }
}
