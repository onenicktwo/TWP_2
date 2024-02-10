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
    // Removed the direct reference to RayPerceptionSensorComponentBase since it's not accessed this way

    public float swingForce = 15f; // Fixed force for simplicity
    public float distanceRewardMultiplier = 0.1f;
    public float maxDistanceReward = 1.0f;
    public Vector3 optimalDirection = Vector3.forward; // Assuming forward is the optimal hit direction
    public float directionRewardMultiplier = 1.0f;
    public float trackingErrorPenalty = 0.01f;
    public float timingReward = 1.0f;

    // Updated: Variables for dynamic swing adjustments
    public float maxSwingDelay = 1.0f; // Maximum delay before swinging
    public Vector3 batStartPosition = new Vector3(0, 1, 0);
    public Vector3 ballStartPosition = new Vector3(0, 1, 5);

    public override void OnEpisodeBegin()
    {
        ResetBatAndBall();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(batTransform.InverseTransformPoint(ballRigidbody.position));
        sensor.AddObservation(batTransform.InverseTransformDirection(ballRigidbody.velocity));
        sensor.AddObservation(Time.timeSinceLevelLoad); // Helps to infer the timing aspect
        // Ray sensor observations are automatically added if the component is attached to the agent
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var continuousActions = actionBuffers.ContinuousActions;
        float swingDelay = Mathf.Clamp(continuousActions[0], 0, maxSwingDelay);
        SwingBat(swingDelay); // Simplified for clarity, adjust as needed for your project
    }

    private void SwingBat(float delay)
    {
        // Calculate swing direction with potential modifications based on new actions
        Vector3 swingDirection = transform.forward * swingForce;
        StartCoroutine(ApplySwing(swingDirection, delay));
    }

    private IEnumerator ApplySwing(Vector3 force, float delay)
    {
        yield return new WaitForSeconds(delay); // Use the action to determine delay
        batRigidbody.AddForce(force, ForceMode.Impulse);
    }

    private void ResetBatAndBall()
    {
        batRigidbody.velocity = Vector3.zero;
        batRigidbody.angularVelocity = Vector3.zero;
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;

        batTransform.position = batStartPosition;
        ballRigidbody.position = ballStartPosition;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            float distance = Vector3.Distance(ballRigidbody.position, batTransform.position);
            AddReward(CalculateDistanceReward(distance));

            Vector3 hitDirection = (collision.transform.position - batTransform.position).normalized;
            AddReward(CalculateDirectionReward(hitDirection));
        }
        else
        {
            AddReward(-0.1f); // Penalty for hitting something other than the ball
        }
        EndEpisode(); // End Episode on collision
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetKey(KeyCode.Space) ? 0.5f : 0f; // Simulate swing timing with space bar
    }

    private float CalculateDistanceReward(float distance)
    {
        return Mathf.Clamp(distanceRewardMultiplier / distance, 0, maxDistanceReward);
    }

    private float CalculateDirectionReward(Vector3 hitDirection)
    {
        float directionMatch = Vector3.Dot(hitDirection.normalized, optimalDirection.normalized);
        return directionMatch * directionRewardMultiplier;
    }
}