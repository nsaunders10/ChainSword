using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class DestroyAfterTime : MonoBehaviour
{
    public float time = 3;
    public bool dontUseTime;

    void Start()
    {
        if (!dontUseTime) 
            Invoke("DestroyObject", time);
    }

    [Button]
    void DestroyObject()
    {
        Destroy(gameObject);
    }
}

