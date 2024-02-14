using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class DynamicPicknPlace : Agent
{
    [SerializeField]
    private Material winMat;
    [SerializeField]
    private Material failMat;
    [SerializeField]
    private MeshRenderer floorMeshRenderer;

    [SerializeField]
    private EnvironmentManager envManager;
    private Transform envTransform;

    [SerializeField]
    private Transform goalTransform;
    private DynamicObject dynObject;

    [SerializeField]
    private Transform goalAreaTransform;
    [SerializeField]
    private Transform objParent;

    private bool hasObject = false;

    [SerializeField]
    private float moveSpeed = 1f;

    private float beginDistance;
    private float prevBest;
    private const float stepPenalty = -0.0001f;

    private void Start()
    {
        dynObject = goalTransform.GetComponent<DynamicObject>();
        dynObject.dynPnP = this;
        envTransform = envManager.transform;
    }
    public override void OnEpisodeBegin()
    {
        goalTransform.parent = envTransform;
        hasObject = false;
        transform.localPosition = new Vector3(0, 55, 0);

        dynObject.RestartEpisode();
        Vector3 spawnPosition = Random.insideUnitCircle.normalized;
        spawnPosition *= Random.Range(GameManager.inst.MinDist, GameManager.inst.MaxDist);
        goalTransform.localPosition = new Vector3(spawnPosition.x, 3f, spawnPosition.y);

        spawnPosition = Random.insideUnitCircle.normalized;
        spawnPosition *= Random.Range(GameManager.inst.MinDist, GameManager.inst.MaxDist);
        goalAreaTransform.localPosition = new Vector3(spawnPosition.x, 3f, spawnPosition.y);

        beginDistance = Vector3.Distance(envTransform.TransformPoint(transform.position), envTransform.TransformPoint(goalTransform.position));
        prevBest = beginDistance;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(envTransform.TransformPoint(transform.position));
        sensor.AddObservation(envTransform.TransformPoint(goalTransform.position));
        sensor.AddObservation(goalAreaTransform.localPosition);
        sensor.AddObservation(Vector3.Distance(envTransform.TransformPoint(transform.position), envTransform.TransformPoint(goalTransform.position)));
        sensor.AddObservation(Vector3.Distance(envTransform.TransformPoint(goalTransform.position), envTransform.TransformPoint(goalAreaTransform.position)));
        sensor.AddObservation(hasObject);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        float moveZ = actions.ContinuousActions[2];

        transform.localPosition += new Vector3(moveX, moveY, moveZ) * Time.deltaTime * moveSpeed;

        float distance;
        float diff;
        
        if (hasObject == false)
        {
            distance = Vector3.Distance(envTransform.TransformPoint(transform.position), envTransform.TransformPoint(goalTransform.position));
        } 
        else
        {
            distance = Vector3.Distance(envTransform.TransformPoint(goalTransform.position), envTransform.TransformPoint(goalAreaTransform.position));
        }

        diff = beginDistance - distance;
        if (distance > prevBest)
        {
            // Penalty if the arm moves away from the closest position to target
            AddReward((prevBest - distance) / 1000);
        }
        else
        {
            // Reward if the arm moves closer to target
            AddReward(diff / 1000);
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
        if (other.gameObject.tag == "Wall")
        {
            CollisionFail();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Object" && hasObject == false)
        {
            Debug.Log("Touched Obj");
            AddReward(+10f);

            hasObject = true;

            goalTransform.parent = objParent;
            beginDistance = Vector3.Distance(envTransform.TransformPoint(goalTransform.position), envTransform.TransformPoint(goalAreaTransform.position));
            prevBest = beginDistance;
        }
    }

    public void CollisionFail()
    {
        AddReward(-10f);
        floorMeshRenderer.material = failMat;
        EndEpisode();
    }

    public void GoalAreaReached()
    {
        if (hasObject)
        {
            Debug.Log("Goal Area Reached");
            AddReward(+10f);
            floorMeshRenderer.material = winMat;
            EndEpisode();
        }
        else
        {
            dynObject.RestartEpisode();
            beginDistance = Vector3.Distance(envTransform.TransformPoint(transform.position), envTransform.TransformPoint(goalTransform.position));
            prevBest = beginDistance;
        }
    }
}
