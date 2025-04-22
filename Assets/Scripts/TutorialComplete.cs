using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialComplete : MonoBehaviour

{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("TutorialComplete", 1);
            PlayerPrefs.Save();

            Debug.Log("Tutorial completed!");
        }
    }
}
