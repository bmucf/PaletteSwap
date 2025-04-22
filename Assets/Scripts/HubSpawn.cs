using UnityEngine;

public class HubSpawn : MonoBehaviour
{
    public GameObject playerPrefab; 
    public Transform defaultSpawnPoint;
    public Transform newSpawnPoint;

    void Awake()
    {
        PlayerPrefs.SetInt("TutorialComplete", 0); // Always reset on boot
        PlayerPrefs.Save(); // Save immediately
    }

    void Start()
    {
        bool tutorialDone = PlayerPrefs.GetInt("TutorialComplete", 0) == 1;

        Vector3 spawnPosition = tutorialDone ? newSpawnPoint.position : defaultSpawnPoint.position;
        Quaternion spawnRotation = tutorialDone ? newSpawnPoint.rotation : defaultSpawnPoint.rotation;

        Instantiate(playerPrefab, spawnPosition, spawnRotation);
    }

}
