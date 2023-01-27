using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.SceneManagement;

public class DisconnectedCheck : MonoBehaviour
{
    bool connected;
    Realtime realtime;

    void Start()
    {
        realtime = FindObjectOfType<Realtime>();  
    }

    
    void Update()
    {
        if (realtime.connected && !connected)
        {
            connected = true;
        }

        if(!realtime.connected && connected)
        {
            SceneManager.LoadScene(0);
        }
    }
}
