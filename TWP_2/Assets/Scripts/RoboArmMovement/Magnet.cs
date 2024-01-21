using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public ArmController armController;
    public GameObject parent;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Object")
        {
            armController.SetStuckObject(other.gameObject);
            other.gameObject.transform.parent = parent.transform;
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
