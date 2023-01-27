using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMainCamera : MonoBehaviour
{
    void Update()
    {
        if (Camera.main)
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
