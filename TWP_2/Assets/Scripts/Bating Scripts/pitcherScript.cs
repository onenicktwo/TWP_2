using System.Collections;
using UnityEngine;

[System.Serializable]
public class PitchType
{
    public string name;
    public float speed;
    public Vector3 direction;
    public float spin;
}

public class pitcherScript : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab;
    [SerializeField]
    private Transform throwPoint;
    [SerializeField]
    private Transform batPosition;

    private PitchType[] pitchTypes;
    private float pitchTimer;
    public float timeBetweenPitches = 3f;

    void Start()
    {
        InitializePitchTypes();
        pitchTimer = timeBetweenPitches;
    }

    void Update()
    {
        pitchTimer -= Time.deltaTime;

        if (pitchTimer <= 0f)
        {
            ThrowBall();
            pitchTimer = timeBetweenPitches;
        }
    }

    public void ThrowBall()
    {
        GameObject ball = Instantiate(ballPrefab, throwPoint.position, Quaternion.identity);
        BallBehavior ballBehavior = ball.GetComponent<BallBehavior>();

        if (ballBehavior != null)
        {
            ballBehavior.pitcher = this;
        }

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        PitchType selectedPitch = pitchTypes[Random.Range(0, pitchTypes.Length)];
        Vector3 throwDirection = (batPosition.position - throwPoint.position).normalized;

        
        rb.AddForce(throwDirection * selectedPitch.speed, ForceMode.Impulse);

        // Apply spin to the ball based on the selected pitch type
        Vector3 spinAxis = Vector3.Cross(throwDirection, Vector3.up).normalized;
        rb.AddTorque(spinAxis * selectedPitch.spin, ForceMode.Impulse);
    }

    private void InitializePitchTypes()
    {
        pitchTypes = new PitchType[]
        {
            new PitchType() { name = "Fastball", speed = 400f, direction = Vector3.forward, spin = 0f },
            new PitchType() { name = "Curveball", speed = 320f, direction = new Vector3(0.5f, 0, 1).normalized, spin = 1f },
            new PitchType() { name = "Slider", speed = 340f, direction = new Vector3(-0.5f, 0, 1).normalized, spin = -1f }
        };
    }
}