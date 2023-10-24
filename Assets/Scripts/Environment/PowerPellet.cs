using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellet : MonoBehaviour
{
    public Action OnActivated;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PacStudentController>())
        {
            OnActivated?.Invoke();
            SoundManager.Instance.PlayScaredGhostMusic();
            gameObject.SetActive(false);
        }
    }

}
