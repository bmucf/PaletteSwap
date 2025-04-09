using UnityEngine;

public class MenuManager : MonoBehaviour
{
    bool isPaused;
    public CanvasGroup PauseUI;
    //GameObject player;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //player = GameObject.Find("Player");
        Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
        PauseUI.alpha = 0;
        PauseUI.blocksRaycasts = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        PauseUI.alpha = 1;
        PauseUI.blocksRaycasts = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
