using UnityEngine;
using UnityEngine.SceneManagement;

public class BlueGem : MonoBehaviour
{ 
    private Collider gemCollider;

    private void Start()
    {
        // Cache the collider and disable it at start
        gemCollider = GetComponent<Collider>();
        gemCollider.enabled = false;
    }

    // This method will be called when the gate opens
    public void Activate()
    {
        gemCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gemCollider.enabled && other.CompareTag("Player"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SceneManager.LoadScene("Win");
        }
    }
}
