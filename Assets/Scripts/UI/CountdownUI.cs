using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private TextMeshProUGUI text;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Show()
    {
        canvasGroup.alpha = 1f;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
    }

    public void UpdateText(float time)
    {
        int intTime = Mathf.FloorToInt(time);
        if (intTime <= 0f)
        {
            text.SetText("Go!!!");
        }
        else
        {
            text.SetText($"{intTime}");
        }
    }
}
