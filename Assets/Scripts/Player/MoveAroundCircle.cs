using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAroundCircle : MonoBehaviour
{
    [SerializeField] private List<Vector2> moveDirection = new List<Vector2>();

    private PacStudentController m_PacStudentController;

    private void Awake()
    {
        m_PacStudentController = GetComponent<PacStudentController>();
    }

    private void Start()
    {
        AddMoveDirection();
        StartCoroutine(MoveAroundCoroutine());
    }

    void AddMoveDirection()
    {
        for (int i = 0; i < 4; i++)
        {
            moveDirection.Add(Vector2.down);
        }

        for (int i = 0; i < 5; i++)
        {
            moveDirection.Add(Vector2.right);
        }
        
        for (int i = 0; i < 4; i++)
        {
            moveDirection.Add(Vector2.up);
        }

        for (int i = 0; i < 5; i++)
        {
            moveDirection.Add(Vector2.left);
        }
    }

    IEnumerator MoveAroundCoroutine()
    {
        for (int i = 0; i < moveDirection.Count; i++)
        {
            yield return m_PacStudentController.MoveCoroutine(moveDirection[i]);
        }

        yield return MoveAroundCoroutine();
    }
}
