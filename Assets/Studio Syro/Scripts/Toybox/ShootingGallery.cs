using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using TMPro;

public class ShootingGallery : MonoBehaviour
{
    Realtime realtime;
    RealtimeView realtimeView;
    Realtime.InstantiateOptions instantiateOptions;

    public GameObject countDownStartPrefab;

    public IntSync timerValue;
    public IntSync syncScoreOne;
    public IntSync syncScoreTwo;

    public int playerOneScore;
    public int playerTwoScore;

    public int currentTime;
    public bool startCalled;

    void Start()
    {
        instantiateOptions = new Realtime.InstantiateOptions
        {
            ownedByClient = true,
            preventOwnershipTakeover = false,
            destroyWhenOwnerLeaves = false,
            destroyWhenLastClientLeaves = true,
            useInstance = realtime,
        };
    }

    
    void Update()
    {
        if (realtimeView.isOwnedLocallySelf)
        {
            if (startCalled)
            {
                startCalled = false;
                syncScoreOne.SetValue(0);
                syncScoreTwo.SetValue(0);
                InvokeRepeating("TickTimer", 3, 1);
                Realtime.Instantiate(countDownStartPrefab.name, instantiateOptions);

            }
        }
    }

    void TickTimer()
    {
        currentTime--;
    }

    public void StartTimer()
    {
        realtimeView.RequestOwnership();
        startCalled = true;
    }
}
