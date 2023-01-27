using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRRig : MonoBehaviour
{
    public Rigidbody rb;

    [Space]
    public Transform trackedObjects;
    public Transform hmd;    
    public HandBehaviour leftHand;
    public HandBehaviour rightHand;

    private void Update()
    {
        if (leftHand.stick.down)
        {
            //ResetPosition();
        }

        if (transform.position.y < -100)
        {
           // transform.position = new Vector3(0, 1, 0);
        }
    }

    public void ResetPosition()
    {
        trackedObjects.parent = null;

        Vector3 targetPos = hmd.position;
        targetPos.y = transform.position.y;
        transform.position = targetPos;

        Quaternion targetRot = Quaternion.identity;
        targetRot.y = hmd.rotation.y;
        transform.rotation = targetRot;

        trackedObjects.parent = transform;
        trackedObjects.SetAsFirstSibling();
    }
}
