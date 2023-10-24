using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneMove : MonoBehaviour
{
    [SerializeField] private List<Vector2> moveDirection = new List<Vector2>();
    [SerializeField] private int moveStep = 21;
    [SerializeField] private bool isVisible = true;

    private Animator anim;
    private Tweener tweener;
    private SpriteRenderer sprite;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        tweener = GetComponent<Tweener>();
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
            yield return MoveCoroutine(moveDirection[i]);
        }

        yield return MoveAroundCoroutine();
    }
    
    public IEnumerator MoveCoroutine(Vector2 moveTo)
    {
        yield return new WaitUntil(() => tweener.AddTween(new Tween(transform, moveTo, Time.time, .5f)));
        anim.SetFloat("Horizontal", moveTo.x);
        anim.SetFloat("Vertical", moveTo.y);
    }
}
