using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRUIHolder : MonoBehaviour
{
    Transform hmd;
    public Vector3 offset;
    public float lerpSpeed = 8;
    
    void Start()
    {
        hmd = Camera.main.transform;

    }

    
    void Update()
    {
        Vector3 targetPos = hmd.position - offset;
        Quaternion targetRot = hmd.rotation;

        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);

        targetRot.x = 0;
        targetRot.z = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, lerpSpeed / 2 * Time.deltaTime);
        
    }
}
