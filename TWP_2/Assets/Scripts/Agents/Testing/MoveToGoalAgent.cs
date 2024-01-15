using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField]
    private Material winMat;
    [SerializeField]
    private Material failMat;
    [SerializeField]
    private MeshRenderer floorMeshRenderer;

    [SerializeField]
    private Transform goalTransform;

    [SerializeField]
    private float moveSpeed = 1f;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-4f, -1f), 1, Random.Range(-1.5f, 1.5f));
        goalTransform.localPosition = new Vector3(Random.Range(4f, 1f), 1, Random.Range(-1.5f, 1.5f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(goalTransform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> contActions = actionsOut.ContinuousActions;
        contActions[0] = Input.GetAxisRaw("Horizontal");
        contActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Goal")
        {
            SetReward(+1f);
            floorMeshRenderer.material = winMat;
            EndEpisode();
        } 
        else if (other.gameObject.tag == "Wall")
        {
            SetReward(-1f);
            floorMeshRenderer.material = failMat;
            EndEpisode();
        }
    }
}
