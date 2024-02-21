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
    [SerializeField]
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

    public override void OnEpisodeBegin()
    {

    float goalAreaWidth = 10f;
    float goalAreaLength = 10f;
    float minDistance = 10f;
    int maxAttempts = 100;

    Vector3 spawnPosition;

    // Get the current position of the goal
    Vector3 goalPosition = goalTransform.localPosition;

    for (int i = 0; i < maxAttempts; i++)
    {
        spawnPosition = new Vector3(Random.Range(-goalAreaWidth / 2f, goalAreaWidth / 2f),
                                     Random.Range(50f, 60f), // Ensure it's different from the goal area's Y-coordinate
                                     Random.Range(-goalAreaLength / 2f, goalAreaLength / 2f));

        if (!IsInsideGoalArea(spawnPosition, goalAreaWidth, goalAreaLength, minDistance) &&
            Vector3.Distance(spawnPosition, goalPosition) > minDistance)
        {
            goalTransform.localPosition = spawnPosition;
            break;
        }

        if (i == maxAttempts - 1)
        {
            Debug.LogWarning("Failed to find a valid spawn position outside the goal area");
            goalTransform.localPosition = new Vector3(0f, 3f, 0f); // Default position
        }
    }



        beginDistance = Vector3.Distance(envTransform.InverseTransformPoint(transform.position), envTransform.InverseTransformPoint(goalTransform.position));
        prevBest = beginDistance;
        // prevBest = Vector3.Distance(envTransform.TransformPoint(transform.position), envTransform.TransformPoint(goalTransform.position)); 
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(envTransform.InverseTransformPoint(transform.position));
        sensor.AddObservation(envTransform.InverseTransformPoint(goalTransform.position));
        sensor.AddObservation(goalAreaTransform.localPosition);
        sensor.AddObservation(Vector3.Distance(envTransform.InverseTransformPoint(transform.position), envTransform.InverseTransformPoint(goalTransform.position)));
        sensor.AddObservation(Vector3.Distance(envTransform.InverseTransformPoint(goalTransform.position), envTransform.InverseTransformPoint(goalAreaTransform.position)));
        sensor.AddObservation(hasObject);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        float moveZ = actions.ContinuousActions[2];

        transform.position += new Vector3(moveX, moveY, moveZ) * Time.deltaTime * moveSpeed;

        float distance;
        float diff;
        
        if (hasObject == false)
        {
            distance = Vector3.Distance(envTransform.InverseTransformPoint(transform.position), envTransform.InverseTransformPoint(goalTransform.position));
        } 
        else
        {
            // distance = Vector3.Distance(envTransform.InverseTransformPoint(goalTransform.position), envTransform.InverseTransformPoint(goalAreaTransform.position));
            distance = Vector3.Distance(envTransform.InverseTransformPoint(transform.position), envTransform.InverseTransformPoint(goalAreaTransform.position));
        }

        diff = beginDistance - distance;
        // diff = prevBest - distance;
        if (distance > prevBest)
        {
            // Penalty if the arm moves away from the closest position to target
            AddReward((prevBest - distance) / 10000);
        }
        else
        {
            // Reward if the arm moves closer to target
            AddReward(diff / 10000);
            prevBest = distance;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> contActions = actionsOut.ContinuousActions;
        // Can add Z if needed
        contActions[0] = Input.GetAxisRaw("Horizontal");
        contActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            CollisionFail();
        }
        if (other.gameObject.tag == "GoalArea" && hasObject == true)
        {
            GoalAreaReached();
        }
        if (other.gameObject.tag == "Object" && hasObject == false)
        {
            Debug.Log("Touched Obj");
            AddReward(+5f);

            hasObject = true;

            goalTransform.parent = objParent;
            beginDistance = Vector3.Distance(envTransform.InverseTransformPoint(transform.position), envTransform.InverseTransformPoint(goalAreaTransform.position));
            prevBest = beginDistance;

            //beginDistance = Vector3.Distance(envTransform.InverseTransformPoint(goalTransform.position), envTransform.InverseTransformPoint(goalAreaTransform.position));
            //prevBest = beginDistance;
        }
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Object" && hasObject == false)
        {
            Debug.Log("Touched Obj");
            AddReward(+1f);

            hasObject = true;

            goalTransform.parent = objParent;
            beginDistance = Vector3.Distance(envTransform.InverseTransformPoint(transform.position), envTransform.InverseTransformPoint(goalAreaTransform.position));
            prevBest = beginDistance;
            Debug.Log(beginDistance);
            Debug.Log(envTransform.InverseTransformPoint(transform.position));
            Debug.Log(envTransform.InverseTransformPoint(goalAreaTransform.position));
            Debug.Log(GetCumulativeReward());
            //beginDistance = Vector3.Distance(envTransform.InverseTransformPoint(goalTransform.position), envTransform.InverseTransformPoint(goalAreaTransform.position));
            //prevBest = beginDistance;
        }
    }
    */

    public void CollisionFail()
    {
        floorMeshRenderer.material = failMat;
        EndEpisode();
    }

    public void GoalAreaReached()
    {
        if (hasObject)
        {
            Debug.Log("Goal Area Reached");
            AddReward(+5f);
            floorMeshRenderer.material = winMat;
            EndEpisode();
        }
        else
        {
            beginDistance = Vector3.Distance(envTransform.InverseTransformPoint(transform.position), envTransform.InverseTransformPoint(goalTransform.position));
            prevBest = beginDistance;
            // prevBest = Vector3.Distance(envTransform.TransformPoint(transform.position), envTransform.TransformPoint(goalTransform.position));
        }
    }
    private bool IsInsideGoalArea(Vector3 position, float width, float length, float minDistance)
    {
        // Check if the position is inside the goal area based on its dimensions
        if (Mathf.Abs(position.x) < width / 2f - minDistance && Mathf.Abs(position.z) < length / 2f - minDistance)
        {
            return true;
        }
        return false;
    }
}
