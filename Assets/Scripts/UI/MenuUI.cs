using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button level1Btn;
    [SerializeField] private Button level2Btn;


    private void OnEnable()
    {
        level1Btn.onClick.RemoveAllListeners();
        level1Btn.onClick.AddListener(LoadAssessment3Scene);
        
        level2Btn.onClick.RemoveAllListeners();
        level2Btn.onClick.AddListener(LoadDesignIteractionScene);
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
        SceneManager.LoadScene("DesignIteractionScene");
    }
}
