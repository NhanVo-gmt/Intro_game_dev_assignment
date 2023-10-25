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
    public Action OnRespawn;
    
    private Animator anim;
    private Collider2D col;
    private Tweener tweener;
    private GameObject pacStudent;
    private GameObject spawmObject;

    private float dieTime = 5f;

    
    [Header("Enemy AI")]
    private List<Vector2> surroundVector2s = new List<Vector2>() { Vector2.up, Vector2.left, Vector2.down, Vector2.right };
    private float tweenerDuration = 0.4f;
    private bool canMove = false;
    private Vector2 sizedBoxCheck = new Vector2(.8f, .8f);
    private Vector2 lastMoveVector = Vector2.zero;
    private bool isInSpawnArea = true;

    [Header("Enemy 4")] 
    [SerializeField] private GameObject[] outsidePatrols;
    private int patrolIndex;
    private bool isOutside = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        tweener = GetComponent<Tweener>();
        pacStudent = GameObject.FindWithTag("Player");
        spawmObject = GameObject.FindWithTag("Spawn");
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

        ResetState();
    }

    void ResetState()
    {
        currentState = GhostState.Normal;
        anim.SetTrigger("Normal");
        col.enabled = true;
        OnRespawn?.Invoke();
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
            lastMoveVector = HelperMethod.GetReverseVector(moveTo);
            anim.SetFloat("Horizontal", moveTo.x);
            anim.SetFloat("Vertical", moveTo.y);
        }
    }

    Vector2 GetMovementVector()
    {
        if (currentState == GhostState.Scared || currentState == GhostState.Recover)
        {
            patrolIndex = -1;
            return GetGhost1MovementVector(pacStudent);
        }
        else if (currentState == GhostState.Die)
        {
            patrolIndex = 0;
            return GetSpawnMovementVector();
        }
        
        switch (ghostName)
        {
            case GhostName.Ghost1:
                return GetGhost1MovementVector(pacStudent);
            case GhostName.Ghost2:
                return GetGhost2MovementVector(pacStudent);
            case GhostName.Ghost3:
                return GetGhost3MovementVector();
            case GhostName.Ghost4:
                return GetGhost4MovementVector();
        }

        // not coming to this case
        return Vector2.zero;
    }

    Vector2 GetGhost1MovementVector(GameObject target)
    {
        Vector2 res = Vector2.zero;
        float furthestDis = Vector2.Distance(transform.position, target.transform.position);
        for (int i = 0; i < surroundVector2s.Count; i++)
        {
            if (!CanMove(surroundVector2s[i])) continue;
            
            float newDis = Vector2.Distance(surroundVector2s[i] + (Vector2)transform.position, target.transform.position);
            if (newDis >= furthestDis)
            {
                furthestDis = newDis;
                res = surroundVector2s[i];
            }
        }
        
        if (res == Vector2.zero)
        {
            return GetGhost3MovementVector();
        }

        return res;
    }
    
    Vector2 GetGhost2MovementVector(GameObject target)
    {
        Vector2 res = Vector2.zero;
        float closestDis = Vector2.Distance(transform.position, target.transform.position);
        for (int i = 0; i < surroundVector2s.Count; i++)
        {
            if (!CanMove(surroundVector2s[i])) continue;
            
            float newDis = Vector2.Distance(surroundVector2s[i] + (Vector2)transform.position, target.transform.position);
            if (newDis <= closestDis)
            {
                closestDis = newDis;
                res = surroundVector2s[i];
            }
        }

        if (res == Vector2.zero)
        {
            return GetGhost3MovementVector();
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

        if (validVector2s.Count == 0)
        {
            return lastMoveVector;
        }
            
        
        return validVector2s[Random.Range(0, validVector2s.Count)];
    }

    Vector2 GetGhost4MovementVector()
    {
        if (patrolIndex >= outsidePatrols.Length)
        {
            patrolIndex = 0;
        }
        else if (patrolIndex == -1) patrolIndex = HelperMethod.GetClosestIndex(outsidePatrols, gameObject);

        if (Vector2.Distance(transform.position, outsidePatrols[patrolIndex].transform.position) < 0.1f)
        {
            patrolIndex++;
        }

        return GetGhost2MovementVector(outsidePatrols[patrolIndex]);
    }
    
    Vector2 GetSpawnMovementVector()
    {
        Vector2 direction = spawmObject.transform.position - transform.position;
        return direction.normalized;
    }
    
    private bool CanMove(Vector2 moveTo)
    {
        if (lastMoveVector == moveTo) return false;
        
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll((Vector2)transform.position + moveTo, sizedBoxCheck, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (!isInSpawnArea && collider.CompareTag("Spawn")) return false;
            if (collider.CompareTag("Wall") || collider.CompareTag("Teleport")) return false;
        }

        return true;
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spawn"))
        {
            StopAllCoroutines();
            MatchGroupGhostState();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Spawn"))
        {
            Debug.Log(1);
            isInSpawnArea = false;
        }
    }

    private void MatchGroupGhostState()
    {
        currentState = GameManager.Instance.currentGroupGhostState;
        switch (currentState)
        {
            case GhostState.Normal:
                Normal();
                break;
            case GhostState.Recover:
                Recover();
                break;
            case GhostState.Scared:
                Scare();
                break;
        }
    }
}
