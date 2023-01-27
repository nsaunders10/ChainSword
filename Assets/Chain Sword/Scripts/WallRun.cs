using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    VRRig vrRig;
    PlayerController player;
    bool wallRunning;
    float wallRunForce;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Wall Run
        Vector3 raypos = vrRig.hmd.position;
        raypos.y = vrRig.transform.position.y;

        RaycastHit hit;
        Ray ray = new Ray(raypos, vrRig.hmd.right * 0.5f);

        Debug.DrawRay(ray.origin, ray.direction, Color.white);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 0.5f))
        {
            if (!wallRunning)
            {
                wallRunning = true;
                player.currentJumps = 1;
                Vector3 targetVel = vrRig.rb.velocity;
                targetVel.y = 0;
                vrRig.rb.velocity = targetVel;
            }

            if (hit.distance < 0.3f)
            {
                vrRig.rb.AddForce((vrRig.transform.up) * wallRunForce);
                //vrRig.rb.AddForce((-ray.direction) * (wallRunForce));
            }
            else
            {
                //vrRig.rb.AddForce((ray.direction) * (wallRunForce / 2));
            }

        }
        else
        {
            wallRunning = false;
        }
    }
}
