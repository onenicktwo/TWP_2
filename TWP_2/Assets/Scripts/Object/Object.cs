using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    private float timer = 0.0f;
    public float timerDuration = 3.0f;  // Adjust the duration as needed
    public bool isTouchingGoal = false; 
    public GameObject goal;
    public StaticDragDrop staticDragDrop;

    
    private void Start()
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
                Debug.Log("End");
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
      if (other.gameObject == goal)
        {
            isTouchingGoal = true;
        }

    }
}
