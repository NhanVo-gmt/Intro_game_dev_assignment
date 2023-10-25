using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneCanvas : MonoBehaviour
{
    [SerializeField] private Button level1Btn;
    [SerializeField] private Button level2Btn;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;

    private void OnEnable()
    {
        level1Btn.onClick.RemoveAllListeners();
        level1Btn.onClick.AddListener(LoadAssessment3Scene);
        
        level2Btn.onClick.RemoveAllListeners();
        level2Btn.onClick.AddListener(LoadDesignIteractionScene);
        
        UpdateText();
    }

    private void OnDisable()
    {
        level1Btn.onClick.RemoveAllListeners();
        level2Btn.onClick.RemoveAllListeners();
    }

    void LoadAssessment3Scene()
    {
        SceneManager.LoadScene("Assessment3Scene");
    }

    void LoadDesignIteractionScene()
    {
        SceneManager.LoadScene("InnovationScene");
    }

    void UpdateText()
    {
        scoreText.SetText($"High Score: {PlayerPrefs.GetInt("Score", 0)}"); 
        timeText.SetText($"Time: {HelperMethod.GetTimeString(PlayerPrefs.GetFloat("GameTime", 0))}");
    }
}
