using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;


public class GunBehaviour : MonoBehaviour
{
    VRRig vrRig;
    public Grabable grabable;
    public RealtimeView realtimeView;

    public Vector3Sync hitPointSync;
    public IntSync hitCountSync;

    public GameObject gunVisuals;
    public Transform gunBarrel;

    public Transform hitPreview;
    public GameObject bulletHitPrefab;
    public float impactForce = 500;

    public int localHitCount;

    void Start()
    {
        grabable = GetComponent<Grabable>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(gunBarrel.position, gunBarrel.forward);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            hitPreview.position = hit.point;

            if (grabable.holdingHand != null)
            {
                if (grabable.holdingHand.trigger.down)
                {                   

                    if (!realtimeView)
                    {
                        GameObject newBulletHole = Instantiate(bulletHitPrefab, hit.point, Quaternion.identity);
                        newBulletHole.transform.forward = hit.normal;
                    }

                    if (realtimeView)
                    {
                        if (realtimeView.realtime.connected)
                        {
                            hitCountSync.SetValue(hitCountSync.currentValue++);
                            hitPointSync.SetValue(hit.point);

                            if (hit.collider.GetComponentInParent<RealtimeView>())
                            {
                                hit.collider.GetComponentInParent<RealtimeView>().RequestOwnership();
                                hit.collider.GetComponentInParent<RealtimeTransform>().RequestOwnership();
                            }
                        }
                    }

                    if (hit.rigidbody)
                    {
                        hit.rigidbody.AddForceAtPosition(gunBarrel.forward * impactForce, hit.point);
                    }
                    gunVisuals.transform.localRotation = Quaternion.Euler(new Vector3(-5 * (impactForce / 100), 0, 0));
                    if (hit.collider.GetComponent<VRButton>())
                    {
                        if (hit.collider.GetComponent<VRButton>().OnClickDown != null)
                        {
                            hit.collider.GetComponent<VRButton>().OnClickDown.Invoke();
                        }
                        else
                        {
                            hit.collider.GetComponent<VRButton>().OnClickUp.Invoke();
                        }
                        
                    }
                }
            }
        }
        else
        {
            hitPreview.position = new Vector3(0, -100, 0);
        }
        if (hitCountSync)
        {
            if (hitCountSync.currentValue != localHitCount)
            {
                localHitCount = hitCountSync.currentValue;

                GameObject newBulletHole = Instantiate(bulletHitPrefab, hit.point, Quaternion.identity);
                newBulletHole.transform.forward = hit.normal;
            }
        }
        gunVisuals.transform.localRotation = Quaternion.Lerp(gunVisuals.transform.localRotation, Quaternion.identity, 8 * Time.deltaTime);

    }
}
