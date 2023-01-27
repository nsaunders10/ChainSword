using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using TMPro;

public class KeyboardController : MonoBehaviour
{
    public Material keyMat;
    public Color fontColor;
    public TextMeshPro[] inputTexts;
    public List<GameObject> keys;

    [SerializeField]
    int inputIndex;

    [Button]
    void Start()
    {
        keys.Clear();
        foreach (VRButton key in GetComponentsInChildren<VRButton>())
        {
            keys.Add(key.gameObject);
            key.GetComponent<Renderer>().material = keyMat;
            key.GetComponentInChildren<TextMeshPro>().color = fontColor;
        }
        if (Application.isPlaying)
        {
            ShowIndicator();
        }
    }

    public void SetInputIndex(int index)
    {
        inputIndex = index;
    }

    public void Type(string keyValue)
    {
        inputTexts[inputIndex].text += keyValue;
    }

    public void Delete()
    {
        if (inputTexts[inputIndex].text.Length != 0)
        {
            inputTexts[inputIndex].text = inputTexts[inputIndex].text.Substring(0, inputTexts[inputIndex].text.Length - 1);
        }
    }

    void ShowIndicator()
    {
       // inputTexts[inputIndex].text += "|";
        //Invoke("HideIndicator", 0.5f);
    }

    public void HideIndicator()
    {
        if (inputTexts[inputIndex].text.Length != 0)
        {
            inputTexts[inputIndex].text = inputTexts[inputIndex].text.Replace("|", "");            
        }
        Invoke("ShowIndicator", 0.5f);
    }

}
