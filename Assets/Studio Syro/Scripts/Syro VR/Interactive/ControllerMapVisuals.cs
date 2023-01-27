using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMapVisuals : MonoBehaviour
{
    public HandBehaviour myHand;
    public Transform trigger;
    public Transform grip;
    public Transform stick;
    public Transform top;
    public Transform bottom;
    public Transform menu;

    void Start()
    {
        
    }

    
    void Update()
    {
        trigger.GetChild(0).gameObject.SetActive(!myHand.trigger.held);
        trigger.GetChild(2).gameObject.SetActive(myHand.trigger.held);

        grip.GetChild(0).gameObject.SetActive(!myHand.grip.held);
        grip.GetChild(2).gameObject.SetActive(myHand.grip.held);

        stick.GetChild(0).gameObject.SetActive(!myHand.stick.held);
        stick.GetChild(2).gameObject.SetActive( myHand.stick.held);

        top.GetChild(0).gameObject.SetActive(!myHand.top.held);
        top.GetChild(2).gameObject.SetActive( myHand.top.held);

        bottom.GetChild(0).gameObject.SetActive(!myHand.bottom.held);
        bottom.GetChild(2).gameObject.SetActive( myHand.bottom.held);

        menu.GetChild(0).gameObject.SetActive(!myHand.menu.held);
        menu.GetChild(2).gameObject.SetActive( myHand.menu.held);
    }
}
