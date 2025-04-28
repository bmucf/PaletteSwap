using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public Transform newSpawnPoint;
    public bool teleportPlayer = false;
    private bool triggered = false;

    public GameObject originalPlayer;  // Reference to the original player
    public GameObject tutorialPlayer;  // Reference to the tutorial player

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            SpawnManager spawnManager = FindObjectOfType<SpawnManager>();
            if (spawnManager != null)
            {
                spawnManager.UpdateSpawnPoint(newSpawnPoint.position);
                Debug.Log("Spawn point updated.");
            }

            if (teleportPlayer)
            {
                other.transform.position = newSpawnPoint.position;
                Debug.Log("Player teleported to spawn.");
            }

            // Switch to tutorial player
            if (originalPlayer != null && tutorialPlayer != null)
            {
                originalPlayer.SetActive(false);  // Disable the original player
                tutorialPlayer.SetActive(true);   // Enable the tutorial player
                Debug.Log("Switched to Tutorial Player.");
            }

            triggered = true;
        }
    }
}
