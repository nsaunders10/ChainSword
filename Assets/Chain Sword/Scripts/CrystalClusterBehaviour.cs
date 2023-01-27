using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalClusterBehaviour : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject shardPrefab;
    public GameObject breakPrefab;
    public float spawnForce = 100;
    public int hp = 5;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void SpawnShard()
    {
        if (hp != 0)
        {
            hp--;
            GameObject newShard = Instantiate(shardPrefab, spawnPoint.position, spawnPoint.rotation);
            Rigidbody shardRB = newShard.GetComponent<Rigidbody>();
            shardRB.AddForce(spawnPoint.up * spawnForce);
        }

        if(hp == 0)
        {
            Instantiate(breakPrefab, spawnPoint.position, spawnPoint.rotation);
            transform.localScale = new Vector3(1, 0.2f, 1);
        }

    }
}
