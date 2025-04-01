using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour
{
    public Material targetMaterial; // Assign in Inspector (color to apply)

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Player presses 'E' near pedestal
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return; // Safety check

            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance <= 2f) // Adjust range as needed
            {
                Debug.Log("Pedestal activated");
                if(gameObject.CompareTag("RedPedestal"))
                    ChangeOnlyRedObjects();
                if (gameObject.CompareTag("BluePedestal"))
                    ChangeOnlyBlueObjects();
                if (gameObject.CompareTag("YellowPedestal"))
                    ChangeOnlyYellowObjects();
            }
        }
    }

    private void ChangeOnlyRedObjects()
    {
        GameObject[] redObjects = GameObject.FindGameObjectsWithTag("Red"); // Find all objects tagged "Red"

        foreach (GameObject obj in redObjects)
        {
            MaterialController materialController = obj.GetComponent<MaterialController>();
            if (materialController != null) // Ensure it has the script
            {
                materialController.ChangeMaterial(targetMaterial.color);
            }
        }
    }

    private void ChangeOnlyBlueObjects()
    {
        GameObject[] redObjects = GameObject.FindGameObjectsWithTag("Blue"); // Find all objects tagged "Blue"

        foreach (GameObject obj in redObjects)
        {
            MaterialController materialController = obj.GetComponent<MaterialController>();
            if (materialController != null) // Ensure it has the script
            {
                materialController.ChangeMaterial(targetMaterial.color);
            }
        }
    }

    private void ChangeOnlyYellowObjects()
    {
        GameObject[] redObjects = GameObject.FindGameObjectsWithTag("Yellow"); // Find all objects tagged "Yellow"

        foreach (GameObject obj in redObjects)
        {
            MaterialController materialController = obj.GetComponent<MaterialController>();
            if (materialController != null) // Ensure it has the script
            {
                materialController.ChangeMaterial(targetMaterial.color);
            }
        }
    }
}