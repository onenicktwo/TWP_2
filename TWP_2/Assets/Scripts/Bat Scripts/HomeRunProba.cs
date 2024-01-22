using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeRunProbabilityController : MonoBehaviour
{
    public Slider homeRunSlider;
    private float homeRunProbability;

    void Start()
    {
        homeRunSlider.onValueChanged.AddListener(delegate { SliderValueChanged(); });
        homeRunSlider.onValueChanged.AddListener(delegate { counterTest(); });
    }

    void SliderValueChanged()
    {
        homeRunProbability = homeRunSlider.value;
        AdjustRoboticArmBehavior(homeRunProbability); // Future
        Debug.Log("Home Run Probability is: " + homeRunProbability);
    }

    void counterTest()
    {
        GameObject batstats = GameObject.Find("BatStatsController");
        if(batstats != null)
        {
            BatStatisticsController batStatistics = batstats.GetComponent<BatStatisticsController>();
            if(batStatistics != null)
            {
                batStatistics.IncrementHomeRun();
                batStatistics.IncrementAtBat();
            }

        }
    }

   void AdjustRoboticArmBehavior(float probability)
    {
        // Future
        //method to adjust the arm behavior based on the probability value
    }
}
