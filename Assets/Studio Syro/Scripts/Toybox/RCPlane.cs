using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCPlane : MonoBehaviour
{
    Rigidbody rb;
    Vector3 currentVelocity;
    public float glideForce;
    public float thrustForce;
    public float liftForce;

    VRRig vrRig;

    void Start()
    {
        vrRig = FindObjectOfType<VRRig>();
        rb = GetComponent<Rigidbody>();
    }

    
    void FixedUpdate()
    {
        currentVelocity = rb.velocity;
        rb.angularVelocity = Vector3.zero;

        if (currentVelocity.y < 0)
        {
            rb.AddForce(transform.forward * -currentVelocity.y * glideForce);           
        }

        rb.AddForce(transform.forward * thrustForce * vrRig.leftHand.stick.axis.y);
        //rb.AddForce(transform.up * liftForce * rb.velocity.magnitude);
    }
}
