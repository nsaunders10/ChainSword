using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Grabable : MonoBehaviour
{
    public Rigidbody rb;
    public float jointStiffness = 60;
    [HideInInspector]
    public HandBehaviour holdingHand;
    public bool holdable;
    public bool lockOnHand;


    private void Update()
    {
        if (rb.isKinematic)
        {
            rb.interpolation = RigidbodyInterpolation.None;
        }

        if (!rb.isKinematic)
        {
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }
}
