using UnityEngine;

public class BucketRed : MonoBehaviour
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

        // spawn the UI icon
        if (UIManager.Instance != null && bucketIconSprite != null)
            UIManager.Instance.AddBucket(bucketIconSprite);
        else
            Debug.LogWarning("UIManager or bucketIconSprite not set on " + name);

        // play sound
        if (audioSource != null && bucketSound != null)
            audioSource.PlayOneShot(bucketSound);
        else
            Debug.LogWarning("Missing audioSource or bucketSound on " + name);

        // finally destroy the bucket object after a short delay
        Destroy(gameObject, 0.5f);
    }
}
