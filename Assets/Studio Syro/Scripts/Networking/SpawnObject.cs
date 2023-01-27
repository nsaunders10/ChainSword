using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using NaughtyAttributes;

public class SpawnObject : MonoBehaviour
{

    Realtime realtime;
    public Transform spawnPoint;
    public GameObject[] prefabs;
    GameObject spawnedObject;
    public bool onDestroy;

    void OnDestroy()
    {
        if (Application.isPlaying)
        {
            if (onDestroy)
            {
                Spawn();
            }
        }
    }

    [Button]
    public void Spawn()
    {
        realtime = FindObjectOfType<Realtime>();

        if (realtime)
        {
            if (realtime.connected)
            {

                int randomIndex = Random.Range(0, prefabs.Length);

                spawnedObject = Realtime.Instantiate(prefabs[randomIndex].name, new Realtime.InstantiateOptions
                {
                    ownedByClient = true,
                    preventOwnershipTakeover = false,
                    destroyWhenOwnerLeaves = false,
                    destroyWhenLastClientLeaves = true,
                    useInstance = realtime,
                });
                spawnedObject.GetComponent<RealtimeTransform>().RequestOwnership();
                spawnedObject.transform.position = spawnPoint.position;
                spawnedObject.transform.rotation = spawnPoint.rotation;
            }
        }
        else
        {
            int randomIndex = Random.Range(0, prefabs.Length);

            spawnedObject = Instantiate(prefabs[randomIndex]);
            spawnedObject.transform.position = spawnPoint.position;
            spawnedObject.transform.rotation = spawnPoint.rotation;
        }

    }
}
