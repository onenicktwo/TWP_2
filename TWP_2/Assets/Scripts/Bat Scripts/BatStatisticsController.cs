using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BatStatisticsController : MonoBehaviour
{
    public int totalHomeRuns = 0;
    public int totalScore = 0;
    public int totalBats = 0;
    public TextMeshProUGUI totalHomeRun;
    public TextMeshProUGUI totalAtBat;
    public TextMeshProUGUI homeRunPercentage;
    public TextMeshProUGUI Score;


    public void IncrementAtBat() {
        totalBats++;
        UpdateUI();
    }

    public void IncrementHomeRun() {
        totalHomeRuns++;
        totalScore += 10;
        UpdateUI();
    }

    public void regularHit()
    {
        totalScore += 5;
        UpdateUI();
    }

    private void UpdateUI() {
        totalHomeRun.text = totalHomeRuns + " Home Runs";
        totalAtBat.text = totalBats + " Swings";
        homeRunPercentage.text = CalculateHomeRunPercentage() + "% Likely To Hit An Homerun";
        Score.text = totalScore + " Points";
    }

    private float CalculateHomeRunPercentage() {
        return totalBats > 0 ? (float)totalHomeRuns / totalBats * 100 : 0;
    }
}
