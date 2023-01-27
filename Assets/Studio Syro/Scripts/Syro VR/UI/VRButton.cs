using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using NaughtyAttributes;

public class VRButton : MonoBehaviour
{
    public bool changeColor = true;
    public Color pressedColor = Color.white;   
    Color startColor;
    Color targetColor;
    Material myMat;
    bool pressed;
    float lerpSpeed = 30;
    public VRRig pressSource;

    [Space]
    public UnityEvent OnClickDown;
    public UnityEvent OnClickUp;
    public string buttonTitle;
    public TextMeshPro buttonText;

    [Button]
    void SetText()
    {
        name = buttonTitle + " Button";
        buttonText.text = buttonTitle;
    }

    void Start()
    {
        if(buttonTitle !="")
        SetText();

        if (GetComponent<Renderer>())
        {
            myMat = GetComponent<Renderer>().material;
            startColor = myMat.color;
            targetColor = startColor;
        }
    }

    void Update()
    {
        if (myMat)
        {
            if (changeColor)
            {
                if (pressed)
                {
                    targetColor = Color.Lerp(targetColor, pressedColor, lerpSpeed * Time.deltaTime);
                }
                if (!pressed)
                {
                    targetColor = Color.Lerp(targetColor, startColor, lerpSpeed * Time.deltaTime);
                }
                GetComponent<Renderer>().material.color = targetColor;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponentInParent<HandBehaviour>())
        {
            pressSource = other.transform.GetComponentInParent<VRRig>();
            OnClickDown.Invoke();
            pressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponentInParent<HandBehaviour>())
        {
            pressSource = other.transform.GetComponentInParent<VRRig>();
            OnClickUp.Invoke();
            pressed = false;
        }
    }
}
