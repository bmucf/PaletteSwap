using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateGreen : MonoBehaviour
{
    public int requiredCollectables = 3;
    private bool gateOpened = false;

    public GreenGem gem;

    [Header("Audio")]
    public AudioSource gateAudioSource;
    public AudioClip gateOpenClip;


    private void Update()
    {
        if (!gateOpened && BucketGreen.collectedCount >= requiredCollectables)
        {
            gateOpened = true;
            OpenGate();
        }
    }
    private void OpenGate()
    {
        Debug.Log("Gate opened!");

        // First, visually disable the cage immediately
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        foreach (var collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }

        // Play sound
        if (gateAudioSource != null && gateOpenClip != null)
        {
            gateAudioSource.PlayOneShot(gateOpenClip);
            // Delay destruction to let sound finish
            Destroy(gameObject, gateOpenClip.length);
        }
        else
        {
            Destroy(gameObject);
        }

        if (gem != null)
            gem.Activate();
    }
}
