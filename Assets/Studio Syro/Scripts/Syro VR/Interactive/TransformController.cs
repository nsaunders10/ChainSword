using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformController : MonoBehaviour
{
   public VRRig vrRig;
    public float moveForce = 30;
    public float maxVelocity = 2;
    public float jumpForce = 50;
    PlayerCollider playerCollider;
    bool turned;

    [HideInInspector]
    public Vector3 moveVector;

    HandBehaviour leftHand;
    HandBehaviour rightHand;

    int doubleJumpCount = 1;
    int currentJumps;

    public Vector3 forward;
    public Vector3 right;

    void Awake()
    {
        playerCollider = GetComponent<PlayerCollider>();
        leftHand = vrRig.leftHand;
        rightHand = vrRig.rightHand;
    }

    void Update()
    {    

        if (leftHand.stick.axis.magnitude > 0f)
        {
            if (vrRig.hmd.forward.y < 0)
            {
                if (leftHand.stick.axis.normalized.y > 0)
                {
                    forward = vrRig.hmd.forward * (leftHand.stick.axis.y - vrRig.hmd.forward.y);
                }
                if (leftHand.stick.axis.normalized.y < 0)
                {
                    forward = vrRig.hmd.forward * (leftHand.stick.axis.y + vrRig.hmd.forward.y);
                }
            }
            if (vrRig.hmd.forward.y > 0)
            {
                if (leftHand.stick.axis.normalized.y > 0)
                {
                    forward = vrRig.hmd.forward * (leftHand.stick.axis.y + vrRig.hmd.forward.y);
                }
                if (leftHand.stick.axis.normalized.y < 0)
                {
                    forward = vrRig.hmd.forward * (leftHand.stick.axis.y - vrRig.hmd.forward.y);
                }
            }

            right = vrRig.hmd.right * leftHand.stick.axis.x;

            forward.y = 0;
            right.y = 0;

            moveVector = (forward + right) * moveForce * Time.deltaTime;
        }
        else
        {
            moveVector = Vector3.zero;
        }

        if (vrRig.rb.isKinematic)
        {
            currentJumps = doubleJumpCount;
        }

        if (playerCollider.isGrounded)
        {

            transform.Translate(moveVector);

            if(vrRig.rightHand.top.down)
            {
                vrRig.rb.AddForce(new Vector3(0, jumpForce * 100, 0));                
            }
        }

        if (!playerCollider.isGrounded)
        {
            if (vrRig.rightHand.top.down && currentJumps > 0)
            {
                vrRig.rb.AddForce(new Vector3(0, jumpForce * 90, 0)); 
                currentJumps--;
            }
        }

        if (!turned && rightHand.stick.axis.x > 0.5f)
        {
            turned = true;
            vrRig.ResetPosition();
            transform.Rotate(Vector3.up, 45);
        }
        if (!turned && rightHand.stick.axis.x < -0.5f)
        {
            turned = true;
            vrRig.ResetPosition();
            transform.Rotate(Vector3.up, -45);
        }
        if (turned && Mathf.Abs(rightHand.stick.axis.x) < 0.2f)
        {
            turned = false;
        }
    }
}
