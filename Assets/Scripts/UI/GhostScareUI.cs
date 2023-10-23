using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GhostScareUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    
    public Action On3sLeft;
    private bool Is3SLeft;
    public Action On0sLeft;
    private bool Is0SLeft;
    
    private CanvasGroup canvasGroup;
    private float timer = 10;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        Hide();
    }
    
    private void Update()
    {
        if (Is0SLeft) return;
        
        timer = Mathf.Clamp(timer - Time.deltaTime, 0, 10);
        UpdateText();

        if (!Is3SLeft && Mathf.FloorToInt(timer) == 3)
        {
            Is3SLeft = true;
            On3sLeft?.Invoke();
        }

        if (timer <= 0)
        {
            Hide();
        }
    }

    void UpdateText()
    {
        timerText.SetText(Mathf.FloorToInt(timer).ToString());
    }

    public void Show()
    {
        canvasGroup.alpha = 1;
        timer = 10;
        Is3SLeft = false;
        Is0SLeft = false;
    }

    public void Hide()
    {
        Is0SLeft = true;
        On0sLeft?.Invoke();
        canvasGroup.alpha = 0;
    }
}
