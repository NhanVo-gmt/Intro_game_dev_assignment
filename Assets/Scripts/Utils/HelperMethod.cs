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

    public static int GetClosestIndex(GameObject[] gameObjects, GameObject player)
    {
        float dis = Vector2.Distance(gameObjects[0].transform.position, player.transform.position);
        int index = 0;
        for (int i = 1; i < gameObjects.Length; i++)
        {
            float newDis = Vector2.Distance(gameObjects[i].transform.position, player.transform.position);
            if (newDis < dis)
            {
                dis = newDis;
                index = i;
            }
        }

        return index;
    }
}
