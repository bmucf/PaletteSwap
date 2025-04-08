using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage()
    {
        health -= 10; // or whatever value you want to reduce

        if (health <= 0)
        {
            // Restart the current scene instantly when health reaches 0
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
