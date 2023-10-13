using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    private Tween activeTween;

    public bool AddTween(Tween tween)
    {
        if (activeTween != null) return false;

        activeTween = tween;
        return true;
    }

    private void Update()
    {
        UpdateTween();
    }

    private void UpdateTween()
    {
        if (activeTween == null) return;
        
        if (Vector2.Distance(activeTween.Target.position, activeTween.EndPos) < 0.1f)
        {
            activeTween.Target.position = activeTween.EndPos;
            activeTween = null;
        }
        else
        {
            activeTween.Target.position = Vector2.Lerp(activeTween.StartPos, activeTween.EndPos,
                (Time.time - activeTween.StartTime) / activeTween.Duration);
        }
    }
}
