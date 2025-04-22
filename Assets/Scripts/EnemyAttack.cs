using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Animator animator;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip attackSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                audioSource.PlayOneShot(attackSound);
                health.TakeDamage();
                animator.SetTrigger("Die");
            }
        }
    }
}
