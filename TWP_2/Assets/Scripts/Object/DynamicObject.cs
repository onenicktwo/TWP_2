using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : MonoBehaviour
{
    private float timer = 0.0f;
    public float timerDuration = 3.0f;  // Adjust the duration as needed
    public PicknPlace pnp;

    /*
    public void RestartEpisode()
    {
        timer = timerDuration;
    }
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
    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Wall" && pnp.hasObject) || other.gameObject.tag == "ArmParts" || other.gameObject.tag == "Neck")
        {
            Debug.Log("Object Clip");
            pnp.CollisionFail();
        }
        if (other.gameObject.tag == "GoalArea")
        {
            pnp.GoalAreaReached();
        }
    }
}
