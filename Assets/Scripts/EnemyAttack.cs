using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Animator animator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage();
                animator.SetTrigger("Die");
            }
        }
    }
}
