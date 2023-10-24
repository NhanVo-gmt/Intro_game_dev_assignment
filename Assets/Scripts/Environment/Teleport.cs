using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Teleport otherTeleport;

    public bool canTeleport = true;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canTeleport) return;
        
        if (other.TryGetComponent<PacStudentController>(out PacStudentController pacStudent))
        {
            
            otherTeleport.canTeleport = false;
            pacStudent.Teleport((int)otherTeleport.transform.position.x, (int)pacStudent.transform.position.y);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PacStudentController>())
        {
            canTeleport = true;
        }
    }
}
