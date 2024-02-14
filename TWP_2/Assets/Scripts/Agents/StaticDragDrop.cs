using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;

public class StaticDragDrop : Agent
{
    [SerializeField] private Material winMat;
    [SerializeField] private Material failMat;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    [SerializeField] private EnvironmentManager envManager;
    [SerializeField] private Transform goalTransform;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float pickUpRange = 1.5f;

    private Vector3 originalObjectPosition; 
    private bool objectMoved = false; 
    private float previousBest;
    public GameObject hand;
    public GameObject Object;
    public GameObject EnvironmentObject;

    private bool isHoldingObject = false;
    private float holdingTimer = 0f;
    private float maxHoldTime = 10f; // Adjust as needed

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(0, 54, 0);
        Object.gameObject.transform.parent = EnvironmentObject.transform;
        Object.transform.localPosition = new Vector3(0, 3, -39.5f);

        originalObjectPosition = Object.transform.localPosition;


        previousBest = Vector3.Distance(EnvironmentObject.transform.TransformPoint(Object.transform.position), EnvironmentObject.transform.TransformPoint(goalTransform.position));

    }

    private void ResetObjects()
    {

      goalTransform.localPosition = new Vector3(Random.Range(4f, 1f), 1, Random.Range(-1.5f, 1.5f));
    


    }

    public override void CollectObservations(VectorSensor sensor)
{    
    sensor.AddObservation(transform.localPosition);
    
    sensor.AddObservation(EnvironmentObject.transform.TransformPoint(Object.transform.position));
    
    // Calculate and add distance to goal
    float distanceToGoal = Vector3.Distance(EnvironmentObject.transform.TransformPoint(Object.transform.position), EnvironmentObject.transform.TransformPoint(goalTransform.position));
    sensor.AddObservation(distanceToGoal);
}

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        float moveZ = actions.ContinuousActions[2];

        transform.localPosition += new Vector3(moveX, moveY, moveZ) * Time.deltaTime * moveSpeed;

        if (hand.GetComponent<Magnet>().isPickedUp ==true)
        {


            float currentDistanceToGoal = Vector3.Distance(EnvironmentObject.transform.TransformPoint(Object.transform.position), EnvironmentObject.transform.TransformPoint(goalTransform.position));
            if (IsMovingTowardsGoal(EnvironmentObject.transform.TransformPoint(Object.transform.position)))
            
            {
                AddReward(0.01f); 
                previousBest = currentDistanceToGoal;
            }
            else
            {
                AddReward(-0.0f); 
            }
            if (holdingTimer > maxHoldTime)
            {
                AddReward(-0.1f); 
                holdingTimer = 0f; 
            }
            
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> contActions = actionsOut.ContinuousActions;
        contActions[0] = Input.GetAxisRaw("Horizontal");
        contActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.CompareTag("Object"))
        {
            AddReward(+0.75f);
        }
        else if (Object != null && Object.GetComponent<Object>().isTouchingGoal == true)
        {
            AddReward(+1f);
            floorMeshRenderer.material = winMat;
            EndEpisode();
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            AddReward(-0.75f);
            floorMeshRenderer.material = failMat;
            EndEpisode();
        }

      
    }
 

private bool IsMovingTowardsGoal(Vector3 objectPosition)
{
    float distanceToGoal = Vector3.Distance(EnvironmentObject.transform.TransformPoint(Object.transform.position), EnvironmentObject.transform.TransformPoint(goalTransform.position));
    return distanceToGoal < previousBest; 
}

}
