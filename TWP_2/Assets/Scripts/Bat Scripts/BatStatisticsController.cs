using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BatStatisticsController : MonoBehaviour
{
    public int totalHomeRuns = 0;
    public int totalBats = 0;
    public TextMeshProUGUI totalHomeRun;
    public TextMeshProUGUI totalAtBat;
    public TextMeshProUGUI homeRunPercentage;

    public void IncrementHomeRun() {
        totalHomeRuns++;
        UpdateUI();
    }

    public void IncrementAtBat() {
        totalBats++;
        UpdateUI();
    }

    private void UpdateUI() {
        totalHomeRun.text = totalHomeRuns + " Home Runs";
        totalAtBat.text = totalBats + " Swings";
        homeRunPercentage.text = CalculateHomeRunPercentage() + "% Likely To Hit An Homerun";
    }

    private float CalculateHomeRunPercentage() {
        return totalBats > 0 ? (float)totalHomeRuns / totalBats * 100 : 0;
    }
}
