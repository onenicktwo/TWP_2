using System.Collections;
using UnityEngine;

public class BatCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Baseball"))
        {
            var rb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 forceDirection = collision.contacts[0].point - transform.position;
            forceDirection.y = 0;
            rb.AddForce(forceDirection.normalized * 500);
        }
    }
}
