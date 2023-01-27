using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using NaughtyAttributes;

public class SpawnNormcorePlayer : MonoBehaviour
{
    public Realtime realtime;
    public string roomName = "Syro Room";
    public GameObject playerPrefab;
    Realtime.InstantiateOptions instantiateOptions;

    void Start()
    {
        instantiateOptions = new Realtime.InstantiateOptions
        {
            ownedByClient = true,
            preventOwnershipTakeover = true,
            destroyWhenOwnerLeaves = true,
            destroyWhenLastClientLeaves = true,
            useInstance = realtime,
        };
        realtime.didConnectToRoom += DidConnectToRoom;
    }


    void DidConnectToRoom(Realtime room)
    {
        if (!gameObject.activeInHierarchy || !enabled)
            return;      

        Realtime.Instantiate(playerPrefab.name, instantiateOptions);
    }

    [Button]
    public void JoinRoom()
    {
        realtime.Connect(roomName);
    }
}
