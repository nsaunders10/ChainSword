using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class HandInteraction : MonoBehaviour
{
    public HandInteraction otherHand;
    HandBehaviour handBehaviour;
    VRRig vrRig;

    [Space]
    //Grab Object    
    public bool canGrab = true;
    public bool holdingObject;
    public bool onGrabable;

    public Grabable grabbedObject;
    [HideInInspector]
    public GameObject grabTarget;
    ConfigurableJoint holdJoint;
    GameObject physicsPoint;
    [HideInInspector]
    public Vector3 throwVelocity;
    [HideInInspector]
    public Vector3 throwAngular;

    [Space]
    //Stop hand through wall
    public Transform visuals;
    public Transform raycastPoint;
    bool onWall;

    [Space]
    //Climbing
    public bool canClimb = true;
    public bool onClimbable;
    int onClimbableCounter;
    int lastClimbCount;

    VRInput climbButton;
    Transform climbPoint;
    Transform localClimbPoint;
    public GameObject climbTarget;

    Vector3 playerStartPos;
    Vector3 climbPointStartPos;
    public bool climbing;
    bool gripReset = true;

    //Collision
    public bool handCollision;
    public Transform handCollider; 

    private void Start()
    {
        handBehaviour = GetComponent<HandBehaviour>();
        climbButton = handBehaviour.grip;
        vrRig = handBehaviour.vrRig;
    }

    private void FixedUpdate()
    {
        if (!holdingObject)
        {
            if (canClimb)
            {
                onClimbable = false;

                if (lastClimbCount != onClimbableCounter)
                {
                    lastClimbCount = onClimbableCounter;
                    onClimbable = true;
                }
                if(!climbing && !otherHand.climbing)
                {
                    gripReset = true;
                }
                if (!onClimbable)
                {
                    gripReset = true;
                }
            }
        }

        if (onWall)
        {
            vrRig.rb.AddForce(-(raycastPoint.position - vrRig.hmd.position) * Vector3.Distance(visuals.position, transform.position) * 150);
            vrRig.rb.AddForce(vrRig.transform.up * Vector3.Dot(vrRig.rb.velocity, -vrRig.transform.up) * 20);
        }
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
                if (!climbing)
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
                visuals.localRotation = Quaternion.Lerp(visuals.localRotation, Quaternion.identity, 15 * Time.deltaTime);
                onWall = false;
            }
        }
        else
        {
            visuals.localPosition = Vector3.Lerp(visuals.localPosition, Vector3.zero, 15 * Time.deltaTime);
            visuals.localRotation = Quaternion.Lerp(visuals.localRotation, Quaternion.identity, 15 * Time.deltaTime);
            onWall = false;
        }

        if (!holdingObject)
        {
            if (canClimb)
            {
                if (onClimbable)
                {
                    if (climbButton.held && !climbing)
                    {
                        if (gripReset)
                        {
                            gripReset = false;
                            if (climbTarget)
                                ClimbStart(climbTarget, transform.position);
                        }
                    }
                }

                if (climbing)
                {
                    Climb(climbButton);
                }
            }
        }


        if (canGrab)
        {
            if (!holdingObject && (handBehaviour.grip.down) && onGrabable)
            {
                Grab();
            }

            if (grabbedObject)
            {
                throwVelocity = grabbedObject.rb.velocity;
                throwAngular = grabbedObject.rb.angularVelocity;

                if (grabbedObject.lockOnHand)
                {
                    grabbedObject.transform.position = transform.position;
                    grabbedObject.transform.rotation = transform.rotation;
                }

                if (holdingObject && (!handBehaviour.grip.held))
                {
                    StopGrab();
                }               
            }
        }

        if (handCollision)
        {
            handCollider.gameObject.SetActive(true);
            handCollider.parent = handBehaviour.vrRig.transform;
            handCollider.position = handBehaviour.transform.position;
            handCollider.rotation = handBehaviour.transform.rotation;
        }
    }

    void Grab()
    {
        grabbedObject = grabTarget.GetComponent<Grabable>();

        if (FindObjectOfType<Realtime>())
        {
            if (FindObjectOfType<Realtime>().connected)
            {
                if (grabbedObject.GetComponent<RealtimeView>())
                {
                    grabbedObject.GetComponent<RealtimeView>().RequestOwnership();
                    grabbedObject.GetComponent<RealtimeTransform>().RequestOwnership();
                }
            }
        }
        if (grabbedObject)
        {
            if (grabbedObject.holdable)
            {
                grabbedObject.transform.position = handBehaviour.visuals.position;
                grabbedObject.transform.eulerAngles = handBehaviour.visuals.eulerAngles;
            }

            if (physicsPoint)
            {
                Destroy(physicsPoint);
            }

            if (!physicsPoint)
            {
                physicsPoint = new GameObject("Physics Point");
                physicsPoint.transform.parent = handBehaviour.visuals;
                physicsPoint.transform.position = handBehaviour.visuals.position;
                

                Rigidbody pointRb = physicsPoint.AddComponent<Rigidbody>();
                //pointRb.isKinematic = true;
                FixedJoint orgJoint = physicsPoint.AddComponent<FixedJoint>();
                orgJoint.connectedBody = GetComponent<Rigidbody>();

                holdJoint = physicsPoint.AddComponent<ConfigurableJoint>();
                holdJoint.connectedBody = grabbedObject.rb;

                JointDrive jointDrive = new JointDrive();
                jointDrive.positionSpring = grabbedObject.jointStiffness;
                jointDrive.positionDamper = 5;
                jointDrive.maximumForce = 3.402823e+38f;

                holdJoint.angularXDrive = jointDrive;
                holdJoint.angularYZDrive = jointDrive;
                holdJoint.xMotion = ConfigurableJointMotion.Locked;
                holdJoint.yMotion = ConfigurableJointMotion.Locked;
                holdJoint.zMotion = ConfigurableJointMotion.Locked;
                holdingObject = true;

                grabbedObject.holdingHand = handBehaviour;

                grabbedObject.transform.parent = null;
                grabbedObject.rb.isKinematic = false;

                if (grabbedObject.lockOnHand)
                {
                    grabbedObject.transform.parent = handBehaviour.visuals;
                    grabbedObject.rb.isKinematic = true;
                }
                
            }
        }
    }

    void StopGrab()
    {
        holdingObject = false;
        grabbedObject.rb.isKinematic = false;
        grabbedObject.transform.parent = null;
        ApplyForce();   
    }

    void ApplyForce()
    {
        grabbedObject.rb.angularVelocity = throwAngular;
        //grabbedObject.rb.velocity = (throwVelocity + handBehaviour.vrRig.rb.velocity);
        grabbedObject.rb.velocity =  handBehaviour.vrRig.rb.velocity;
        Rigidbody grabbedRb = grabbedObject.rb;
        Vector3 releaseVelocity = grabbedObject.rb.velocity;

        grabbedObject.holdingHand = null;
        grabbedObject = null;
        grabTarget = null;
        Destroy(physicsPoint);
        grabbedRb.velocity = releaseVelocity;

    }

    public void ClimbStart(GameObject climbTarget, Vector3 localClimbPointPosition)
    {
        climbing = true;

        if (otherHand.climbing)
        {
            otherHand.ClimbEnd();
        }
        if (!climbPoint)
        {
            climbPoint = new GameObject(handBehaviour.handedness.ToString() + " Climb Point").transform;
            climbPoint.parent = climbTarget.transform;
            climbPoint.position = localClimbPointPosition;
        }

        if (!localClimbPoint)
        {
            localClimbPoint = new GameObject(handBehaviour.handedness.ToString() + "Local Climb Point").transform;
            localClimbPoint.parent = handBehaviour.vrRig.transform;
            localClimbPoint.position = localClimbPointPosition;
        }

        playerStartPos = handBehaviour.vrRig.transform.position;
        climbPointStartPos = climbPoint.position;
        handBehaviour.vrRig.rb.isKinematic = true;
    }

    void Climb(VRInput release)
    {
        // climbOffset = transform.position - localClimbPoint.position;
        if (localClimbPoint)
        {
            Vector3 localClimbDifference = localClimbPoint.position - transform.position;
            Vector3 climbPointDifference = climbPoint.position - climbPointStartPos;
            handBehaviour.vrRig.transform.position = climbPointDifference + (playerStartPos + localClimbDifference);
        }
        onClimbable = true;
        handBehaviour.vrRig.rb.isKinematic = true;

            
        if (climbButton.up)
        {
            ClimbEnd();
        }
    }

    public void ClimbEnd()
    {
        climbing = false;
        handBehaviour.vrRig.rb.isKinematic = false;
        handBehaviour.vrRig.rb.velocity -= handBehaviour.velocity;

        if (climbPoint )
        {
            Destroy(climbPoint.gameObject);
        }

        if (localClimbPoint)
        {
            Destroy(localClimbPoint.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        EnvironmentObject otherEnviroObj = other.transform.GetComponent<EnvironmentObject>();

        if (otherEnviroObj)
        {
            if (otherEnviroObj.climbable)
            {
                climbTarget = other.gameObject;
                climbButton = handBehaviour.grip;
                onClimbableCounter++;
            }
        }

        if (other.transform.GetComponentInParent<Grabable>())
        {
            grabTarget = other.transform.GetComponentInParent<Grabable>().gameObject;
            onGrabable = true;
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        EnvironmentObject otherEnviroObj = other.transform.GetComponent<EnvironmentObject>();

        if (otherEnviroObj)
        {
            if (otherEnviroObj.climbable)
            {
             //   climbTarget = null;
            }
        }

        if (other.transform.GetComponentInParent<Grabable>())
        {
            onGrabable = false;
        }
    }

   
}
