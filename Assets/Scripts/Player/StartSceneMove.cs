using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneMove : MonoBehaviour
{
    [SerializeField] private List<Vector2> moveDirection = new List<Vector2>();
    [SerializeField] private int moveStep = 21;
    [SerializeField] private bool isVisible = true;

    private PacStudentController m_PacStudentController;
    private SpriteRenderer sprite;

    private void Awake()
    {
        m_PacStudentController = GetComponent<PacStudentController>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        AddMoveDirection();
        StartCoroutine(MoveAroundCoroutine());
    }

    void AddMoveDirection()
    {
        for (int i = 0; i < moveStep; i++)
        {
            moveDirection.Add(Vector2.left);
        }

        for (int i = 0; i < moveStep; i++)
        {
            moveDirection.Add(Vector2.right);
        }
    }

    IEnumerator MoveAroundCoroutine()
    {
        for (int i = 0; i < moveDirection.Count; i++)
        {
            if (!isVisible)
            {
                if (i == moveStep - 1)
                {
                    sprite.enabled = true;
                }
                else if (i == 0)
                {
                    sprite.enabled = false;
                }
            }
            yield return m_PacStudentController.MoveCoroutine(moveDirection[i]);
        }

        yield return MoveAroundCoroutine();
    }
}
