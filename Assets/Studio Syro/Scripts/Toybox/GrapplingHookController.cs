using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class GrapplingHookController : MonoBehaviour
{
    VRRig vrRig;
    Grabable grabable;
    public RealtimeView realtimeView;
    public GameObject gunVisuals;

    public GameObject hook;
    HookBehaviour hookBehaviour;
    Rigidbody hookRb;
    public float launchForce = 5000;
    public bool launched;
    bool canShoot;
    Vector3 localStartPos;
    public float reelForce = 5;
    RealtimeView hookView;
    RealtimeTransform hookRealTrans;
    public bool playerForce;
    public Transform barrel;

    VRInput activationTrigger;
    Rigidbody rb;

    void Start()
    {
        grabable = GetComponent<Grabable>();        
        localStartPos = hook.transform.localPosition;        
        hookBehaviour = hook.GetComponent<HookBehaviour>();
        hookView = hookBehaviour.hookVisuals.GetComponent<RealtimeView>();
        hookRealTrans = hookBehaviour.hookVisuals.GetComponent<RealtimeTransform>();
        rb = GetComponent<Rigidbody>();
        
    }

    void Update()
    {

        if (realtimeView)
        {
            if (realtimeView.isOwnedLocallySelf)
            {
                if(hookRb)
                hookRb.useGravity = rb.useGravity;
                hookView.RequestOwnership();
                hookRealTrans.RequestOwnership();
                if(grabable.holdingHand)
                activationTrigger = grabable.holdingHand.trigger;
                hookBehaviour.hookVisuals.gameObject.SetActive(true);
            }
            if (realtimeView.isUnownedInHierarchy)
            {
                hook.transform.parent = null;
                hookBehaviour.hookVisuals.gameObject.SetActive(false);
            }

        }

        if (grabable.holdingHand != null)
        {
            if (activationTrigger.up)
                canShoot = true;

            if (activationTrigger.down && canShoot)
            {
                if (!launched)
                {
                    launched = true;
                    hook.transform.parent = null;
                    hookRb = hook.AddComponent<Rigidbody>();
                    hookRb.mass = 3;
                    hookRb.interpolation = RigidbodyInterpolation.Interpolate;
                    hookRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    if (playerForce)
                    {
                        hookRb.AddForce((hookRb.transform.forward * launchForce) - grabable.holdingHand.vrRig.rb.velocity);
                        //vrRig.rb.AddForce(-hookRb.transform.forward * launchForce * 5);
                    }

                    if (!playerForce)
                        hookRb.AddForce((hookRb.transform.forward * launchForce));
                    gunVisuals.transform.localRotation = Quaternion.Euler(new Vector3(-35, 0, 0));
                }
            }

            if (launched && canShoot)
            {
                if (grabable.holdingHand.bottom.down)
                {
                    if (hookRb)
                    {
                        Destroy(hookRb);
                        Destroy(hook.GetComponent<SpringJoint>());
                    }
                    hookBehaviour.didHit = false;
                    hook.transform.parent = transform;
                    hook.transform.localPosition = localStartPos;
                    hook.transform.localRotation = Quaternion.identity;
                    launched = false;
                }
            }

            if (launched)
            {
                RaycastHit hit;
                Vector3 direction = barrel.position - hook.transform.position;
                Ray ray = new Ray(barrel.position, -direction * Vector3.Distance(barrel.position, hook.transform.position));
                int layerMask = 1 << 7;
                layerMask = ~layerMask;

                Debug.DrawRay(ray.origin, ray.direction * Vector3.Distance(barrel.position, hook.transform.position), Color.green);

                if (Physics.Raycast(ray.origin, ray.direction,  out hit, Vector3.Distance(barrel.position, hook.transform.position), layerMask))
                {
                    if (hit.collider.gameObject != hook && !hit.collider.GetComponentInParent<GrapplingHookController>() && !hit.collider.GetComponentInParent<VRRig>())
                    {
                        hookBehaviour.ConnectHook(hit.transform, hit.point);
                    }
                }
            }

            gunVisuals.transform.localRotation = Quaternion.Lerp(gunVisuals.transform.localRotation, Quaternion.identity, 8 * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (grabable.holdingHand != null)
            if (launched && activationTrigger.held && !hookRb)
            {
                grabable.holdingHand.vrRig.rb.AddForce(transform.forward * reelForce / 2);
                grabable.holdingHand.vrRig.rb.AddForce(Vector3.Normalize(hook.transform.position - transform.position) * reelForce);
            }
    }
}
