using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketGreen : MonoBehaviour
{
    public static int collectedCount = 0;
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip bucketSound;

    // Ensure the OnTriggerEnter method is used
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with the collectable is the player
        if (other.CompareTag("Player"))
        {
            collectedCount++;
            audioSource.PlayOneShot(bucketSound);
            Destroy(gameObject);
        }
    }
}

