using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class DynamicTouchAgent : Agent
{
    [SerializeField]
    private Material winMat;
    [SerializeField]
    private Material failMat;
    [SerializeField]
    private MeshRenderer floorMeshRenderer;

    [SerializeField]
    private EnvironmentManager envManager;

    [SerializeField]
    private Transform goalTransform;

    [SerializeField]
    private float moveSpeed = 1f;

    private float beginDistance;
    private float prevBest;
    private const float stepPenalty = -0.0001f;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(0, 55, 0);
    
        Vector3 spawnPosition = Random.insideUnitCircle.normalized;
        spawnPosition *= Random.Range(GameManager.inst.MinDist, GameManager.inst.MaxDist);
        goalTransform.localPosition = new Vector3(spawnPosition.x, 3f, spawnPosition.y);

        beginDistance = Vector3.Distance(transform.localPosition, goalTransform.localPosition);
        prevBest = beginDistance;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(goalTransform.localPosition);
        sensor.AddObservation(Vector3.Distance(goalTransform.localPosition, transform.localPosition));
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        float moveZ = actions.ContinuousActions[2];

        transform.localPosition += new Vector3(moveX, moveY, moveZ) * Time.deltaTime * moveSpeed;

        float distance = Vector3.Distance(transform.localPosition,
            goalTransform.localPosition);
        float diff = beginDistance - distance;

        if (distance > prevBest)
        {
            // Penalty if the arm moves away from the closest position to target
            AddReward((prevBest - distance) / MaxStep);
        }
        else
        {
            // Reward if the arm moves closer to target
            AddReward(diff / MaxStep);
            prevBest = distance;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> contActions = actionsOut.ContinuousActions;
        // Can add Y if needed
        contActions[0] = Input.GetAxisRaw("Horizontal");
        contActions[2] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Object")
        {
            Debug.Log("Win");
            AddReward(+10f);
            floorMeshRenderer.material = winMat;
            EndEpisode();
        }
        else if (other.gameObject.tag == "Wall")
        {
            //AddReward(-1f);
            floorMeshRenderer.material = failMat;
            EndEpisode();
        }
    }
}
