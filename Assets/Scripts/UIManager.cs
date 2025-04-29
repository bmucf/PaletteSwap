using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Bucket UI")]
    public List<GameObject> existingBucketIcons = new List<GameObject>(); // <-- NEW
    private int currentBucketIndex = 0; // <-- NEW

    [Header("Gem UI")]
    public Image redGemSlot, greenGemSlot, yellowGemSlot;

    [Tooltip("Sprite to show before Red gem is collected")]
    public Sprite redGemCageSprite;
    [Tooltip("Sprite to show before Green gem is collected")]
    public Sprite greenGemCageSprite;
    [Tooltip("Sprite to show before Yellow gem is collected")]
    public Sprite yellowGemCageSprite;

    [Tooltip("Sprite to swap in once Red gem is collected")]
    public Sprite redGemSprite;
    [Tooltip("Sprite to swap in once Green gem is collected")]
    public Sprite greenGemSprite;
    [Tooltip("Sprite to swap in once Yellow gem is collected")]
    public Sprite yellowGemSprite;

    [Header("Win Text")]
    public GameObject winText;

    [Header("Root UI Canvas")]
    public GameObject uiRoot;

    public Canvas uiCanvas;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // ONE-TIME GEM WIPE
            PlayerPrefs.DeleteKey("RedGemCollected");
            PlayerPrefs.DeleteKey("GreenGemCollected");
            PlayerPrefs.DeleteKey("YellowGemCollected");
            PlayerPrefs.Save();

            SceneManager.sceneLoaded += OnSceneLoaded;
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Hide everything in MainMenu or WinScene
        bool hide = scene.name == "MainMenu" || scene.name == "WinScene";
        if (uiRoot != null)
            uiRoot.SetActive(!hide);
        else
            Debug.LogWarning("UIManager.uiRoot is null!");

        if (hide)
            return;

        var go = GameObject.Find("WinText");
        if (go != null)
            winText = go;

        uiRoot.SetActive(true);

        // Reset everything
        ClearBuckets();
        UpdateGemUI();

        // Hide buckets initially (this happens when the game starts)
        ToggleBucketIcons(false);

        // Show bucket icons only in Levels 1, 2, 3
        if (scene.name == "Level1" || scene.name == "Level2" || scene.name == "Level3")
        {
            ToggleBucketIcons(true);
        }

        // Reset WinText visibility based on gems
        if (winText != null)
            winText.SetActive(scene.name == "Hub" && AllGemsCollected());
    }

    // Method to toggle bucket icons' visibility
    void ToggleBucketIcons(bool isVisible)
    {
        foreach (var icon in existingBucketIcons)
        {
            if (icon != null)
            {
                icon.SetActive(isVisible);
                Debug.Log($"Bucket Icon {icon.name} is now {(isVisible ? "visible" : "hidden")}");
            }
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            AllGemsCollected();
            Debug.Log("All gems collected cheat");
        }
    }

    // ------------ BUCKETS ------------

    public void RemoveBucketIcon()
    {
        if (currentBucketIndex < existingBucketIcons.Count)
        {
            existingBucketIcons[currentBucketIndex].SetActive(false);
            currentBucketIndex++;
        }
        else
        {
            Debug.LogWarning("No more bucket icons to remove!");
        }
    }

    void ClearBuckets()
    {
        // Reactivate all bucket icons at start
        foreach (var icon in existingBucketIcons)
        {
            if (icon != null)
                icon.SetActive(true);
        }
        currentBucketIndex = 0;

        // Reset bucket counts if needed
        BucketRed.collectedCount = 0;
        BucketGreen.collectedCount = 0;
        BucketYellow.collectedCount = 0;
    }

    // ------------ GEMS ------------

    public void AddGem(string color)
    {
        switch (color)
        {
            case "Red":
                redGemSlot.sprite = redGemSprite;
                PlayerPrefs.SetInt("RedGemCollected", 1);
                break;
            case "Green":
                greenGemSlot.sprite = greenGemSprite;
                PlayerPrefs.SetInt("GreenGemCollected", 1);
                break;
            case "Yellow":
                yellowGemSlot.sprite = yellowGemSprite;
                PlayerPrefs.SetInt("YellowGemCollected", 1);
                break;
        }
        PlayerPrefs.Save();
    }

    void UpdateGemUI()
    {
        if (PlayerPrefs.GetInt("RedGemCollected", 0) == 1)
            redGemSlot.sprite = redGemSprite;

        if (PlayerPrefs.GetInt("GreenGemCollected", 0) == 1)
            greenGemSlot.sprite = greenGemSprite;

        if (PlayerPrefs.GetInt("YellowGemCollected", 0) == 1)
            yellowGemSlot.sprite = yellowGemSprite;
    }

    bool AllGemsCollected()
    {
        return PlayerPrefs.GetInt("RedGemCollected", 0) == 1 &&
               PlayerPrefs.GetInt("GreenGemCollected", 0) == 1 &&
               PlayerPrefs.GetInt("YellowGemCollected", 0) == 1;
    }
}
