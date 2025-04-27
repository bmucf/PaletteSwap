using System.Collections;
using UnityEngine;

public class GrappleHand : MonoBehaviour
{
    public Transform hand;             // Assign: the rigged hand transform
    public float reachDuration = 0.3f; // Time to reach out
    public float returnDuration = 0.2f; // Time to return
    public float maxStretch = 3f;      // Max stretch factor for the hand

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
        Vector3 startHandPosition = hand.position;
        Quaternion startHandRotation = hand.rotation;

        float elapsed = 0f;

        if (audioSource != null && grappleShootSound != null)
        {
            audioSource.PlayOneShot(grappleShootSound);
        }

        // === Reach ===
        while (elapsed < reachDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / reachDuration);

            // Calculate the direction from the player to the grapple point (in world space)
            Vector3 directionToTarget = (target - startHandPosition).normalized;

            // Stretch the hand towards the target
            float currentStretch = Mathf.Lerp(1f, maxStretch, t); // Smoothly increase stretch

            // Apply the stretch to the hand
            hand.localScale = new Vector3(currentStretch, 1f, 1f);

            // Rotate the hand to face the target
            hand.rotation = Quaternion.Slerp(startHandRotation, Quaternion.LookRotation(directionToTarget), t);

            // Move the hand directly towards the target (grapple point in world space)
            hand.position = Vector3.Lerp(startHandPosition, target, t);

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
            float t = Mathf.Clamp01(elapsed / returnDuration);
            hand.position = Vector3.Lerp(returnStart, returnEnd, t);
            yield return null;
        }

        hand.localPosition = originalLocalPosition;
        hand.localScale = Vector3.one;  // Reset stretch for hand
    }
}
