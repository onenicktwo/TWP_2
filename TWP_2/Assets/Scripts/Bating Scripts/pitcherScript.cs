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
    private Transform batPosition; // Reference to the bat's position

    private PitchType[] pitchTypes;
    private float pitchTimer; // Timer to control pitch frequency
    public float timeBetweenPitches = 3f; // Time in seconds between pitches

    void Start()
    {
        InitializePitchTypes();
        pitchTimer = timeBetweenPitches; // Initialize the pitch timer
    }

    void Update()
    {
        pitchTimer -= Time.deltaTime; // Decrement the timer each frame

        // Check if it's time to throw another ball
        if (pitchTimer <= 0f)
        {
            ThrowBall();
            pitchTimer = timeBetweenPitches; // Reset the timer
        }
    }

    public void ThrowBall()
    {
        GameObject ball = Instantiate(ballPrefab, throwPoint.position, Quaternion.identity);
        BallBehavior ballBehavior = ball.GetComponent<BallBehavior>();

        if (ballBehavior != null)
        {
            ballBehavior.pitcher = this; // Set the pitcher reference
        }
        else
        {
            Debug.LogError("BallBehavior script not found on the ball prefab!");
        }

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        PitchType selectedPitch = pitchTypes[Random.Range(0, pitchTypes.Length)];
        Vector3 throwDirection = (batPosition.position - throwPoint.position).normalized; // Calculate direction towards the bat

        Debug.Log($"Throw Direction: {throwDirection}, Speed: {selectedPitch.speed}");
        rb.AddForce(throwDirection * selectedPitch.speed, ForceMode.Impulse);
    }

    private void InitializePitchTypes()
    {
        pitchTypes = new PitchType[]
        {
            new PitchType() { name = "Fastball", speed = 120f, direction = Vector3.forward, spin = 0f },
            new PitchType() { name = "Curveball", speed = 105f, direction = new Vector3(0.5f, 0, 1).normalized, spin = 1f },
            new PitchType() { name = "Slider", speed = 100f, direction = new Vector3(-0.5f, 0, 1).normalized, spin = -1f }
        };
    }
}