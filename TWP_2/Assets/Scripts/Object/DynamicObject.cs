using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObject : MonoBehaviour
{
    private float timer = 0.0f;
    public float timerDuration = 3.0f;  // Adjust the duration as needed
    public DynamicPicknPlace dynPnP;

    public void RestartEpisode()
    {
        timer = timerDuration;
        Vector3 spawnPosition = Random.insideUnitCircle.normalized;
        spawnPosition *= Random.Range(GameManager.inst.MinDist, GameManager.inst.MaxDist);
        transform.localPosition = new Vector3(spawnPosition.x, 3f, spawnPosition.y);
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
}
