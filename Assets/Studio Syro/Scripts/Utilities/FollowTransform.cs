using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public bool nullParent;
    public bool follow;
    public Transform target;
    public GameObject visuals;

    void Start()
    {
        if (nullParent)
        {
            transform.parent = null;
        }
    }

    
    void LateUpdate()
    {
        if (follow)
        {
            visuals.SetActive(target.gameObject.activeInHierarchy);
            transform.position = target.position;
            transform.rotation = target.rotation;
        }
    }
}
