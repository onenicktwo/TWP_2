using System.Collections;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    public pitcherScript pitcher; // Add this reference
    public float waitTimeBeforeDespawn = 5f;
    public float delayBeforeNewPitch = 2f;

    private void OnCollisionEnter(Collision collision)
    {
        // Check for collision with the bat instead of the floor
        if (collision.gameObject.tag == "Wall")
        {
            StartCoroutine(DelayNewPitch());
            StartCoroutine(DespawnAfterDelay());
        }
    }

    private IEnumerator DelayNewPitch()
    {
        yield return new WaitForSeconds(delayBeforeNewPitch);
        pitcher.ThrowBall(); // This now happens after a delay
    }

    private IEnumerator DespawnAfterDelay()
    {
        yield return new WaitForSeconds(waitTimeBeforeDespawn);
        gameObject.SetActive(false);
    }
}
