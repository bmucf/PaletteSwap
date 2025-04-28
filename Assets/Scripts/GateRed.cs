using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateRed : MonoBehaviour
{
    public int requiredCollectables = 3;
    private bool gateOpened = false;

    public RedGem gem;

    [Header("Audio")]
    public AudioSource gateAudioSource;
    public AudioClip gateOpenClip;


    private void Update()
    {
        if (!gateOpened && BucketRed.collectedCount >= requiredCollectables)
        {
            gateOpened = true;
            OpenGate();
        }
    }

    private void OpenGate()
    {
        Debug.Log("Gate opened!");

        if (gateAudioSource != null && gateOpenClip != null)
        {
            gateAudioSource.PlayOneShot(gateOpenClip);
            // Delay destruction to let the sound play
            Destroy(gameObject, gateOpenClip.length); // Or just use 0.5f if you're unsure of length
        }
        else
        {
            // Fallback destroy if there's no audio
            Destroy(gameObject);
        }

        if (gem != null)
            gem.Activate();
    }
}
