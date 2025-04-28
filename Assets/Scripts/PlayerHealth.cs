using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage()
    {
        health -= 10;

        if (health <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            SceneManager.LoadScene("GameOver");
        }
    }
}
