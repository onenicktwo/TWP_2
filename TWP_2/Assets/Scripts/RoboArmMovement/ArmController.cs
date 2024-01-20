using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmController : MonoBehaviour
{
    public Slider baseSlider;
    public Slider lowerArmSlider;
    public Slider upperArmSlider;
    public Slider neckHingeSlider;
    public Slider neckSwivelSlider;

    private float baseSliderValue = 0f;
    private float lowerArmSliderValue = 0f;
    private float upperArmSliderValue = 0f;
    private float neckHingeSliderValue = 0f;
    private float neckSwivelSliderValue = 0f;

    public Rigidbody robotBase;
    public Rigidbody robotLowerArm;
    public Rigidbody robotUpperArm;
    public Rigidbody robotNeckHinge;
    public Rigidbody robotNeckSwivel;

    public float baseTurnRate = 1f;
    public float lowerArmTurnRate = 1f;
    public float upperArmTurnRate = 1f;
    public float neckHingeTurnRate = 1f;
    public float neckSwivelTurnRate = 1f;

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

    private float neckSwivelYRot = 0f;
    public float neckSwivelYRotMin = -180f;
    public float neckSwivelYRotMax = 180f;

    private void Start()
    {
        baseSlider.minValue = -1;
        lowerArmSlider.minValue = -1;
        upperArmSlider.minValue = -1;
        neckHingeSlider.minValue = -1;
        neckSwivelSlider.minValue = -1;

        baseSlider.maxValue = 1;
        lowerArmSlider.maxValue = 1;
        upperArmSlider.maxValue = 1;
        neckHingeSlider.maxValue = 1;
        neckSwivelSlider.maxValue = 1;

        ResetSliders();
    }

    private void FixedUpdate()
    {
        CheckInput();
        ProcessMovement();
    }

    void CheckInput()
    {
        baseSliderValue = baseSlider.value;
        lowerArmSliderValue = lowerArmSlider.value;
        upperArmSliderValue = upperArmSlider.value;
        neckHingeSliderValue = neckHingeSlider.value;
        neckSwivelSliderValue = neckSwivelSlider.value;
    }

    void ProcessMovement()
    {
        baseYRot += baseSliderValue * baseTurnRate;
        baseYRot = Mathf.Clamp(baseYRot, baseYRotMin, baseYRotMax);
        robotBase.rotation = Quaternion.Euler(robotBase.rotation.x, baseYRot, robotBase.rotation.z);

        lowerArmXRot += lowerArmSliderValue * lowerArmTurnRate;
        lowerArmXRot = Mathf.Clamp(lowerArmXRot, lowerArmXRotMin, lowerArmXRotMax);
        robotLowerArm.rotation = Quaternion.Euler(lowerArmXRot, robotLowerArm.rotation.y, robotLowerArm.rotation.z);

        upperArmXRot += upperArmSliderValue * upperArmTurnRate;
        upperArmXRot = Mathf.Clamp(upperArmXRot, upperArmXRotMin, upperArmXRotMax);
        robotUpperArm.rotation = Quaternion.Euler(upperArmXRot, robotUpperArm.rotation.y, robotUpperArm.rotation.z);

        neckHingeXRot += neckHingeSliderValue * neckHingeTurnRate;
        neckHingeXRot = Mathf.Clamp(neckHingeXRot, neckHingeXRotMin, neckHingeXRotMax);
        robotNeckHinge.rotation = Quaternion.Euler(neckHingeXRot, robotNeckHinge.rotation.y, robotNeckHinge.rotation.z);

        neckSwivelYRot += neckSwivelSliderValue * neckSwivelTurnRate;
        neckSwivelYRot = Mathf.Clamp(neckSwivelYRot, neckSwivelYRotMin, neckSwivelYRotMax);
        robotNeckSwivel.rotation = Quaternion.Euler(robotNeckSwivel.rotation.x, neckSwivelYRot, robotNeckSwivel.rotation.z);
    }

    public void ResetSliders()
    {
        baseSliderValue = 0f;
        lowerArmSliderValue = 0f;
        upperArmSliderValue = 0f;
        neckHingeSliderValue = 0f;
        neckSwivelSliderValue = 0f;

        baseSlider.value = 0f;
        lowerArmSlider.value = 0f;
        upperArmSlider.value = 0f;
        neckHingeSlider.value = 0f;
        neckSwivelSlider.value = 0f;
    }
}
