using System.Collections;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    public pitcherScript pitcher;
    public float waitTimeBeforeDespawn = 5f;
    public float delayBeforeNewPitch = 2f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            StartCoroutine(DelayNewPitch());
            StartCoroutine(DespawnAfterDelay());
        }
        else if (collision.gameObject.tag == "Bat")
        {
            StartCoroutine(DelayNewPitch());
            StartCoroutine(DespawnAfterDelay());
        }
    }

    private IEnumerator DelayNewPitch()
    {
        yield return new WaitForSeconds(delayBeforeNewPitch);
        pitcher.ThrowBall();
    }

    private IEnumerator DespawnAfterDelay()
    {
        yield return new WaitForSeconds(waitTimeBeforeDespawn);
        gameObject.SetActive(false);
    }
}
