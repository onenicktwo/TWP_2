using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BatStatisticsController : MonoBehaviour
{
    public int totalHits = 0;
    public int totalScore = 0;
    public int totalBats = 0;
    public TextMeshProUGUI totalHitsText;
    public TextMeshProUGUI totalAtBat;
    public TextMeshProUGUI hitPercentage;
    public TextMeshProUGUI Score;

    public void IncrementAtBat() {
        totalBats++;
        UpdateUI();
    }

    public void IncrementHit() {
        totalHits++;
        totalScore += 5;
        UpdateUI();
    }

    private void UpdateUI() {
        totalHitsText.text = totalHits + " Hits";
        totalAtBat.text = totalBats + " Swings";
        hitPercentage.text = CalculateHitPercentage() + "% Hit Rate";
        Score.text = totalScore + " Points";
    }

    private float CalculateHitPercentage() {
        return totalBats > 0 ? (float)totalHits / totalBats * 100 : 0;
    }
}