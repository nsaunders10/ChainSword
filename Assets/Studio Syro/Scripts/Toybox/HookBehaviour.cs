using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class HookBehaviour : MonoBehaviour
{
    Rigidbody rb;
    public GameObject myGrapplingGun;
    public Transform hookSpot;
    public bool hitGrabable;
    public bool didHit;
    public Transform hookVisuals;
    public RealtimeView realtimeView;

    private void Start()
    {
        hookVisuals.parent = null;
    }

    private void Update()
    {
        if (realtimeView.isOwnedLocallySelf)
        {
            hookVisuals.position = transform.position;
            hookVisuals.rotation = transform.rotation;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject != myGrapplingGun)
        {
            rb = GetComponent<Rigidbody>();
            transform.parent = collision.transform;
            Destroy(rb);
            didHit = true;
        }
    }

    public void ConnectHook(Transform parent, Vector3 position)
    {
        rb = GetComponent<Rigidbody>();
        transform.position = position;
        transform.parent = parent;
        if (rb)
        {
            Destroy(rb);
        }
        didHit = true;
    }
}
