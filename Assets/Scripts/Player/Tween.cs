using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween 
{
    public Transform Target { get; private set; }
    public Vector2 StartPos { get; private set; }
    public Vector2 EndPos { get; private set; }
    public float StartTime { get; private set; }
    public float Duration { get; private set;  }

    
    public Tween(Transform Target, Vector2 MovePos, float StartTime, float Duration)
    {
        this.Target = Target;
        this.StartPos = Target.position;
        this.EndPos = StartPos + MovePos;
        this.StartTime = StartTime;
        this.Duration = Duration;
    }
}
