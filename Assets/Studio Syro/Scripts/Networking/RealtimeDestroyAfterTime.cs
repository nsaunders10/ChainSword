using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using NaughtyAttributes;

public class RealtimeDestroyAfterTime : MonoBehaviour
{
    public float time = 3;
    public bool dontUseTime;
   
    void Start()
    {
        if(!dontUseTime)
            Invoke("DestroyObject", time);
    }

    [Button]
    void DestroyObject()
    {
        Realtime.Destroy(gameObject);
    }
}
