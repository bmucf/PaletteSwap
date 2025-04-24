using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private Vector3 currentSpawnPoint;

    private void Start()
    {
        currentSpawnPoint = transform.position;
    }

    public void UpdateSpawnPoint(Vector3 newSpawn)
    {
        currentSpawnPoint = newSpawn;
    }

    public Vector3 GetSpawnPoint()
    {
        return currentSpawnPoint;
    }
}