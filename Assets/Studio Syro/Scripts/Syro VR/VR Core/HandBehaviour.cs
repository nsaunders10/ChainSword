using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBehaviour : MonoBehaviour
{
    public VRRig vrRig;

    public enum InputType { Trigger, Grip, Stick, Top, Bottom, Menu }
    public enum Handedness { Left, Right }
    [Space]
    public Handedness handedness;
    public HandBehaviour otherHand;
    [HideInInspector]
    public HandInteraction interaction;
    public Transform visuals;

    [Space]
    //Calculate Velocity
    public Vector3 velocity;
    Vector3 lastPos;
    public Vector3 angularVelocity;
    Vector3 eulerLast;

    [Header("Inputs")]
    public VRInput trigger;
    public VRInput grip;
    public VRInput stick;
    public VRInput top;
    public VRInput bottom;
    public VRInput menu;

    string[] inputNames =  new string[28];

    private void Start()
    {
        interaction = GetComponent<HandInteraction>();
        char hand = handedness.ToString()[0];
        //Trigger
        inputNames[0] = hand + "Trigger";
        inputNames[1] = hand + "TriggerButton";
        inputNames[2] = hand + "TriggerButton";
        inputNames[3] = hand + "TriggerButton";

        //Grip
        inputNames[4] = hand + "Grip";
        inputNames[5] = hand + "GripButton";
        inputNames[6] = hand + "GripButton";
        inputNames[7] = hand + "GripButton";

        //Stick
        inputNames[8] = hand + "StickX";
        inputNames[9] = hand + "StickY";
        inputNames[10] = hand + "StickButton";
        inputNames[11] = hand + "StickButton";
        inputNames[12] = hand + "StickButton";
        inputNames[13] = hand + "StickTouch";
        inputNames[14] = hand + "StickTouch";
        inputNames[15] = hand + "StickTouch";

        //Top Button
        inputNames[16] = hand + "TopButton";
        inputNames[17] = hand + "TopButton";
        inputNames[18] = hand + "TopButton";
        inputNames[19] = hand + "TopTouch";
        inputNames[20] = hand + "TopTouch";
        inputNames[21] = hand + "TopTouch";

        //Bottom Button
        inputNames[22] = hand + "BottomButton";
        inputNames[23] = hand + "BottomButton";
        inputNames[24] = hand + "BottomButton";
        inputNames[25] = hand + "BottomTouch";
        inputNames[26] = hand + "BottomTouch";
        inputNames[27] = hand + "BottomTouch";
    }

    void Update()
    {
        GetInputs();
        CalculateHandVelocity();
    }

    void GetInputs()
    {
        //Trigger
        trigger.analogValue = Input.GetAxis(inputNames[0]);
        trigger.held = Input.GetButton(inputNames[1]);
        trigger.down = Input.GetButtonDown(inputNames[2]);
        trigger.up = Input.GetButtonUp(inputNames[3]);

        //Grip
        grip.analogValue = Input.GetAxis(inputNames[4]);
        grip.held = Input.GetButton(inputNames[5]);
        grip.down = Input.GetButtonDown(inputNames[6]);
        grip.up = Input.GetButtonUp(inputNames[7]);

        //Stick
        stick.axis.x = Input.GetAxis(inputNames[8]);
        stick.axis.y = Input.GetAxis(inputNames[9]);
        stick.held = Input.GetButton(inputNames[10]);
        stick.down = Input.GetButtonDown(inputNames[11]);
        stick.up = Input.GetButtonUp(inputNames[12]);
        stick.touch.held = Input.GetButton(inputNames[13]);
        stick.touch.down = Input.GetButtonDown(inputNames[14]);
        stick.touch.up = Input.GetButtonUp(inputNames[15]);

        //Top Button
        top.held = Input.GetButton(inputNames[16]);
        top.down = Input.GetButtonDown(inputNames[17]);
        top.up = Input.GetButtonUp(inputNames[18]);
        top.touch.held = Input.GetButton(inputNames[19]);
        top.touch.down = Input.GetButtonDown(inputNames[20]);
        top.touch.up = Input.GetButtonUp(inputNames[21]);

        //Bottom Button
        bottom.held = Input.GetButton(inputNames[22]);
        bottom.down = Input.GetButtonDown(inputNames[23]);
        bottom.up = Input.GetButtonUp(inputNames[24]);
        bottom.touch.held = Input.GetButton(inputNames[25]);
        bottom.touch.down = Input.GetButtonDown(inputNames[26]);
        bottom.touch.up = Input.GetButtonUp(inputNames[27]);

        //Menu Button
        menu.held = Input.GetButton("LMenuButton");
        menu.down = Input.GetButtonDown("LMenuButton");
        menu.up = Input.GetButtonUp("LMenuButton");
    }

    void CalculateHandVelocity()
    {
        if (vrRig)
        {
            //Calculate Hand Velocity
            velocity = (transform.localPosition - lastPos) / Time.deltaTime;
            velocity = Quaternion.Euler(vrRig.transform.eulerAngles.x, vrRig.transform.eulerAngles.y, vrRig.transform.eulerAngles.z) * velocity;
           
            lastPos = transform.localPosition;

            //Angular Velocity
            angularVelocity = (transform.eulerAngles - eulerLast) / Time.deltaTime;
            eulerLast = transform.eulerAngles;
        }
    }

    public VRInput TranslateInput(InputType inputType)
    {
        VRInput targetInput;
        if (inputType == InputType.Trigger)
        {
            targetInput = trigger;
            return targetInput;
        }
        if (inputType == InputType.Grip)
        {
            targetInput = grip;
            return targetInput;
        }
        if (inputType == InputType.Stick)
        {
            targetInput = stick;
            return targetInput;
        }
        if (inputType == InputType.Top)
        {
            targetInput = top;
            return targetInput;
        }
        if (inputType == InputType.Bottom)
        {
            targetInput = bottom;
            return targetInput;
        }
        if (inputType == InputType.Menu)
        {
            targetInput = menu;
            return targetInput;
        }
        else
        {

            return null;
        }
    }
}
