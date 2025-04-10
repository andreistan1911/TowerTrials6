using UnityEngine;

public class Health : MonoBehaviour
{
    [HideInInspector]
    public float maxHealth;

    [HideInInspector]
    public float currentHealth;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}