using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Grabable))]
public class EventOnButtonPress : MonoBehaviour
{
    Grabable grabable;
    public enum Buttons { Trigger, Grip, Stick, Top, Bottom }
    public Buttons targetButton;
    public UnityEvent onButtonDown;

    void Start()
    {
        grabable = GetComponent<Grabable>();
    }


    void Update()
    {
        if (grabable.holdingHand)
        {
            if (targetButton == Buttons.Trigger)
            {
                if (grabable.holdingHand.trigger.down)
                {
                    onButtonDown.Invoke();
                }
            }

            if (targetButton == Buttons.Grip)
            {
                if (grabable.holdingHand.grip.down)
                {
                    onButtonDown.Invoke();
                }
            }

            if (targetButton == Buttons.Stick)
            {
                if (grabable.holdingHand.stick.down)
                {
                    onButtonDown.Invoke();
                }
            }

            if (targetButton == Buttons.Top)
            {
                if (grabable.holdingHand.top.down)
                {
                    onButtonDown.Invoke();
                }
            }

            if (targetButton == Buttons.Bottom)
            {
                if (grabable.holdingHand.bottom.down)
                {
                    onButtonDown.Invoke();
                }
            }
        }
    }
}

