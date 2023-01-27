using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsLookAt : MonoBehaviour
{
    private readonly VectorPid angularVelocityController = new VectorPid(33.7766f, 0, 0.2553191f);
    private readonly VectorPid headingController = new VectorPid(9.244681f, 0, 0.06382979f);

    public Transform target;
    Rigidbody rb;
    public float recenterForce;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Rotate();
    }

    public class VectorPid
    {
        public float pFactor, iFactor, dFactor;/// <summary>
                                               /// 
                                               /// </summary>

        private Vector3 integral;
        private Vector3 lastError;

        public VectorPid(float pFactor, float iFactor, float dFactor)
        {
            this.pFactor = pFactor;
            this.iFactor = iFactor;
            this.dFactor = dFactor;
        }

        public Vector3 Update(Vector3 currentError, float timeFrame)
        {
            integral += currentError * timeFrame;
            var deriv = (currentError - lastError) / timeFrame;
            lastError = currentError;
            return currentError * pFactor
                + integral * iFactor
                + deriv * dFactor;
        }
    }

    void Rotate()
    {

        var angularVelocityError = rb.angularVelocity * -1;
        Debug.DrawRay(transform.position, rb.angularVelocity * 10, Color.black);

        var angularVelocityCorrection = angularVelocityController.Update(angularVelocityError, Time.deltaTime);
        Debug.DrawRay(transform.position, angularVelocityCorrection, Color.green);

        rb.AddTorque(angularVelocityCorrection);

        var desiredHeading = target.position - transform.position;
        Debug.DrawRay(transform.position, desiredHeading, Color.magenta);

        var currentHeading = transform.up;
        Debug.DrawRay(transform.position, currentHeading * 15, Color.blue);

        var headingError = Vector3.Cross(currentHeading, desiredHeading);
        var headingCorrection = headingController.Update(headingError, Time.deltaTime);

        rb.AddTorque(headingCorrection * recenterForce);
    }


}
