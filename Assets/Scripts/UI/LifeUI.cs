using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    [SerializeField] private GameObject liveIndicatorPrefab;

    private List<GameObject> livesIndicatorList = new List<GameObject>();

    private int maxLives = 3;

    private void Awake()
    {
        for (int i = 0; i < maxLives; i++)
        {
            livesIndicatorList.Add(Instantiate(liveIndicatorPrefab, this.transform));
        }
    }

    public void UpdateLives(int live)
    {
        for (int i = 0; i < live; i++)
        {
            livesIndicatorList[i].SetActive(true);
        }
        
        for (int i = live; i < maxLives; i++)
        {
            livesIndicatorList[i].SetActive(false);
        }
    }
}
