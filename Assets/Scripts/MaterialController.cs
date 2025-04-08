using System.Collections;
using UnityEngine;

public class MaterialController : MonoBehaviour
{
    private Renderer objectRenderer;
    private Color originalColor;
    private bool hasChanged = false; // Tracks if the object has already changed color

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer == null)
        {
            Debug.LogError("No Renderer found on " + gameObject.name);
            return;
        }

        originalColor = objectRenderer.material.color; // Store original color
    }

    public void ChangeMaterial(Color newColor)
    {
        if (hasChanged) return; // Prevent changing the color again

        StartCoroutine(ChangeColorGradually(newColor));
    }

    private IEnumerator ChangeColorGradually(Color targetColor)
    {
        float elapsedTime = 0f;
        float duration = 2f;
        Material tempMaterial = new Material(objectRenderer.material); // Prevent modifying shared material

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            tempMaterial.color = Color.Lerp(originalColor, targetColor, elapsedTime / duration);
            objectRenderer.material = tempMaterial; // Apply new material
            yield return null;
        }

        hasChanged = true; // Mark the object as permanently changed
        Debug.Log(gameObject.name + " color permanently changed.");
    }
}