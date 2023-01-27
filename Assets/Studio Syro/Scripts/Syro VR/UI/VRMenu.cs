using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VRMenu : MonoBehaviour
{
    public string playerName;
    //public TextMeshPro playerName;
    public Color playerColor;

    void Start()
    {
        if(PlayerPrefs.HasKey("Player Name"))
        {
            playerName = PlayerPrefs.GetString("Player Name");
        }

        if (PlayerPrefs.HasKey("Player Color R"))
        {
            playerColor = new Color(PlayerPrefs.GetFloat("Player Color R"), PlayerPrefs.GetFloat("Player Color G"), PlayerPrefs.GetFloat("Player Color B"));
        }
    }

    void Update()
    {
        PlayerPrefs.SetString("Player Name", playerName);
        PlayerPrefs.SetFloat("Player Color R", playerColor.r);
        PlayerPrefs.SetFloat("Player Color G", playerColor.g);
        PlayerPrefs.SetFloat("Player Color B", playerColor.b);
    }
}
