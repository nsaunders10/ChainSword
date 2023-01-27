using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    VRRig vrRig;
    Transform hmd;
    HandBehaviour leftHand;
    HandBehaviour rightHand;

    public bool canTurn = true;

    [Space]
    public float moveForce = 100;
    public float groundDrag = 5;
    public bool isGrounded;

    [Space]
    public float airMoveForce = 20;
    public float airDrag = 0.3f;

    [Space]
    public float jumpForce = 20;
    public float jumpForwardForce = 10;
    public int doubleJumpCount = 1;
    public float gravityForce = 80;

    float climbLerpSpeed = 20;
    
    
    public int currentJumps;
    bool turned;   

    [HideInInspector]
    public Vector3 moveVector;
    [HideInInspector]
    public Vector3 forward;
    [HideInInspector]
    public Vector3 right;    
    
    

    void Awake()
    {
        vrRig = GetComponent<VRRig>();
        hmd = vrRig.hmd;
        leftHand = vrRig.leftHand;
        rightHand = vrRig.rightHand;
    }

    Vector3 CaluculateMoveForce()
    {
        Vector3 targetForce;

        if (leftHand.stick.axis.magnitude > 0.01f)
        {
            if (vrRig.hmd.forward.y < 0)
            {
                if (leftHand.stick.axis.y > 0)
                {
                    forward = vrRig.hmd.forward * (leftHand.stick.axis.y - vrRig.hmd.forward.y);
                }
                if (leftHand.stick.axis.y < 0)
                {
                    forward = vrRig.hmd.forward * (leftHand.stick.axis.y + vrRig.hmd.forward.y);
                }
            }
            if (vrRig.hmd.forward.y > 0)
            {
                if (leftHand.stick.axis.y > 0)
                {
                    forward = vrRig.hmd.forward * (leftHand.stick.axis.y + vrRig.hmd.forward.y);
                }
                if (leftHand.stick.axis.y < 0)
                {
                    forward = vrRig.hmd.forward * (leftHand.stick.axis.y - vrRig.hmd.forward.y);
                }
            }

            right = vrRig.hmd.right * leftHand.stick.axis.x;

            forward.y = 0;
            right.y = 0;

            targetForce = (forward + right);
            return targetForce;

        }
        else
        {
            targetForce = Vector3.zero;
            return targetForce;
        }

    }

    void CheckForTurn()
    {
        if (!turned && rightHand.stick.axis.x > 0.5f)
        {
            turned = true;
            //vrRig.ResetPosition();
            transform.RotateAround(hmd.position, Vector3.up, 45);
        }
        if (!turned && rightHand.stick.axis.x < -0.5f)
        {
            turned = true;
           // vrRig.ResetPosition();
            transform.RotateAround(hmd.position, Vector3.up, -45);
        }
        if (turned && Mathf.Abs(rightHand.stick.axis.x) < 0.2f)
        {
            turned = false;
        }
    }

    void Update()
    {

        RaycastHit hit;
        float rayOffset = 0.1f;
        Ray ray = new Ray(hmd.position, Vector3.down * (hmd.localPosition.y));
        Debug.DrawRay(hmd.position, ray.direction * (hmd.localPosition.y + rayOffset), Color.green);

        if (Physics.Raycast(ray, out hit, hmd.localPosition.y + rayOffset))
        {
            EnvironmentObject hitEnviroObj = hit.collider.GetComponent<EnvironmentObject>();

            if (hitEnviroObj)
            {
                if (hitEnviroObj.walkable || hit.collider.GetComponent<Walkable>())
                {
                    Vector3 targPos = vrRig.rb.position;
                    targPos.y = Mathf.Lerp(targPos.y, hit.point.y + 0.005f, climbLerpSpeed * Time.deltaTime);

                    if (targPos.y < hit.point.y)
                    {
                        vrRig.rb.position = targPos;
                        Vector3 targVel = vrRig.rb.velocity;
                        targVel.y = 0;
                        vrRig.rb.velocity = targVel;
                    }

                    vrRig.rb.drag = groundDrag;

                    isGrounded = true;
                }
                else
                {
                    isGrounded = false;
                }
            }
        }
        else
        {
            if (isGrounded)
            {
                vrRig.rb.drag = airDrag;
                isGrounded = false;
            }
        }

        if (canTurn)
        {
            CheckForTurn();
        }
        moveVector = CaluculateMoveForce();

        if (vrRig.rb.isKinematic)
        {
            currentJumps = doubleJumpCount;
        }

        
        if (isGrounded)
        {
            currentJumps = doubleJumpCount;

            if(vrRig.rightHand.top.down)
            {
                Jump();
            }
        }

        
        if (!isGrounded)
        {
            if (vrRig.rightHand.top.down && currentJumps > 0)
            {
                if(vrRig.rb.velocity.y < 0)
                {
                    Vector3 targetVel = vrRig.rb.velocity;
                    targetVel.y = vrRig.rb.velocity.y / 10;
                    vrRig.rb.velocity = targetVel;
                }
                Jump();                
            }
        }
       
    }

    public void Jump()
    {
        vrRig.rb.AddForce((transform.up * jumpForce), ForceMode.Impulse);
        vrRig.rb.AddForce((hmd.forward - hmd.up) * jumpForwardForce, ForceMode.Impulse);
        if(!isGrounded)
        currentJumps--;
    }

    private void FixedUpdate()
    {
        if (vrRig.rb.isKinematic)
        {
            currentJumps = doubleJumpCount;
        }

        //Is Grounded
        if (isGrounded)
        {
            vrRig.rb.AddForce(moveVector * moveForce);
        }

        //In Air
        if (!isGrounded)
        {
            vrRig.rb.AddForce(new Vector3(0, -gravityForce, 0));
            vrRig.rb.AddForce(moveVector * airMoveForce);
        }
    }
}
