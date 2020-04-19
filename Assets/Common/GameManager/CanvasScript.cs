﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    [UsedImplicitly]
    private GameObject GameOverPanel;

    [UsedImplicitly]
    private GameObject PausePanel;

    [UsedImplicitly]
    private GameObject FinishPanel;

    [UsedImplicitly]
    private GameObject CountDownLabel;
    
    // Start is called before the first frame update
    void Awake()
    {
        GameOverPanel = gameObject.transform.Find("GameOverPanel").gameObject;
        FinishPanel = gameObject.transform.Find("FinishPanel").gameObject;
        PausePanel = gameObject.transform.Find("PausePanel").gameObject;
        CountDownLabel = gameObject.transform.Find("CountDownText").gameObject;
    }

    public void SetGameOver(bool value)
    {
        this.GameOverPanel.SetActive(true);
    }

    public void TogglePause()
    {
        this.PausePanel.SetActive(GameManager.Instance.IsPaused());
    }

    public void DisableMenus()
    {
        this.GameOverPanel.SetActive(false);
        this.PausePanel.SetActive(false);
        this.FinishPanel.SetActive(false);
    }

    public void ShowFinish(float time, float besttime)
    {
        var finishTime = this.FinishPanel.GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(f => f.name == "Time");
        finishTime.text = $"{time.ToString("F")}s";
        var bestTime = this.FinishPanel.GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(f => f.name == "TimeBest");
        if (besttime < 10)
        {
            besttime = time;
        }

        bestTime.text = $"{besttime.ToString("F")}s";
        this.FinishPanel.SetActive(true);
    }

    public void StartGame()
    {
    }
}
