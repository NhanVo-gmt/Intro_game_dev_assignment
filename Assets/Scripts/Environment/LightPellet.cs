using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightPellet : MonoBehaviour
{
    private Light2D globalLight2D;

    private void Awake()
    {
        globalLight2D = GameObject.FindWithTag("GlobalLight").GetComponent<Light2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PacStudentController>())
        {
            globalLight2D.enabled = true;
        }
    }
}
