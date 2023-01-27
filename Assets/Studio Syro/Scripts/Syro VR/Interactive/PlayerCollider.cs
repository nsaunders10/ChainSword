using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    VRRig vrRig;
    PlayerController playerController;

    public Transform hmd;
    [HideInInspector]
    public bool isGrounded;
    public float gravityForce = 35;
    public float lerpSpeed = 20;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();

    }

    void Update()
    {

        RaycastHit hit;
        float rayOffset = 0f;
        Ray ray = new Ray(hmd.position, Vector3.down * (hmd.localPosition.y));
        Debug.DrawRay(hmd.position, ray.direction * (hmd.localPosition.y + rayOffset), Color.green);

        if (Physics.Raycast(ray, out hit, hmd.localPosition.y + rayOffset))
        {
            if (hit.collider.GetComponent<Walkable>())
            {
                Vector3 targPos = vrRig.rb.position;
                targPos.y = Mathf.Lerp(targPos.y, hit.point.y + 0.01f, lerpSpeed * Time.deltaTime);

                if (targPos.y < hit.point.y)
                {
                    vrRig.rb.position = targPos;
                    Vector3 targVel = vrRig.rb.velocity;
                    targVel.y = 0;
                    vrRig.rb.velocity = targVel;
                }

                vrRig.rb.drag = 5;

                isGrounded = true;
            }
        }
        else
        {
            vrRig.rb.AddForce(new Vector3(0, -gravityForce, 0));
            vrRig.rb.drag = 0.2f;
            isGrounded = false;
        }


    }
    
}
