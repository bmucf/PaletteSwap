using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Bucket UI")]
    public RectTransform bucketPanel;
    public GameObject bucketIconPrefab;

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

    private List<GameObject> _bucketIcons = new List<GameObject>();
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
        ClearBuckets();

        // reset cages, swap collected gems
        if (winText != null)
            winText.SetActive(scene.name == "Hub" && AllGemsCollected());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            AllGemsCollected();
            Debug.Log("All gems collected cheat");
        }
    }
    public void AddBucket(Sprite iconSprite)
    {
        var go = Instantiate(bucketIconPrefab, bucketPanel);
        var img = go.GetComponent<Image>();
        if (img != null) img.sprite = iconSprite;
        _bucketIcons.Add(go);
    }

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

    void ClearBuckets()
    {
        foreach (var go in _bucketIcons) Destroy(go);
        _bucketIcons.Clear();

        BucketRed.collectedCount = 0;
        BucketGreen.collectedCount = 0;
        BucketYellow.collectedCount = 0;
    }

    bool AllGemsCollected()
    {
        return PlayerPrefs.GetInt("RedGemCollected", 0) == 1 &&
               PlayerPrefs.GetInt("GreenGemCollected", 0) == 1 &&
               PlayerPrefs.GetInt("YellowGemCollected", 0) == 1;
    }
}
