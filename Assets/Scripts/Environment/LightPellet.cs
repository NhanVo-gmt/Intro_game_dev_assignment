using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightPellet : MonoBehaviour
{
    public Action OnActivated;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PacStudentController>())
        {
            OnActivated?.Invoke();
            Destroy(gameObject);
        }
    }
}
