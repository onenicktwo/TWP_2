using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class BaseballAgent : Agent
{
    public Rigidbody batRigidbody;
    public Rigidbody ballRigidbody;
    public Transform batTransform;

    public RayPerceptionSensor raySensor;

    // Parameters for swing dynamics
    public float minForce = 10f;
    public float maxForce = 20f;
    public float maxAngle = 45f;
    public float swingDelay = 0.5f;  // Delay before the swing

    private float pitchSpeed;
    private string pitchType;

    public override void OnEpisodeBegin()
    {
        ResetBatAndBall();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Ball's position and velocity relative to the bat
        sensor.AddObservation(batTransform.InverseTransformPoint(ballRigidbody.position));
        sensor.AddObservation(batTransform.InverseTransformDirection(ballRigidbody.velocity));
        sensor.AddObservation(pitchSpeed);
        sensor.AddObservation(ConvertPitchTypeToNumeric(pitchType));
    }

    private float ConvertPitchTypeToNumeric(string type)
    {
        switch(type)
        {
            case "fastball": return 0f;
            case "curveball": return 1f;
            case "slider": return 2f;
            default: return -1f;
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var continuousActions = actionBuffers.ContinuousActions;
        float swingPower = continuousActions[0];
        float swingAngle = continuousActions[1];
        float swingSpeed = continuousActions[2];
        
        
        AdjustBat(swingSpeed);
        SwingBat(swingPower, swingAngle);
        ApplyRewards();
    }

    private void SwingBat(float power, float angle)
    {
        float forceMagnitude = Mathf.Lerp(minForce, maxForce, (power + 1) / 2);
        Vector3 swingForce = transform.forward * forceMagnitude;
        Quaternion swingRotation = Quaternion.Euler(0, angle * maxAngle, 0);

        StartCoroutine(ApplySwing(swingRotation, swingForce));
    }

    private IEnumerator ApplySwing(Quaternion rotation, Vector3 force)
    {
        yield return new WaitForSeconds(swingDelay);

        batRigidbody.MoveRotation(rotation);
        batRigidbody.AddForce(force, ForceMode.Impulse);
    }

    private void ResetBatAndBall()
    {
        batRigidbody.velocity = Vector3.zero;
        batRigidbody.angularVelocity = Vector3.zero;
        // Additional reset logic for ball and game environment
    }

    private void ApplyRewards()
    {
        // Reward logic based on game outcomes
    }

    public void SetPitchData(float speed, string type)
    {
        pitchSpeed = speed;
        pitchType = type;
    }

    private void AdjustRoboticArmBehavior()
    {

    }

    private void AdjustBat(float speed)
    {

    }
}
