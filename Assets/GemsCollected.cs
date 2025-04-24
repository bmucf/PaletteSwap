using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemsCollected : MonoBehaviour
{
    public GameObject redGemDisplay;
    public GameObject greenGemDisplay;
    public GameObject yellowGemDisplay;

    private void Start()
    {
        redGemDisplay.SetActive(PlayerPrefs.GetInt("RedGemCollected", 0) == 1);
        greenGemDisplay.SetActive(PlayerPrefs.GetInt("GreenGemCollected", 0) == 1);
        yellowGemDisplay.SetActive(PlayerPrefs.GetInt("YellowGemCollected", 0) == 1);
    }
}