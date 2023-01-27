using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainSwordBehaviour : MonoBehaviour
{
    VRRig vrRig;
    PlayerController player;
    public ChainSwordBehaviour otherChainSword;
    public HandBehaviour myHand;

    public Transform hitPoint;
    public Transform tipTrans;
    public Transform hiltTrans;

    public Transform target;
    public GameObject chainHolder;
    public GameObject wingVisuals;
    public GameObject swordVisuals;
    public GameObject cursor;
    public GameObject badCursor;

    public SkinnedMeshRenderer gloveMesh;
    float[] blendValues = new float[4];

    [Space]
    public float reelForce = 10;
    public float chargeRate = 0.3f;
    public float maxLength = 8;
    public float chainLength;
    public float hangLength;
    float charge = 0;    

    [Space]
    public float liftForce = 8.6f;
    public float glideForce = 7.2f;    
    public float dragForce;   


    Transform hitObject;
    Rigidbody hookedRb;

    bool canCharge = true;
    bool charging;
    bool canPin;

    bool pinned;
    bool gliding;
    bool climbing;
    bool stabbed;
    float tossForce = 10;
    Vector3 stabbedStartEuler = new Vector3();

    public bool hasChain;

    public AudioSource sound;
    public AudioClip chainSound;
    public AudioClip swordSound;

    bool didSwordHit;

    void Start()
    {
        vrRig = myHand.vrRig;
        player = vrRig.GetComponent<PlayerController>();
        hitPoint.transform.parent = null;
    }

    private void LateUpdate()
    {
        transform.position = target.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, 30 * Time.deltaTime);

        chainHolder.transform.position = (hitPoint.transform.position + hiltTrans.position) / 2;
        chainHolder.transform.LookAt(hiltTrans.position);
        chainHolder.transform.localScale = new Vector3(1, 1, chainLength);
    }

    void Update()
    {      
        if (hasChain)
        {
            if (canCharge && !myHand.interaction.climbing)
            {
                //Chain Charging
                if (myHand.trigger.held)
                {
                    if (!charging)
                    {
                        charging = true;
                        ChangeToSword();                        
                    }
                    pinned = false;
                    
                    hitPoint.transform.position = tipTrans.position;

                    RaycastHit hit;
                    if (Physics.Raycast(hiltTrans.position, hiltTrans.TransformDirection(Vector3.up), out hit, charge))
                    {
                        hitObject = hit.transform;
                        cursor.transform.parent = null;
                        cursor.transform.position = hit.point;
                        cursor.transform.up = hit.normal;
                        badCursor.transform.position = hit.point;
                        badCursor.transform.up = hit.normal;

                        EnvironmentObject hitEnvirObj = hitObject.transform.GetComponent<EnvironmentObject>();

                        if (hit.transform.GetComponent<Grabable>())
                        {
                            canPin = true;
                            badCursor.SetActive(false);
                        }
                        if (!hitEnvirObj && !hit.transform.GetComponent<Grabable>())
                        {
                            canPin = false;
                            cursor.transform.position = hit.point;
                            cursor.transform.up = hit.normal;
                            badCursor.SetActive(true);
                        }

                        if (hitEnvirObj)
                        {
                            if (hitEnvirObj.climbable)
                            {
                                canPin = true;
                                badCursor.SetActive(false);
                            }
                            if (!hitEnvirObj.climbable)
                            {
                                canPin = false;
                                badCursor.SetActive(true);
                            }
                        }
                      
                    }
                    else
                    {
                        badCursor.SetActive(false);
                        canPin = false;
                        cursor.transform.position = hit.point;
                        cursor.transform.up = hit.normal;
                        //hitPoint.transform.position = tipTrans.transform.position;
                    }

                    if (myHand.bottom.down)
                    {
                        RetractChain();
                    }

                    RaycastHit swordTipHit;
                    if (Physics.Raycast(hiltTrans.position, hiltTrans.TransformDirection(Vector3.up), out swordTipHit, 0.48f))
                    {
                        CrystalClusterBehaviour hitCrystal = swordTipHit.transform.GetComponentInParent<CrystalClusterBehaviour>();

                        if (hitCrystal)
                        {
                            if (!didSwordHit)
                            {
                                didSwordHit = true;
                                hitCrystal.SpawnShard();
                            }
                        }
                    }
                    else
                    {
                        didSwordHit = false;
                    }
                }

                chainLength = Vector3.Distance(hitPoint.transform.position, hiltTrans.position);

                if (hookedRb)
                {
                    if (stabbed)
                    {
                        hookedRb.transform.position = tipTrans.position;
                        hookedRb.transform.parent = tipTrans;
                        //hookedRb.transform.eulerAngles = stabbedStartEuler + tipTrans.eulerAngles;
                        hookedRb.isKinematic = true;
                        ChangeToSword();

                        if (myHand.trigger.down || myHand.interaction.climbing)
                        {
                            hookedRb.isKinematic = false;
                            LaunchStabbed();
                        }
                    }

                    if (!stabbed)
                    {
                        stabbed = true;
                        if (hookedRb)
                        {
                            hookedRb.isKinematic = true;
                            stabbedStartEuler = hookedRb.transform.eulerAngles;
                        }
                    }
                }

                //Release charge and fire
                if (myHand.trigger.up)
                {
                    ShootHook();
                }              

                if (!pinned && !canPin)
                {
                    cursor.transform.parent = tipTrans;
                    cursor.transform.localPosition = new Vector3(0, charge - 0.5f, 0);
                    hitPoint.transform.position = tipTrans.transform.position;
                    cursor.transform.localRotation = Quaternion.identity;

                    ChangeToSword();

                    if (hookedRb)
                    {
                        hookedRb.angularDrag = 0.05f;
                        hookedRb = null;
                    }
                }

                if (pinned)
                {             
                    if (myHand.bottom.down)
                    {
                        RetractChain();
                    }                   
                }
            }

            if (myHand.interaction.climbing  && pinned)
            {
                RetractChain();
            }

            if (!canCharge)
            {
                cursor.transform.position = tipTrans.transform.position;
                hitPoint.transform.position = tipTrans.transform.position;

                if (myHand.trigger.down)
                {
                    canCharge = true; 
                }
            }

            //Set state bools

            climbing = myHand.interaction.climbing || myHand.otherHand.interaction.climbing;

            if (myHand.grip.held && !player.isGrounded && !climbing)
            {
                gliding = true;
            }
            else
            {
                gliding = false;
            }
            
            wingVisuals.SetActive(gliding);
           // swordVisuals.SetActive(charging);
            cursor.SetActive(charging);
            BlendStateControl();           
        }
    }

    private void FixedUpdate()
    {
        if (canCharge)
        {
            if (pinned)
            {
                cursor.transform.position = hitPoint.position;

                Vector3 chainVector = Vector3.Normalize(hitPoint.transform.position - hiltTrans.position);

                if (chainLength <= hangLength)
                {
                    hangLength = chainLength;
                }

                if (!hookedRb)
                {
                        if (chainLength > hangLength)
                        {
                            if (Vector3.Dot(vrRig.rb.velocity, chainVector) < 0.2f)
                            {
                                vrRig.rb.AddForce(chainVector * Vector3.Dot(vrRig.rb.velocity * 1.5f, -chainVector) * 100);
                                vrRig.rb.AddForce(chainVector * 60);
                            }
                        }

                    if (myHand.grip.held)
                    {
                        vrRig.rb.AddForce(chainVector * reelForce);
                        vrRig.rb.AddForce(transform.up * reelForce);
                    }
                }
            }

            if (charging)
            {
                if (charge <= maxLength)
                {
                    charge += chargeRate;
                }
            }
            if (!charging)
            {
                charge = 0;
            }
        }
        //Glide Control
        if (myHand.grip.held && !player.isGrounded)
        {

            Vector3 liftVector = -transform.forward * Vector3.Dot(vrRig.rb.velocity, transform.forward) * liftForce;
            vrRig.rb.AddForce(liftVector);

            Vector3 glideVector = transform.up * Mathf.Abs(Vector3.Dot(vrRig.rb.velocity, transform.forward)) * glideForce;
            vrRig.rb.AddForce(glideVector);

        }

        wingVisuals.SetActive(myHand.grip.held);
    }


    void LaunchStabbed()
    {
        hookedRb.transform.parent = null;
        hookedRb.AddForce(vrRig.rb.velocity);
        hookedRb.AddForce(tipTrans.up * tossForce, ForceMode.Impulse);
        hookedRb = null;
        stabbed = false;
        RetractChain();
    }

    void BlendStateControl()
    {
        float blendSpeed = 15;
        if (pinned)
        {
            blendValues[0] = Mathf.Lerp(blendValues[0], 100, Time.deltaTime * blendSpeed);
            blendValues[1] = Mathf.Lerp(blendValues[1], 0, Time.deltaTime * blendSpeed);
            blendValues[2] = Mathf.Lerp(blendValues[2], 0, Time.deltaTime * blendSpeed);
            blendValues[3] = Mathf.Lerp(blendValues[3], 0, Time.deltaTime * blendSpeed);
        }

        if (gliding)
        {
            blendValues[0] = Mathf.Lerp(blendValues[0], 0, Time.deltaTime * blendSpeed);
            blendValues[1] = Mathf.Lerp(blendValues[1], 0, Time.deltaTime * blendSpeed);
            blendValues[2] = Mathf.Lerp(blendValues[2], 100, Time.deltaTime * blendSpeed);
            blendValues[3] = Mathf.Lerp(blendValues[3], 0, Time.deltaTime * blendSpeed);
        }

        if (climbing)
        {
            blendValues[0] = Mathf.Lerp(blendValues[0], 0, Time.deltaTime * blendSpeed);
            blendValues[1] = Mathf.Lerp(blendValues[1], 0, Time.deltaTime * blendSpeed);
            blendValues[2] = Mathf.Lerp(blendValues[2], 0, Time.deltaTime * blendSpeed);
            blendValues[3] = Mathf.Lerp(blendValues[3], 100, Time.deltaTime * blendSpeed);
        }

        if (charging)
        {
            blendValues[0] = Mathf.Lerp(blendValues[0], 0, Time.deltaTime * blendSpeed);
            blendValues[1] = Mathf.Lerp(blendValues[1], 100, Time.deltaTime * blendSpeed);
            blendValues[2] = Mathf.Lerp(blendValues[2], 0, Time.deltaTime * blendSpeed);
            blendValues[3] = Mathf.Lerp(blendValues[3], 0, Time.deltaTime * blendSpeed);
        }

        if (!climbing && !gliding && !pinned && !charging)
        {
            blendValues[0] = Mathf.Lerp(blendValues[0], 0, Time.deltaTime * blendSpeed);
            blendValues[1] = Mathf.Lerp(blendValues[1], 0, Time.deltaTime * blendSpeed);
            blendValues[2] = Mathf.Lerp(blendValues[2], 0, Time.deltaTime * blendSpeed);
            blendValues[3] = Mathf.Lerp(blendValues[3], 0, Time.deltaTime * blendSpeed);
            swordVisuals.SetActive(false);
        }

        gloveMesh.SetBlendShapeWeight(0, blendValues[0]);
        gloveMesh.SetBlendShapeWeight(1, blendValues[1]);
        gloveMesh.SetBlendShapeWeight(2, blendValues[2]);
        gloveMesh.SetBlendShapeWeight(3, blendValues[3]);

    }

    void RetractChain()
    {
        if (canCharge)
        {
            canCharge = false;
            charging = false;
            pinned = false;
            canPin = false;
            ChangeToSword();
            return;
        }

        if (!canCharge)
        {
            canCharge = true;          
        }
    }

    void ShootHook()
    {
        charging = false;
        badCursor.SetActive(false);
        if (canPin)
        {
            pinned = true;            
            hitPoint.parent = hitObject;
            hitPoint.transform.position = cursor.transform.position;
            hitPoint.transform.rotation = cursor.transform.rotation;
            hookedRb = hitObject.GetComponentInParent<Rigidbody>();           
            ChangeToChain();
        }
    }

    void ChangeToSword()
    {
        chainHolder.SetActive(false);
        cursor.SetActive(true);
        swordVisuals.SetActive(true);
        hitPoint.gameObject.SetActive(false);
    }

    void ChangeToChain()
    {
        chainHolder.SetActive(true);
        //cursor.SetActive(false);
        badCursor.SetActive(false);
        swordVisuals.SetActive(false);
        hitPoint.gameObject.SetActive(true);
    }

   
    public void GetChain()
    {
        hasChain = true;
    }
}
