using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : MonoBehaviour
{
    private float timer = 0.0f;
    public float timerDuration = 3.0f;  // Adjust the duration as needed
    public PicknPlace pnp;
    public bool inFloor = false;

    
    public void RestartEpisode()
    {
        timer = timerDuration;
        inFloor = false;
    }
    /*
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))  // Adjust the tag as needed
        {
            timer -= Time.deltaTime;

            // Check if the timer has reached or gone below zero
            if (timer <= 0)
            {
                dynPnP.CollisionFail();
            }
        }
    }
    */

    private void Update()
    {
        if (inFloor)
        {
            // timer -= Time.deltaTime;
            pnp.InFloor();

            // Check if the timer has reached or gone below zero
            /*
            if (timer <= 0)
            {
                pnp.CollisionFail();
            }
            */
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Wall" && pnp.hasObject) || other.gameObject.tag == "ArmParts" || other.gameObject.tag == "Neck")
        {
            inFloor = true;
        }
        if (other.gameObject.tag == "GoalArea")
        {
            pnp.GoalAreaReached();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.tag == "Wall" && pnp.hasObject) || other.gameObject.tag == "ArmParts" || other.gameObject.tag == "Neck")
        {
            inFloor = false;
        }
    }
}
