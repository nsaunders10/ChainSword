using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoleBehaviour : MonoBehaviour
{
    bool done;

    private void OnTriggerEnter(Collider other)
    {
        if (!done)
        {
            done = true;
            transform.position = other.ClosestPoint(transform.position);
            transform.parent = other.transform;
        }
    }

    private void Update()
    {
        if(transform.parent == null)
        {
            Destroy(gameObject);
        }
    }
}
