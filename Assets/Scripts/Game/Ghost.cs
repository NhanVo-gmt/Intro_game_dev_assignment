using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Scare()
    {
        anim.SetTrigger("Scared");
    }

    public void Recover()
    {
        anim.SetTrigger("Recover");
    }

    public void Normal()
    {
        anim.SetTrigger("Normal");
    }
}
