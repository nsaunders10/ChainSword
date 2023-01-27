using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VRInputField : MonoBehaviour
{
    KeyboardController myKeyboard;
    public TextMeshPro myInputText;
    public int myInputIndex;
    public string prefsName;

    [Space]
    public bool changeColor;
    public Color pressedColor = Color.white;

    
    void Start()
    {
        myInputText.text = PlayerPrefs.GetString(prefsName);
        myKeyboard = FindObjectOfType<KeyboardController>();

        for (int i= 0; i < myKeyboard.inputTexts.Length; i ++)
        {
            if (myKeyboard.inputTexts[i] == myInputText)
            {
                myInputIndex = i;
            }
        }
    }

    private void Update()
    {
        PlayerPrefs.SetString(prefsName, myInputText.text);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponentInParent<HandBehaviour>())
        {
            myKeyboard.SetInputIndex(myInputIndex);
        }
    }
}
