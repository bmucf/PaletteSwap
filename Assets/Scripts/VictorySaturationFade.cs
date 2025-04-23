using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Camera))]
public class VictorySaturationFade : MonoBehaviour
{
    [Tooltip("Your Global Volume")]
    public Volume volume;
    [Tooltip("Seconds to fade to full color")]
    public float fadeDuration = 2f;

    void Awake()
    {
        if (volume == null) volume = FindObjectOfType<Volume>();
        // start fully desaturated
        volume.weight = 1f;
    }

    void OnEnable()
    {
        Debug.Log("[VSF] OnEnable – starting fade, initial weight=" + volume.weight);
        StartCoroutine(FadeOutVolume());
    }

    IEnumerator FadeOutVolume()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            volume.weight = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            Debug.Log($"[VSF] t={elapsed:f2}/{fadeDuration} weight={volume.weight:F2}");
            yield return null;
        }
        volume.weight = 0f;
        Debug.Log("[VSF] Fade complete, weight=" + volume.weight);
    }
}