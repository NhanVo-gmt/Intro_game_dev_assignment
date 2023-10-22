using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryMovement : MonoBehaviour
{
    [SerializeField] private float moveDuration = 0.2f;
    private Vector2 direction = Vector2.zero;
    private Tweener tweener;
    
    [Header("Camera Out bound")]
    private int minX = Int32.MinValue;
    private int maxX = Int32.MaxValue;
    private int minY = Int32.MinValue;
    private int maxY = Int32.MaxValue;

    private void Awake()
    {
        tweener = GetComponent<Tweener>();
    }

    public void SetDirection(Vector2 moveTo)
    {
        direction = moveTo.normalized;
    }

    public void SetCameraBound(int minX, int maxX, int minY, int maxY)
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
    }

    private void Update()
    {
        if (direction == Vector2.zero) return;
        
        Move();
        DestroyIfOutOfCamera();
    }

    void Move()
    {
        tweener.AddTween(new Tween(transform, direction, Time.time, moveDuration));
    }

    bool CheckIfOutOfCamera()
    {
        return transform.position.x < minX || transform.position.x > maxX || transform.position.y < minY ||
               transform.position.y > maxY;
    }

    void DestroyIfOutOfCamera()
    {
        if (CheckIfOutOfCamera())
        {
            Destroy(gameObject);
        }
    }
}
