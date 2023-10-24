using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GhostController : MonoBehaviour
{
    public enum GhostState
    {
        Normal,
        Scared,
        Recover,
        Die
    }
    
    public enum GhostName
    {
        Ghost1,
        Ghost2,
        Ghost3,
        Ghost4,
    }

    [SerializeField] private GhostName ghostName;
    [SerializeField] private GhostState currentState;
    
    private Animator anim;
    private Collider2D col;
    private Tweener tweener;
    private GameObject pacStudent;

    private List<Vector2> surroundVector2s = new List<Vector2>() { Vector2.up, Vector2.left, Vector2.down, Vector2.right };

    private float dieTime = 5f;
    private float tweenerDuration = 0.6f;
    private bool canMove = false;
    private Vector2 sizedBoxCheck = new Vector2(.9f, .9f);

    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        tweener = GetComponent<Tweener>();
        pacStudent = GameObject.FindWithTag("Player");
        currentState = GhostState.Normal;
    }

    private void Start()
    {
        GameManager.Instance.OnPlayGame += ((() => canMove = true));
        GameManager.Instance.OnPausedGame += ((() => canMove = false));
    }

    private void Update()
    {
        if (pacStudent == null) return;

        if (!canMove) return;
        Move();
    }

    #region State
    
    public void Scare()
    {
        currentState = GhostState.Scared;
        anim.SetTrigger("Scared");
    }

    public void Recover()
    {
        if (IsDead()) return;
        
        currentState = GhostState.Recover;
        anim.SetTrigger("Recover");
    }

    public bool IsDead()
    {
        return currentState == GhostState.Die;
    }

    public void Die()
    {
        currentState = GhostState.Die;
        anim.SetTrigger("Die");
        SoundManager.Instance.PlayDeadGhostMusic();
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        col.enabled = false;
        yield return new WaitForSeconds(dieTime);

        currentState = GhostState.Normal;
        anim.SetTrigger("Normal");
        col.enabled = true;
    }

    public void Normal()
    {
        if (IsDead()) return;
        
        currentState = GhostState.Normal;
        anim.SetTrigger("Normal");
    }

    public GhostState GetCurrentState()
    {
        return currentState;
    }
    
    #endregion

    #region Movement

    void Move()
    {
        Vector2 moveTo = GetMovementVector();
        if (tweener.AddTween(new Tween(transform, moveTo, Time.time, tweenerDuration)))
        {
            anim.SetFloat("Horizontal", moveTo.x);
            anim.SetFloat("Vertical", moveTo.y);
        }
    }

    Vector2 GetMovementVector()
    {
        if (currentState == GhostState.Scared || currentState == GhostState.Recover)
        {
            return GetGhost1MovementVector();
        }
        
        switch (ghostName)
        {
            case GhostName.Ghost1:
                return GetGhost1MovementVector();
            case GhostName.Ghost2:
                return GetGhost2MovementVector();
            case GhostName.Ghost3:
                return GetGhost3MovementVector();
            case GhostName.Ghost4:
                return GetGhost4MovementVector();
        }

        return Vector2.zero;
    }

    Vector2 GetGhost1MovementVector()
    {
        Vector2 res = Vector2.zero;
        float furthestDis = Vector2.Distance(transform.position, pacStudent.transform.position);
        for (int i = 0; i < surroundVector2s.Count; i++)
        {
            if (!CanMove(surroundVector2s[i])) continue;
            
            float newDis = Vector2.Distance(surroundVector2s[i] + (Vector2)transform.position, pacStudent.transform.position);
            if (newDis >= furthestDis)
            {
                furthestDis = newDis;
                res = surroundVector2s[i];
            }
        }

        return res;
    }
    
    Vector2 GetGhost2MovementVector()
    {
        Vector2 res = Vector2.zero;
        float closestDis = Vector2.Distance(transform.position, pacStudent.transform.position);
        for (int i = 0; i < surroundVector2s.Count; i++)
        {
            if (!CanMove(surroundVector2s[i])) continue;
            
            float newDis = Vector2.Distance(surroundVector2s[i] + (Vector2)transform.position, pacStudent.transform.position);
            if (newDis <= closestDis)
            {
                closestDis = newDis;
                res = surroundVector2s[i];
            }
        }

        return res;
    }

    Vector2 GetGhost3MovementVector()
    {
        List<Vector2> validVector2s = new List<Vector2>();
        for (int i = 0; i < surroundVector2s.Count; i++)
        {
            if (!CanMove(surroundVector2s[i])) continue;
            
            validVector2s.Add(surroundVector2s[i]);
        }
        
        return validVector2s[Random.Range(0, validVector2s.Count)];
    }

    Vector2 GetGhost4MovementVector()
    {
        return Vector2.zero;
    }
    
    private bool CanMove(Vector2 moveTo)
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll((Vector2)transform.position + moveTo, sizedBoxCheck, 0);
        foreach (Collider2D collider2D in collider2Ds)
        {
            if (collider2D.CompareTag("Wall"))
            {
                return false;
            }
        }

        return true;
    }

    #endregion
}
