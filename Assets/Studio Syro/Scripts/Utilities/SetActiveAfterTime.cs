using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveAfterTime : MonoBehaviour
{

    public GameObject target;
    public float time;

    void OnEnable()
    {
        target.SetActive(false);
        Invoke("SetActive", time);
    }

    void SetActive()
    {
        target.SetActive(true);
    }

}
