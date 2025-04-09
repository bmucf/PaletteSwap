using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBlue : MonoBehaviour
{
    public static int collectedCount = 0;

    // Ensure the OnTriggerEnter method is used
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with the collectable is the player
        if (other.CompareTag("Player"))
        {
            // Increment the collected count
            collectedCount++;

            // Optionally destroy the collectable object to indicate it was collected
            Destroy(gameObject);
        }
    }
}

