using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour
{

    VRRig vrRig;
    HandBehaviour handBehaviour;

    public Transform visuals;
    public Transform raycastPoint;

    public Vector3 collisionPoint;
    public Vector3 offset;
    public Quaternion collisionRotation;

    bool onWall;

    void Start()
    {
        handBehaviour = GetComponent<HandBehaviour>();
        vrRig = handBehaviour.vrRig;

    }


    void Update()
    {
        float distanceToHMD = Vector3.Distance(vrRig.hmd.position, raycastPoint.position);
        Ray ray = new Ray(vrRig.hmd.position, raycastPoint.position - vrRig.hmd.position);
        //Ray ray = new Ray(raycastPoint.position, vrRig.hmd.position - raycastPoint.position) ;
        Debug.DrawRay(ray.origin, ray.direction * distanceToHMD, Color.blue);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanceToHMD))
        {
            if (hit.collider.gameObject.layer == 6)
            {
                visuals.position = Vector3.Lerp(visuals.position, hit.point, 15 * Time.deltaTime);
                //visuals.forward = Vector3.Lerp(visuals.forward, -hit.normal, 15 * Time.deltaTime);
                if (!onWall)
                {
                    if (!hit.transform.GetComponent<Grabable>())
                    {
                        onWall = true;
                        vrRig.rb.velocity = vrRig.rb.velocity / 2;
                    }
                }
            }
            else
            {
                visuals.localPosition = Vector3.Lerp(visuals.localPosition, Vector3.zero, 15 * Time.deltaTime);
                //visuals.localRotation = Quaternion.Lerp(visuals.localRotation, Quaternion.identity, 15 * Time.deltaTime);
                onWall = false;
            }
        }
        else
        {
            visuals.localPosition = Vector3.Lerp(visuals.localPosition, Vector3.zero, 15 * Time.deltaTime);
            visuals.localRotation = Quaternion.Lerp(visuals.localRotation, Quaternion.identity, 15 * Time.deltaTime);
            onWall = false;
        }
    }
    private void FixedUpdate()
    {
        if (onWall)
        {
            vrRig.rb.AddForce(-(raycastPoint.position - vrRig.hmd.position) * Vector3.Distance(visuals.position, transform.position) * 150);
            vrRig.rb.AddForce(vrRig.transform.up * Vector3.Dot(vrRig.rb.velocity, -vrRig.transform.up) * 50);
        }
       // vrRig.rb.velocity += (-visuals.forward * Vector3.Distance(visuals.position, transform.position) * 0.5f);
    }
}
