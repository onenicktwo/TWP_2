using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class PicknPlace : Agent
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

    public bool hasObject = false;

    [SerializeField]
    private float moveSpeed = 1f;

    private float beginDistance;
    private float prevBest;
    private const float stepPenalty = -0.0001f;


    private void Start()
    {
        dynObject = goalTransform.GetComponent<DynamicObject>();
        dynObject.pnp = this;
    }
    public override void OnEpisodeBegin()
    {
        goalTransform.parent = envTransform;
    hasObject = false;
    transform.localPosition = new Vector3(0, 55, 0);
    Vector3 minimumDistance = new Vector3(20.0f, 5.0f, 20.0f);

    Vector3 spawnPosition;
    float distanceCheck;
    bool touching = true;
    //Debug.Log(touching + "inside of OnEpisodeBegin");
    while (touching)
    {
        spawnPosition = Random.insideUnitCircle.normalized;
        spawnPosition *= Random.Range(GameManager.inst.MinDist, GameManager.inst.MaxDist);
        goalAreaTransform.localPosition = new Vector3(spawnPosition.x, 3f, spawnPosition.y);

        spawnPosition = Random.insideUnitCircle.normalized;
        spawnPosition *= Random.Range(GameManager.inst.MinDist, GameManager.inst.MaxDist);
        goalTransform.localPosition = new Vector3(spawnPosition.x, 3f, spawnPosition.y);

        distanceCheck = Vector3.Distance(goalTransform.position, goalAreaTransform.position);
        //Debug.Log(touching + "inside of while");
    if (distanceCheck > minimumDistance.x && distanceCheck > minimumDistance.y && distanceCheck > minimumDistance.z)
        {
            touching = false; // Break the loop if the objects are not touching
                    //Debug.Log(touching + "inside of if");
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
            distance = Vector3.Distance(envTransform.InverseTransformPoint(goalTransform.position), envTransform.InverseTransformPoint(goalAreaTransform.position));
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
        if (other.gameObject.tag == "Object" && hasObject == false)
        {
            Debug.Log("Touched Obj");
            AddReward(+5f);

            hasObject = true;

            goalTransform.parent = objParent;
            beginDistance = Vector3.Distance(envTransform.InverseTransformPoint(goalTransform.position), envTransform.InverseTransformPoint(goalAreaTransform.position));
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
    }
}
