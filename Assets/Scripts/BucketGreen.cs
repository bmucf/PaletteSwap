using UnityEngine;

public class BucketGreen : MonoBehaviour
{
    public static int collectedCount = 0;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip bucketSound;

    [Header("UI")]
    public Sprite bucketIconSprite;

    private bool hasCollected = false;
    private Collider _col;

    private void Start()
    {
        _col = GetComponent<Collider>();
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasCollected) return;
        if (!other.CompareTag("Player")) return;

        hasCollected = true;
        _col.enabled = false;

        collectedCount++;

        if (UIManager.Instance != null)
            UIManager.Instance.RemoveBucketIcon();
        else
            Debug.LogWarning("UIManager not set on " + name);

        // Play sound
        if (audioSource != null && bucketSound != null)
            audioSource.PlayOneShot(bucketSound);
        else
            Debug.LogWarning("Missing audioSource or bucketSound on " + name);
        Destroy(gameObject);
    }
}