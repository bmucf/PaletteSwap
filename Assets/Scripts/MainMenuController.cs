using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public CanvasGroup HowToPlayPanel;
    public CanvasGroup CreditsPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("Hub");
    }

    public void HowToPlay()
    {
        HowToPlayPanel.alpha = 1;
        HowToPlayPanel.blocksRaycasts = true;
    }
    public void Back()
    {
        HowToPlayPanel.alpha = 0;
        HowToPlayPanel.blocksRaycasts = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Credits()
    {
        CreditsPanel.alpha = 1;
        CreditsPanel.blocksRaycasts = true;
    }
    public void CreditsBack()
    {
        CreditsPanel.alpha = 0;
        CreditsPanel.blocksRaycasts = false;
    }

}
