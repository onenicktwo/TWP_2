using UnityEngine;

public class BatAdjustment : MonoBehaviour
{
    public Transform endEffector;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    void Update()
    {
        transform.position = endEffector.position + positionOffset;
        transform.rotation = Quaternion.Euler(rotationOffset) * endEffector.rotation;
    }
}
