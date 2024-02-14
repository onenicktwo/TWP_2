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

    public float swingForce = 15f;
    public float distanceRewardMultiplier = 0.1f;
    public float maxDistanceReward = 1.0f;
    public Vector3 optimalDirection = Vector3.forward;
    public float directionRewardMultiplier = 1.0f;
    public float timingReward = 1.0f;

    public float maxSwingDelay = 1.0f;
    private Vector3 batStartPosition;
    private Vector3 ballStartPosition = new Vector3(0, 1, 5);

    private bool hasSwung = false;

    void Start()
    {
        batStartPosition = batTransform.position;
        SetRotationConstraints();
    }

    private void SetRotationConstraints()
    {
        batRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | 
                                   RigidbodyConstraints.FreezeRotationZ | 
                                   RigidbodyConstraints.FreezePosition;
    }

    public override void OnEpisodeBegin()
    {
        ResetBatAndBall();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        int observationCounter = 0;
        sensor.AddObservation(batTransform.InverseTransformPoint(ballRigidbody.position));
        observationCounter += 3;

        sensor.AddObservation(batTransform.InverseTransformDirection(ballRigidbody.velocity));
        observationCounter += 3;
        
        Debug.Log("Total observations added: " + observationCounter);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float swingDelay = Mathf.Clamp(actionBuffers.ContinuousActions[0], 0, maxSwingDelay);
        SwingBat(swingDelay);
    }

    private void SwingBat(float delay)
    {
        if (!hasSwung)
        {
            hasSwung = true;
            StartCoroutine(ApplySwing(delay));
        }
    }

    private IEnumerator ApplySwing(float delay)
    {
        yield return new WaitForSeconds(delay);
        batRigidbody.AddTorque(transform.forward * swingForce, ForceMode.Impulse);
        yield return new WaitForSeconds(maxSwingDelay);
        hasSwung = false;
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
            hasSwung = false;
            float distance = Vector3.Distance(ballRigidbody.position, batTransform.position);
            float directionMatch = Vector3.Dot((collision.transform.position - batTransform.position).normalized, optimalDirection.normalized);
            AddReward((distanceRewardMultiplier / distance) * directionMatch);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        
        continuousActionsOut[0] = Input.GetKey(KeyCode.Space) ? 0f : 1f;
    }


}
