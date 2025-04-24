using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public Transform newSpawnPoint;
    public bool teleportPlayer = false;
    private bool triggered = false;

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

            triggered = true;
        }
    }
}