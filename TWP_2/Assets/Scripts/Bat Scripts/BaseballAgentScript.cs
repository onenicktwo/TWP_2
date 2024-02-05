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

    public float swingForce = 15f; // Fixed force for simplicity
    public float distanceRewardMultiplier = 0.1f;
    public float maxDistanceReward = 1.0f;
    public Vector3 optimalDirection = Vector3.forward; // Assuming forward is the optimal hit direction
    public float directionRewardMultiplier = 1.0f;
    public float trackingErrorPenalty = 0.01f;
    public float timingReward = 1.0f;

    public override void OnEpisodeBegin()
    {
        ResetBatAndBall();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(batTransform.InverseTransformPoint(ballRigidbody.position));
        sensor.AddObservation(batTransform.InverseTransformDirection(ballRigidbody.velocity));
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var continuousActions = actionBuffers.ContinuousActions;
        float swingDelay = continuousActions[0]; // Assuming the first action is a delay time before swinging
        SwingBat(swingDelay);
    }

    private void SwingBat(float delay)
    {
        Vector3 swingDirection = transform.forward * swingForce;
        Quaternion batOrientation = Quaternion.Euler(0, 0, 0); // No angle adjustment needed for simplification
        StartCoroutine(ApplySwing(batOrientation, swingDirection, delay));
    }

    private IEnumerator ApplySwing(Quaternion rotation, Vector3 force, float delay)
    {
        yield return new WaitForSeconds(delay); // Use the action to determine delay

        batRigidbody.MoveRotation(rotation);
        batRigidbody.AddForce(force, ForceMode.Impulse);
    }

    private void ResetBatAndBall()
    {
        batRigidbody.velocity = Vector3.zero;
        batRigidbody.angularVelocity = Vector3.zero;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;

        // Reposition bat and ball to their initial positions for the next episode
        batTransform.position = new Vector3(0, 1, 0); // Example starting position
        ballRigidbody.position = new Vector3(0, 1, 5); // Example starting position for the ball
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Example reward calculations (these functions need to be implemented based on your game's logic)
            float distance = Vector3.Distance(ballRigidbody.position, batTransform.position); // Simplified distance calculation
            AddReward(CalculateDistanceReward(distance));

            Vector3 hitDirection = (collision.transform.position - batTransform.position).normalized;
            AddReward(CalculateDirectionReward(hitDirection));

            EndEpisode();
        }
        else
        {
            AddReward(-0.1f);
        }
    }

    public float CalculateDistanceReward(float distance)
    {
        return Mathf.Clamp(distance * distanceRewardMultiplier, 0, maxDistanceReward);
    }

    public float CalculateDirectionReward(Vector3 hitDirection)
    {
        float angle = Vector3.Angle(optimalDirection, hitDirection);
        return Mathf.Max(0, (180 - angle) / 180 * directionRewardMultiplier);
    }

    public float CalculateTrackingReward(Vector3 ballPosition, Vector3 batPosition)
    {
        float alignmentError = Vector3.Distance(ballPosition, batPosition);
        return Mathf.Max(0, 1 - alignmentError * trackingErrorPenalty);
    }

    public float CalculateTimingReward(float swingTime, float optimalTimeWindowStart, float optimalTimeWindowEnd)
    {
        if (swingTime >= optimalTimeWindowStart && swingTime <= optimalTimeWindowEnd)
        {
            return timingReward;
        }
        return 0; // No reward for swinging outside the optimal window
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetKey(KeyCode.Space) ? 1.0f : 0.0f; // Example: Press space to simulate ideal swing timing
    }
}