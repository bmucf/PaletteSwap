using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    // This ensures only one instance of MusicManager exists
    public static MusicManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MusicManager>();
                if (instance == null)
                {
                    GameObject musicManager = new GameObject("MusicManager");
                    instance = musicManager.AddComponent<MusicManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        // Make sure the MusicManager is not destroyed when a new scene is loaded
        DontDestroyOnLoad(gameObject);

        // Subscribe to the scene load event to stop music on level switch
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Optional: Add a reference to your music audio source here
    public AudioSource musicAudioSource;

    // This function is called when a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Stop the music when the scene is loaded (change this as needed for specific scenes)
        StopMusic();
    }

    // Play music
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.clip = musicClip;
            musicAudioSource.Play();
        }
    }

    // Stop the music
    public void StopMusic()
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.Stop();
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event when this object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
