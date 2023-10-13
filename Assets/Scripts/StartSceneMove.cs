using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneMove : MonoBehaviour
{
    [SerializeField] private List<Vector2> moveDirection = new List<Vector2>();

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        AddMoveDirection();
        StartCoroutine(MoveAroundCoroutine());
    }

    void AddMoveDirection()
    {
        for (int i = 0; i < 12; i++)
        {
            moveDirection.Add(Vector2.left);
        }

        for (int i = 0; i < 12; i++)
        {
            moveDirection.Add(Vector2.right);
        }
    }

    IEnumerator MoveAroundCoroutine()
    {
        for (int i = 0; i < moveDirection.Count; i++)
        {
            yield return player.MoveCoroutine(moveDirection[i]);
        }

        yield return MoveAroundCoroutine();
    }
}
