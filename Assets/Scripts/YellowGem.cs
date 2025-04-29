using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YellowGem : MonoBehaviour
{
    private Collider gemCollider;

    public GameObject victoryCamera;
    public Animator playerAnimator;
    public MonoBehaviour playerControlScript;
    public float delayBeforeWinScreen = 5f;
    public GameObject victoryPlayerModel;
    public GameObject originalPlayerModel;
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip gemSound;

    private void Start()
    {
        gemCollider = GetComponent<Collider>();
        gemCollider.enabled = false;

        if (victoryCamera != null)
            victoryCamera.SetActive(false);
    }

    public void Activate()
    {
        gemCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gemCollider.enabled && other.CompareTag("Player"))
        {
            StartCoroutine(PlayVictorySequence(other.gameObject));
        }
    }

    private IEnumerator PlayVictorySequence(GameObject player)
    {
        if (audioSource != null && gemSound != null)
        {
            audioSource.PlayOneShot(gemSound);
        }
        else
        {
            Debug.LogWarning("Missing audio source or gem sound!");
        }

        // Disable the original player model
        if (originalPlayerModel != null)
            originalPlayerModel.SetActive(false);

        // Enable the new victory model (the one with the victory animation)
        if (victoryPlayerModel != null)
            victoryPlayerModel.SetActive(true);

        // Disable the movement script of the new player model (if any)
        if (victoryPlayerModel.GetComponent<MonoBehaviour>() != null)
            victoryPlayerModel.GetComponent<MonoBehaviour>().enabled = false;

        gemCollider.enabled = false;

        // Lock the mouse during the cutscene
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Disable all cameras to avoid interference
        foreach (Camera cam in Camera.allCameras)
        {
            cam.enabled = false;
        }

        // Enable the victory camera
        if (victoryCamera != null)
        {
            victoryCamera.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Victory camera not assigned!");
        }

        // Trigger the victory animation
        if (victoryPlayerModel.GetComponent<Animator>() != null)
        {
            victoryPlayerModel.GetComponent<Animator>().SetTrigger("Victory");
        }

        yield return new WaitForSeconds(delayBeforeWinScreen);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        PlayerPrefs.SetInt("YellowGemCollected", 1);
        PlayerPrefs.Save();
        if (UIManager.Instance != null)
            UIManager.Instance.AddGem("Yellow");

        SceneManager.LoadScene("Win");
    }
}
