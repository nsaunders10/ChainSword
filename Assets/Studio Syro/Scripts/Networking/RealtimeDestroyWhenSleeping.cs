using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class RealtimeDestroyWhenSleeping : MonoBehaviour
{
    public float destroyTime = 30;
    RealtimeView realtimeView;
    bool startedTimer;

    private void Start()
    {
        realtimeView = GetComponent<RealtimeView>();
    }

    void Update()
    {
        if(realtimeView.isUnownedSelf && !startedTimer)
        {
            startedTimer = true;
            Invoke("DestroyAfterTime", destroyTime);
        }

        if (!realtimeView.isUnownedSelf)
        {
            startedTimer = false;
            CancelInvoke("DestroyAfterTime");
        }
    }

    void DestroyAfterTime()
    {
        Realtime.Destroy(gameObject);
    }
}
