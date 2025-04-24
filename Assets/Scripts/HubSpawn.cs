using UnityEngine;

public class HubSpawn : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform defaultSpawnPoint;
    public Transform newSpawnPoint;
    public Collider tutorialCompleteTrigger; // The trigger that marks the tutorial complete

    void Start()
    {
        // Check if the tutorial is complete
        bool tutorialDone = PlayerPrefs.GetInt("TutorialComplete", 0) == 1;
        Debug.Log("Tutorial Complete: " + tutorialDone); // Debug the tutorial completion status

        // Spawn the player only if they don't already exist in the scene
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            // Choose spawn point based on tutorial completion
            Vector3 spawnPosition = tutorialDone ? newSpawnPoint.position : defaultSpawnPoint.position;
            Quaternion spawnRotation = tutorialDone ? newSpawnPoint.rotation : defaultSpawnPoint.rotation;

            // Instantiate the player at the spawn point
            Debug.Log("Spawning player at: " + spawnPosition); // Debug the spawn position
            Instantiate(playerPrefab, spawnPosition, spawnRotation);
        }
    }

    // This function is called when the player enters the trigger area
    void OnTriggerEnter(Collider other)
    {
        // Check if the player hit the trigger
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the tutorial complete trigger!"); // Debug the trigger hit

            // Mark the tutorial as complete
            PlayerPrefs.SetInt("TutorialComplete", 1);  // Mark tutorial as complete
            PlayerPrefs.Save();  // Save immediately

            Debug.Log("Tutorial marked as complete!"); // Debug tutorial completion
        }
    }
}

