using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmController : MonoBehaviour
{

    [SerializeField]
    public GameObject stuckObject;

    public Transform floor;

    [SerializeField]
    public bool isHeuristic = true;

    public Slider baseSlider;
    public Slider lowerArmSlider;
    public Slider upperArmSlider;
    public Slider neckHingeSlider;

    private float baseSliderValue = 0f;
    private float lowerArmSliderValue = 0f;
    private float upperArmSliderValue = 0f;
    private float neckHingeSliderValue = 0f;

    public Transform robotBase;
    public Transform robotLowerArm;
    public Transform robotUpperArm;
    public Transform robotNeckHinge;

    public float baseTurnRate = 1f;
    public float lowerArmTurnRate = 1f;
    public float upperArmTurnRate = 1f;
    public float neckHingeTurnRate = 1f;

    private float baseYRot = 0f;
    public float baseYRotMin = -45f;
    public float baseYRotMax = 45f;

    private float lowerArmXRot = 0f;
    public float lowerArmXRotMin = -45f;
    public float lowerArmXRotMax = 45f;

    private float upperArmXRot = 0f;
    public float upperArmXRotMin = -45f;
    public float upperArmXRotMax = 45f;

    private float neckHingeXRot = 0f;
    public float neckHingeXRotMin = -45f;
    public float neckHingeXRotMax = 45f;

    private void Start()
    {
        baseSlider.minValue = -1;
        lowerArmSlider.minValue = -1;
        upperArmSlider.minValue = -1;
        neckHingeSlider.minValue = -1;

        baseSlider.maxValue = 1;
        lowerArmSlider.maxValue = 1;
        upperArmSlider.maxValue = 1;
        neckHingeSlider.maxValue = 1;

        ResetSliders();
    }

    private void Update()
    {
        CheckInput();
        ProcessMovement();
    }

    void CheckInput()
    {
        if (isHeuristic)
        {
            baseSliderValue = baseSlider.value;
            lowerArmSliderValue = lowerArmSlider.value;
            upperArmSliderValue = upperArmSlider.value;
            neckHingeSliderValue = neckHingeSlider.value;
        } 
        else
        {
            // Set values from continous array
        }
    }

    void ProcessMovement()
    {
        baseYRot += baseSliderValue * baseTurnRate;
        baseYRot = Mathf.Clamp(baseYRot, baseYRotMin, baseYRotMax);
        robotBase.localEulerAngles = new Vector3(robotBase.localEulerAngles.x, baseYRot, robotBase.localEulerAngles.z);

        lowerArmXRot += lowerArmSliderValue * lowerArmTurnRate;
        lowerArmXRot = Mathf.Clamp(lowerArmXRot, lowerArmXRotMin, lowerArmXRotMax);
        robotLowerArm.localEulerAngles = new Vector3(lowerArmXRot, robotLowerArm.localEulerAngles.y, robotLowerArm.localEulerAngles.z);

        upperArmXRot += upperArmSliderValue * upperArmTurnRate;
        upperArmXRot = Mathf.Clamp(upperArmXRot, upperArmXRotMin, upperArmXRotMax);
        robotUpperArm.localEulerAngles = new Vector3(upperArmXRot, robotUpperArm.localEulerAngles.y, robotUpperArm.localEulerAngles.z);

        neckHingeXRot += neckHingeSliderValue * neckHingeTurnRate;
        neckHingeXRot = Mathf.Clamp(neckHingeXRot, neckHingeXRotMin, neckHingeXRotMax);
        robotNeckHinge.localEulerAngles = new Vector3(neckHingeXRot, robotNeckHinge.localEulerAngles.y, robotNeckHinge.localEulerAngles.z);
    }

    private void RotateBase(float val)
    {

    }

    public void ResetSliders()
    {
        baseSliderValue = 0f;
        lowerArmSliderValue = 0f;
        upperArmSliderValue = 0f;
        neckHingeSliderValue = 0f;

        baseSlider.value = 0f;
        lowerArmSlider.value = 0f;
        upperArmSlider.value = 0f;
        neckHingeSlider.value = 0f;
    }

    public void SetStuckObject(GameObject obj)
    {
        floor.tag = "Ground";
        stuckObject = obj;
    }
}
