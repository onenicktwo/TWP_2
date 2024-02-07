using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    //public StaticDragDrop StaticDragDrop;
    public GameObject parent;
    public bool isPickedUp = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Object")
        {
            isPickedUp=true;
            Debug.Log("opsghislbngv");
            //armController.SetStuckObject(other.gameObject);
            other.gameObject.transform.parent = parent.transform;
            Debug.Log(parent.transform);
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
       



    }
    private void OnTriggerExit(Collider other)
    {
        isPickedUp=false;
    }
}

