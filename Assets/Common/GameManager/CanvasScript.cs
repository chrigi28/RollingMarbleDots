using System.Collections;
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
    private GameObject FinishPanelPortrait;

    [UsedImplicitly]
    private GameObject FinishPanelLandscape;

    [UsedImplicitly]
    private GameObject CountDownLabel;

    private TextMeshProUGUI finishTime;
    private TextMeshProUGUI bestTime;

    // Start is called before the first frame update
    void Awake()
    {
        GameOverPanel = gameObject.transform.Find("GameOverPanel").gameObject;
        
        FinishPanelPortrait = gameObject.transform.Find("FinishPanelPortrait").gameObject;
        FinishPanelLandscape = gameObject.transform.Find("FinishPanelLandscape").gameObject;
        PausePanel = gameObject.transform.Find("PausePanel").gameObject;
        CountDownLabel = gameObject.transform.Find("CountDownText").gameObject;
        finishTime = this.FinishPanelPortrait.GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(f => f.name == "Time");
        bestTime = this.FinishPanelPortrait.GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(f => f.name == "TimeBest");
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
        this.FinishPanelPortrait.SetActive(false);
        this.FinishPanelLandscape.SetActive(false);
        }

    public void ShowFinish(float time, float besttime)
    {
        if (this.IsLandscape)
        {
            this.SetTime(time, besttime, true);
            this.FinishPanelLandscape.SetActive(true);
        }
        else
        {
            this.SetTime(time, besttime, false);
            this.FinishPanelPortrait.SetActive(true);
        }

    }

    public void SetTime(float time, float besttime, bool landscape)
    {
        if (landscape)
        {
            finishTime = this.FinishPanelLandscape.GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(f => f.name == "Time");
            bestTime = this.FinishPanelLandscape.GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(f => f.name == "TimeBest");
        }
        else
        {
            finishTime = this.FinishPanelPortrait.GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(f => f.name == "Time");
            bestTime = this.FinishPanelPortrait.GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(f => f.name == "TimeBest");
        }

        finishTime.text = $"{time.ToString("F")}s";

        if (besttime < 10)
        {
            besttime = time;
        }

        bestTime.text = $"{besttime.ToString("F")}s";

    }
    public bool IsLandscape
    {
        get
        {
            var res = Screen.currentResolution;
            return res.width > res.height;
        }
    }

}
