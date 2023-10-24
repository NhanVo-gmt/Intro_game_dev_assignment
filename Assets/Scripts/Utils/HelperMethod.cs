using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperMethod
{
    public static string GetTimeString(float timer)
    {
        int fraction = (int)((timer * 100) % 100);
        int second = (int)timer % 100;
        int minute = 0;
        return $"{FormatTimeString(minute)}:{FormatTimeString(second)}:{FormatTimeString(fraction)}";
    }

    private static string FormatTimeString(int time)
    {
        return time < 10 ? $"0{time}" : time.ToString();
    }

    public static Vector2 GetReverseVector(Vector2 reverseVector)
    {
        return reverseVector * -1;
    }

    public static Vector2 SnapToGrid(Vector2 position)
    {
        return new Vector2((int)position.x, (int)position.y);
    }
}
