using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GhostScareUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        Hide();
    }

    public void UpdateTime(float timer)
    {
        timerText.SetText(Mathf.FloorToInt(timer).ToString());
    }

    public void Show()
    {
        canvasGroup.alpha = 1;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
    }
}
