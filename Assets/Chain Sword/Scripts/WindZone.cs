using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    public float force = 150;
    bool addForce;
    VRRig vrRig;

    void Start()
    {
        vrRig = FindObjectOfType<VRRig>();
    }

    
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponentInParent<Rigidbody>();

        if(rb)
        {
            rb.AddForce(transform.up * force);
            Vector3 centerPos = transform.position;
            centerPos.y = rb.position.y;
            Vector3 centerForce = centerPos - rb.position;
           // rb.AddForce(centerForce * force);

        }
    }
}
