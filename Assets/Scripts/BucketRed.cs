using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketRed : MonoBehaviour
{
    public static int collectedCount = 0;
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip bucketSound;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collectedCount++;

            if (audioSource != null && bucketSound != null)
            {
                audioSource.PlayOneShot(bucketSound);
            }
            else
            {
                Debug.LogWarning("Missing audioSource or bucketSound on " + gameObject.name);
            }
            Destroy(gameObject, 0.5f);
        }
    }
}