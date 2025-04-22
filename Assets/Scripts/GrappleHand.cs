using System.Collections;
using UnityEngine;

public class GrappleHand : MonoBehaviour
{
    public Transform hand;             // Assign: the rigged hand transform
    public float reachDuration = 0.3f; // Time to reach out
    public float returnDuration = 0.2f; // Time to return

    private Vector3 originalLocalPosition;
    private Coroutine stretchRoutine;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip grappleShootSound;

    void Start()
    {
        if (hand != null)
        {
            originalLocalPosition = hand.localPosition;
        }
    }

    public void FireToTarget(Vector3 worldTargetPosition)
    {
        if (stretchRoutine != null)
            StopCoroutine(stretchRoutine);

        stretchRoutine = StartCoroutine(ReachAndReturn(worldTargetPosition));
    }

    IEnumerator ReachAndReturn(Vector3 target)
    {
        Vector3 start = hand.position;
        float elapsed = 0f;


        if (audioSource != null && grappleShootSound != null)
        {
            audioSource.PlayOneShot(grappleShootSound);
        }

        // === Reach ===
        while (elapsed < reachDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / reachDuration;
            hand.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        hand.position = target;

        yield return new WaitForSeconds(0.1f); // Small hold at target if desired

        // === Return ===
        elapsed = 0f;
        Vector3 returnStart = hand.position;
        Vector3 returnEnd = transform.TransformPoint(originalLocalPosition);

        while (elapsed < returnDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / returnDuration;
            hand.position = Vector3.Lerp(returnStart, returnEnd, t);
            yield return null;
        }

        hand.localPosition = originalLocalPosition;
    }
}