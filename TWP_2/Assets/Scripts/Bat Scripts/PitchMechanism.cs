using UnityEngine;

public class PitchMechanism : MonoBehaviour
{
    public GameObject baseballPrefab;
    public Transform pitchPoint;
    public BaseballAgent baseballAgent;

    public void PitchBall(string pitchType)
    {
        GameObject ball = Instantiate(baseballPrefab, pitchPoint.position, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();

        float speedVariation = Random.Range(-2f, 2f);
        float angleVariation = Random.Range(-5f, 5f);

        switch(pitchType)
        {
            case "fastball":
                rb.velocity = new Vector3(0, angleVariation, -20 + speedVariation);
                break;
            case "curveball":
                rb.velocity = new Vector3(0, angleVariation, -15 + speedVariation);
                break;
            case "slider":
                rb.velocity = new Vector3(-5, angleVariation, -15 + speedVariation);
                break;
        }

    }
}
